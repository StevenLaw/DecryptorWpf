using Decryptor.Core.Enums;

namespace Decryptor.Core.Interfaces;

public interface IEncryptionFactory
{
    ISimpleEncryption Create(EncryptionAlgorithm algorithm);
}