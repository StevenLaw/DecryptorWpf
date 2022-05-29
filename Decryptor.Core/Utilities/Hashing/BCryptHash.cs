using Decryptor.Core.Interfaces;
using Bc = BCrypt.Net;
using System.Text;
using BCrypt.Net;

namespace Decryptor.Core.Utilities.Hashing;

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
            return Bc.BCrypt.Verify(clearText, hash);
        }
        catch (SaltParseException)
        {
            return false;
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
            return Task.Run(() => Bc.BCrypt.Verify(clearText, hash));
        }
        catch (SaltParseException)
        {
            return Task.FromResult(false);
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
            return Bc.BCrypt.Verify(clearText, hash);
        }
        catch (SaltParseException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public async Task<string> GetFileHashAsync(string filename)
    {
        var bytes = await File.ReadAllBytesAsync(filename);
        string salt = Bc.BCrypt.GenerateSalt(_workFactor);
        var clearText = Encoding.Default.GetString(bytes);
        return Bc.BCrypt.HashPassword(clearText, salt);
    }

    public Task<string> GetHashAsync(string clearText)
    {
        string salt = Bc.BCrypt.GenerateSalt(_workFactor);
        return Task.Run(() => Bc.BCrypt.HashPassword(clearText, salt));
    }

    public async Task<string> GetHashAsync(Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        string salt = Bc.BCrypt.GenerateSalt(_workFactor);
        var clearText = Encoding.Default.GetString(ms.ToArray());
        return Bc.BCrypt.HashPassword(clearText, salt);
    }
}