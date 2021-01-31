using Decryptor.Interfaces;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class SHA1Hash : IHash
    {
        public async Task<bool> CheckFileHashAsync(string filename, string hash) =>
            await GetFileHashAsync(filename) == hash;

        public async Task<bool> CheckHashAsync(string clearText, string hash) =>
            await GetHashAsync(clearText) == hash;

        public async Task<bool> CheckHashAsync(Stream stream, string hash) =>
            await GetHashAsync(stream) == hash;

        public async Task<string> GetFileHashAsync(string Filename)
        {
            using var fileStream = new FileStream(Filename, FileMode.Open);
            return await GetHashAsync(fileStream);
        }

        public Task<string> GetHashAsync(string clearText)
        {
            return Task.Run(() =>
            {
                using var sha1 = SHA1.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(clearText);
                byte[] hash = sha1.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x}", b);
                }
                return sb.ToString();
            });
        }

        public async Task<string> GetHashAsync(Stream stream)
        {
            using var sha1 = SHA1.Create();
            byte[] hash = await sha1.ComputeHashAsync(stream);

            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.AppendFormat("{0:x}", b);
            }
            return sb.ToString();
        }
    }
}