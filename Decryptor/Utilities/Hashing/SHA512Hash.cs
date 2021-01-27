using Decryptor.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class SHA512Hash : IHash
    {
        public async Task<bool> CheckHashAsync(string clearText, string hash) =>
            await GetHashAsync(clearText) == hash;

        public Task<string> GetHashAsync(string clearText)
        {
            return Task.Run(() =>
            {
                using var sha512 = SHA512.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(clearText);
                byte[] hash = sha512.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x}", b);
                }
                return sb.ToString();
            });
        }
    }
}