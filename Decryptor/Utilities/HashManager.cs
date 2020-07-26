using DevOne.Security.Cryptography.BCrypt;
using Konscious.Security.Cryptography;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Printing.IndexedProperties;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Decryptor.Utilities
{
    public class HashManager
    {
        private readonly HashAlgorithm _algorithm;
        private readonly int _workFactor;
        private readonly int _scryptIterations;
        private readonly int _blockCount;
        private readonly int _threadCount;
        private readonly int _degreesOfParallelism;
        private readonly int _argon2Iterations;
        private readonly int _memorySize;
        private readonly int _saltLength;
        private readonly int _hashLength;

        public HashManager(HashAlgorithm algorithm, int workFactor, int scryptIterations, int blockCount,
                           int threadCount, int degrees, int argon2Iterations, int memorySize, int saltLength,
                           int hashLength)
        {
            _algorithm = algorithm;
            _workFactor = workFactor;
            _scryptIterations = scryptIterations;
            _blockCount = blockCount;
            _threadCount = threadCount;
            _degreesOfParallelism = degrees;
            _argon2Iterations = argon2Iterations;
            _memorySize = memorySize;
            _saltLength = saltLength;
            _hashLength = hashLength;
        }

        public static HashManager NewHashManager(HashAlgorithm algorithm)
        {
            return new HashManager(algorithm,
                                   Properties.Settings.Default.BCryptWorkFactor,
                                   Properties.Settings.Default.ScryptIterationCount,
                                   Properties.Settings.Default.ScryptBlockCount,
                                   Properties.Settings.Default.ScryptThreadCount,
                                   Properties.Settings.Default.Argon2DegreesOfParallelism,
                                   Properties.Settings.Default.Argon2Iterations,
                                   Properties.Settings.Default.Argon2MemorySize,
                                   Properties.Settings.Default.Argon2SaltLength,
                                   Properties.Settings.Default.Argon2HashLength);
        }

        public async Task<string> GetHashAsync(string input)
        {
            return _algorithm switch
            {
                HashAlgorithm.BCrypt => await GetBCryptHashAsync(input),
                HashAlgorithm.Scrypt => await GetScryptHashAsync(input),
                HashAlgorithm.Argon2 => await GetArgon2HashAsync(input),
                HashAlgorithm.MD5 => await GetMD5HashAsync(input),
                HashAlgorithm.SHA1 => await GetSHA1HashAsync(input),
                HashAlgorithm.SHA256 => await GetSHA256HashAsync(input),
                HashAlgorithm.SHA512 => await GetSHA512HashAsync(input),
                HashAlgorithm.None => throw new NotImplementedException(),
                _ => throw new InvalidOperationException($"{_algorithm} is not a valid algorithm"),
            };
        }

        public async Task<bool> CheckHashAsync(string clearText, string hash)
        {
            return _algorithm switch
            {
                HashAlgorithm.BCrypt => await CheckBCryptHashAsync(clearText, hash),
                HashAlgorithm.Scrypt => await CheckScryptHashAsync(clearText, hash),
                HashAlgorithm.Argon2 => await CheckArgon2HashAsync(clearText, hash),
                HashAlgorithm.MD5 => await CheckMD5HashAsync(clearText, hash),
                HashAlgorithm.SHA1 => await CheckSHA1HashAsync(clearText, hash),
                HashAlgorithm.SHA256 => await CheckSHA256HashAsync(clearText, hash),
                HashAlgorithm.SHA512 => await CheckSHA512HashAsync(clearText, hash),
                HashAlgorithm.None => throw new NotImplementedException(),
                _ => throw new InvalidOperationException($"{_algorithm} is not a valid algorithm"),
            };
        }

        private Task<string> GetBCryptHashAsync(string clearText)
        {
            string salt = BCryptHelper.GenerateSalt(_workFactor);
            return Task.Run(() => BCryptHelper.HashPassword(clearText, salt));
        }

        private Task<bool> CheckBCryptHashAsync(string clearText, string hash)
        {
            try
            {
                return Task.Run(() => BCryptHelper.CheckPassword(clearText, hash));
            }
            catch (ArgumentException)
            {
                return Task.FromResult(false);
            }
        }

        private Task<string> GetScryptHashAsync(string input)
        {
            ScryptEncoder encoder = new ScryptEncoder(_scryptIterations, _blockCount, _threadCount);
            return Task.Run(() => encoder.Encode(input));
        }

        private Task<bool> CheckScryptHashAsync(string clearText, string hash)
        {
            ScryptEncoder encoder = new ScryptEncoder(_scryptIterations, _blockCount, _threadCount);
            try
            {
                return Task.Run(() => encoder.Compare(clearText, hash));
            }
            catch (ArgumentException)
            {
                return Task.FromResult(false);
            }
        }

        private byte[] CreateArgon2Salt()
        {
            return CreateArgon2Salt(_saltLength);
        }

        private byte[] CreateArgon2Salt(int saltLength)
        {
            var buffer = new byte[saltLength];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        private async Task<string> GetArgon2HashAsync(string input)
        {
            return await GetArgon2Hash(input, CreateArgon2Salt(), _degreesOfParallelism, _argon2Iterations, _memorySize, _hashLength);
        }

        private async Task<string> GetArgon2Hash(string input, byte[] salt, int degreesOfParallelism, int iterations, int memorySize, int hashLength)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(input))
            {
                Salt = salt,
                DegreeOfParallelism = degreesOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            string saltString = Convert.ToBase64String(argon2.Salt);
            byte[] hash = await argon2.GetBytesAsync(hashLength);
            string hashString = Convert.ToBase64String(hash);

            return $"$a2${degreesOfParallelism}${iterations}${memorySize}${salt.Length}${hashLength}${saltString}${hashString}";
        }

        private async Task<bool> CheckArgon2HashAsync(string clearText, string hash)
        {
            var split = hash.Split("$");

            // The a2 is simply what I am using to indicate argon2
            // if it is missing the encoding scheme is different.
            // Additionally there are 8 fields and the encoding
            // starts with the seperator so a length of 9 is needed.
            // We also verify that the fields are in specification.
            if (split.Length == 9 &&
                split[1] == "a2" &&
                int.TryParse(split[2], out int deg) && 
                int.TryParse(split[3], out int it) && 
                int.TryParse(split[4], out int mem) && 
                int.TryParse(split[5], out int saltL) && 
                int.TryParse(split[6], out int hashL) &&
                deg >= 1 &&
                it >= 1 &&
                mem >= 4 &&
                saltL >= 16 && saltL <= 1024 &&
                hashL >= 16 && saltL <= 1024)
            {
                var newHash = await GetArgon2Hash(clearText, Convert.FromBase64String(split[7]), deg, it, mem, hashL);
                return newHash == hash;
            }
            return false;
        }

        private Task<string> GetMD5HashAsync(string input)
        {
            return Task.Run(() =>
            {
                using MD5 md5 = MD5.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x}", b);
                }
                return sb.ToString();
            });
        }

        private async Task<bool> CheckMD5HashAsync(string clearText, string hash) => 
            await GetMD5HashAsync(clearText) == hash;

        private Task<string> GetSHA1HashAsync(string input)
        {
            return Task.Run(() =>
            {
                using var sha1 = SHA1.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha1.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x}", b);
                }
                return sb.ToString();
            });
        }

        private async Task<bool> CheckSHA1HashAsync(string clearText, string hash) =>
            await GetSHA1HashAsync(clearText) == hash;

        private Task<string> GetSHA256HashAsync(string input)
        {
            return Task.Run(() =>
            {
                using var sha256 = SHA256.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x}", b);
                }
                return sb.ToString();
            });
        }

        private async Task<bool> CheckSHA256HashAsync(string clearText, string hash) => 
            await GetSHA256HashAsync(clearText) == hash;

        private Task<string> GetSHA512HashAsync(string input)
        {
            return Task.Run(() =>
            {
                using var sha512 = SHA512.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha512.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x}", b);
                }
                return sb.ToString();
            });
        }

        private async Task<bool> CheckSHA512HashAsync(string clearText, string hash) => 
            await GetSHA512HashAsync(clearText) == hash;
    }
}
