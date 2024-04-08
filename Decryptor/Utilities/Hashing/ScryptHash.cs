using Decryptor.Interfaces;
using Scrypt;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing;

public class ScryptHash(int scryptIterations, int blockCount, int threadCount) : IHash
{
    private readonly int _scryptIterations = scryptIterations;
    private readonly int _blockCount = blockCount;
    private readonly int _threadCount = threadCount;

    public Task<bool> CheckFileHashAsync(string filename, string hash)
    {
        var bytes = File.ReadAllBytes(filename);
        string clearText = Encoding.Default.GetString(bytes);
        ScryptEncoder encoder = new(_scryptIterations, _blockCount, _threadCount);
        try
        {
            return Task.Run(() => encoder.Compare(clearText, hash));
        }
        catch (ArgumentException)
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> CheckHashAsync(string clearText, string hash)
    {
        ScryptEncoder encoder = new(_scryptIterations, _blockCount, _threadCount);
        try
        {
            return Task.Run(() => encoder.Compare(clearText, hash));
        }
        catch (ArgumentException)
        {
            return Task.FromResult(false);
        }
    }

    public async Task<bool> CheckHashAsync(Stream stream, string hash)
    {
        ScryptEncoder encoder = new(_scryptIterations, _blockCount, _threadCount);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var clearText = Encoding.Default.GetString(ms.ToArray());
        try
        {
            return encoder.Compare(clearText, hash);
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public async Task<string> GetFileHashAsync(string filename)
    {
        var bytes = await File.ReadAllBytesAsync(filename);
        ScryptEncoder encoder = new(_scryptIterations, _blockCount, _threadCount);
        var clearText = Encoding.Default.GetString(bytes);
        return encoder.Encode(clearText);
    }

    public Task<string> GetHashAsync(string clearText)
    {
        ScryptEncoder encoder = new(_scryptIterations, _blockCount, _threadCount);
        return Task.Run(() => encoder.Encode(clearText));
    }

    public async Task<string> GetHashAsync(Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        ScryptEncoder encoder = new(_scryptIterations, _blockCount, _threadCount);
        var clearText = Encoding.Default.GetString(ms.ToArray());
        return encoder.Encode(clearText);
    }
}