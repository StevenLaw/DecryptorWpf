namespace Decryptor.Core.Interfaces;

public interface IHash
{
    Task<bool> CheckFileHashAsync(string filename, string hash);

    Task<bool> CheckHashAsync(string clearText, string hash);

    Task<bool> CheckHashAsync(Stream stream, string hash);

    Task<string> GetFileHashAsync(string filename);

    Task<string> GetHashAsync(string clearText);

    Task<string> GetHashAsync(Stream stream);
}