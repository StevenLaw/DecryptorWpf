using Decryptor.Interfaces;
using DevOne.Security.Cryptography.BCrypt;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class BCryptHash : IHash
    {
        private readonly int _workFactor;

        public BCryptHash(int workFactor)
        {
            _workFactor = workFactor;
        }

        public async Task<bool> CheckFileHashAsync(string filename, string hash)
        {
            var bytes = await File.ReadAllBytesAsync(filename);
            string clearText = Encoding.Default.GetString(bytes);
            try
            {
                return BCryptHelper.CheckPassword(clearText, hash);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public Task<bool> CheckHashAsync(string clearText, string hash)
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

        public async Task<bool> CheckHashAsync(Stream stream, string hash)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var clearText = Encoding.Default.GetString(ms.ToArray());
            try
            {
                return BCryptHelper.CheckPassword(clearText, hash);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public async Task<string> GetFileHashAsync(string filename)
        {
            var bytes = await File.ReadAllBytesAsync(filename);
            string salt = BCryptHelper.GenerateSalt(_workFactor);
            var clearText = Encoding.Default.GetString(bytes);
            return BCryptHelper.HashPassword(clearText, salt);
        }

        public Task<string> GetHashAsync(string clearText)
        {
            string salt = BCryptHelper.GenerateSalt(_workFactor);
            return Task.Run(() => BCryptHelper.HashPassword(clearText, salt));
        }

        public async Task<string> GetHashAsync(Stream stream)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            string salt = BCryptHelper.GenerateSalt(_workFactor);
            var clearText = Encoding.Default.GetString(ms.ToArray());
            return BCryptHelper.HashPassword(clearText, salt);
        }
    }
}