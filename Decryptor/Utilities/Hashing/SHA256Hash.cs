using Decryptor.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing;

public class SHA256Hash : IHash
{
    public async Task<bool> CheckFileHashAsync(string filename, string hash) =>
        (await GetFileHashAsync(filename)).Equals(hash, StringComparison.OrdinalIgnoreCase);

    public async Task<bool> CheckHashAsync(string clearText, string hash) =>
        (await GetHashAsync(clearText)).Equals(hash, StringComparison.OrdinalIgnoreCase);

    public async Task<bool> CheckHashAsync(Stream stream, string hash) =>
        (await GetHashAsync(stream)).Equals(hash, StringComparison.OrdinalIgnoreCase);

    public async Task<string> GetFileHashAsync(string filename)
    {
        using var fileStream = new FileStream(filename, FileMode.Open);
        return await GetHashAsync(fileStream);
    }

    public Task<string> GetHashAsync(string clearText)
    {
        return Task.Run(() =>
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearText);
            byte[] hash = SHA256.HashData(bytes);

            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        });
    }

    public async Task<string> GetHashAsync(Stream stream)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = await sha256.ComputeHashAsync(stream);

        var sb = new StringBuilder();
        foreach (byte b in hash)
        {
            sb.AppendFormat("{0:x2}", b);
        }
        return sb.ToString();
    }
}