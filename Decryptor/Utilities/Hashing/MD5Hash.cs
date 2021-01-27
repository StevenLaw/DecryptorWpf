using Decryptor.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class MD5Hash : IHash
    {
        public async Task<bool> CheckHashAsync(string clearText, string hash) =>
            await GetHashAsync(clearText) == hash;

        public Task<string> GetHashAsync(string clearText)
        {
            return Task.Run(() =>
            {
                using MD5 md5 = MD5.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(clearText);
                byte[] hash = md5.ComputeHash(bytes);

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