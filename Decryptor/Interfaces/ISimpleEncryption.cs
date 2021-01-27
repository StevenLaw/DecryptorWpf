using System.Threading.Tasks;

namespace Decryptor.Interfaces
{
    public interface ISimpleEncryption
    {
        Task<string> DecryptAsync(string cypherText);
        Task<string> EncryptAsync(string clearText);
    }
}