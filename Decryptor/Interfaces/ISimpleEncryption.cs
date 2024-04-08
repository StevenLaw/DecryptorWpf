using System.IO;
using System.Threading.Tasks;

namespace Decryptor.Interfaces;

public interface ISimpleEncryption
{
    Task<string> DecryptAsync(string cypherText);

    Task<byte[]> DecryptAsync(Stream cypherStream);

    Task DecryptAsync(string cypherFile, string clearFile);

    Task<string> EncryptAsync(string clearText);

    Task<byte[]> EncryptAsync(Stream clearStream);

    Task EncryptAsync(string clearFile, string cypherFile);
}