using Decryptor.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class Pbkdf2Hash : IHash
    {
        private readonly int _hashSize;
        private readonly int _iterations;
        private readonly string _prefix;
        private readonly int _saltSize;

        public Pbkdf2Hash(int iterations, int saltSize, int hashSize, string prefix)
        {
            _iterations = iterations;
            _saltSize = saltSize;
            _hashSize = hashSize;
            _prefix = prefix;
        }

        public async Task<bool> CheckFileHashAsync(string filename, string hash)
        {
            // Check hash
            if (!IsHashSupported(hash))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            byte[] bytes = await File.ReadAllBytesAsync(filename);
            return CheckBytes(hash, bytes);
        }

        public Task<bool> CheckHashAsync(string clearText, string hash)
        {
            // Check hash
            if (!IsHashSupported(hash))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            return Task.Run(() =>
            {
                // Extract iteration and Base64 string
                var splitHashString = hash.Replace(_prefix, "").Split('$');
                var iterations = int.Parse(splitHashString[0]);
                var base64hash = splitHashString[1];

                // Get hash bytes
                var hashBytes = Convert.FromBase64String(base64hash);

                // Get salt
                var salt = new byte[_saltSize];
                Array.Copy(hashBytes, 0, salt, 0, _saltSize);

                // Create hash with given salt
                var pbdkf2 = new Rfc2898DeriveBytes(clearText, salt, iterations);
                byte[] hash2 = pbdkf2.GetBytes(_hashSize);

                for (int i = 0; i < _hashSize; i++)
                {
                    if (hashBytes[i + _saltSize] != hash2[i])
                    {
                        return false;
                    }
                }

                // Get result

                return true;
            });
        }

        public async Task<bool> CheckHashAsync(Stream stream, string hash)
        {
            // Check hash
            if (!IsHashSupported(hash))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            using MemoryStream ms = new();
            await stream.CopyToAsync(ms);
            return CheckBytes(hash, ms.ToArray());
        }

        public async Task<string> GetFileHashAsync(string filename)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[_saltSize]);
            //salt = RandomNumberGenerator.GetBytes(_saltSize);

            byte[] bytes = await File.ReadAllBytesAsync(filename);
            return HashBytes(salt, bytes);
        }

        public Task<string> GetHashAsync(string clearText)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[_saltSize]);
            //salt = RandomNumberGenerator.GetBytes(_saltSize);

            return Task.Run(() =>
            {
                // Create hash
                Rfc2898DeriveBytes pbkdf2 = new(clearText, salt, _iterations);
                byte[] hash = pbkdf2.GetBytes(_hashSize);

                // Combine salt and hash
                byte[] hashBytes = new byte[_saltSize + _hashSize];
                Array.Copy(salt, 0, hashBytes, 0, _saltSize);
                Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize);

                // Convert to base64
                string base64Hash = Convert.ToBase64String(hashBytes);

                return $"{_prefix}{_iterations}${base64Hash}";
            });
        }

        public async Task<string> GetHashAsync(Stream stream)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[_saltSize]);
            //salt = RandomNumberGenerator.GetBytes(_saltSize);

            using MemoryStream ms = new();
            await stream.CopyToAsync(ms);
            return HashBytes(salt, ms.ToArray());
        }

        private bool CheckBytes(string hash, byte[] bytes)
        {
            // Extract iteration and Base64 string
            var splitHashString = hash.Replace(_prefix, "").Split('$');
            var iterations = int.Parse(splitHashString[0]);
            var base64hash = splitHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64hash);

            // Get salt
            var salt = new byte[_saltSize];
            Array.Copy(hashBytes, 0, salt, 0, _saltSize);

            // Create hash with given salt
            var pbdkf2 = new Rfc2898DeriveBytes(bytes, salt, iterations);
            byte[] hash2 = pbdkf2.GetBytes(_hashSize);

            for (int i = 0; i < _hashSize; i++)
            {
                if (hashBytes[i + _saltSize] != hash2[i])
                {
                    return false;
                }
            }

            // Get result

            return true;
        }

        private string HashBytes(byte[] salt, byte[] bytes)
        {
            // Create hash
            Rfc2898DeriveBytes pbkdf2 = new(bytes, salt, _iterations);
            byte[] hash = pbkdf2.GetBytes(_hashSize);

            // Combine salt and hash
            byte[] hashBytes = new byte[_saltSize + _hashSize];
            Array.Copy(salt, 0, hashBytes, 0, _saltSize);
            Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize);

            // Convert to base64
            string base64Hash = Convert.ToBase64String(hashBytes);

            return $"{_prefix}{_iterations}${base64Hash}";
        }

        private bool IsHashSupported(string hashString)
        {
            if (hashString == null)
            {
                return false;
            }
            return hashString.Contains(_prefix);
        }
    }
}