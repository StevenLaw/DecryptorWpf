using Decryptor.Core.Interfaces;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Decryptor.Core.Utilities.Hashing;

public class ArgonHash : IHash
{
    private readonly int _argon2Iterations;
    private readonly int _degreesOfParallelism;
    private readonly int _hashLength;
    private readonly int _memorySize;
    private readonly int _saltLength;

    public ArgonHash(int degreesOfParallelism, int argon2Iterations, int memorySize, int saltLength, int hashLength)
    {
        _degreesOfParallelism = degreesOfParallelism;
        _argon2Iterations = argon2Iterations;
        _memorySize = memorySize;
        _saltLength = saltLength;
        _hashLength = hashLength;
    }

    public Task<bool> CheckFileHashAsync(string filename, string hash)
    {
        var bytes = File.ReadAllBytes(filename);
        string clearText = Encoding.Default.GetString(bytes);
        return CheckHashAsync(clearText, hash);
    }

    public async Task<bool> CheckHashAsync(string clearText, string hash)
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

    public async Task<bool> CheckHashAsync(Stream stream, string hash)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var clearText = Encoding.Default.GetString(ms.ToArray());
        return await CheckHashAsync(clearText, hash);
    }

    public async Task<string> GetFileHashAsync(string filename)
    {
        var bytes = await File.ReadAllBytesAsync(filename);
        var clearText = Encoding.Default.GetString(bytes);
        return await GetArgon2Hash(clearText, CreateArgon2Salt(), _degreesOfParallelism, _argon2Iterations, _memorySize, _hashLength);
    }

    public async Task<string> GetHashAsync(string clearText) =>
        await GetArgon2Hash(clearText, CreateArgon2Salt(), _degreesOfParallelism, _argon2Iterations, _memorySize, _hashLength);

    public async Task<string> GetHashAsync(Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        string clearText = Encoding.Default.GetString(ms.ToArray());
        return await GetArgon2Hash(clearText, CreateArgon2Salt(), _degreesOfParallelism, _argon2Iterations, _memorySize, _hashLength);
    }

    private static byte[] CreateArgon2Salt(int saltLength)
    {
        //var buffer = new byte[saltLength];
        //var rng = new RNGCryptoServiceProvider();
        //rng.GetBytes(buffer);
        var buffer = RandomNumberGenerator.GetBytes(saltLength);
        return buffer;
    }

    private static async Task<string> GetArgon2Hash(string clearText, byte[] salt, int degreesOfParallelism, int iterations, int memorySize, int hashLength)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(clearText))
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

    private byte[] CreateArgon2Salt()
    {
        return CreateArgon2Salt(_saltLength);
    }
}