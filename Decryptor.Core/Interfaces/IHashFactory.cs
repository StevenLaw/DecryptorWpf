using Decryptor.Core.Enums;

namespace Decryptor.Core.Interfaces;

public interface IHashFactory
{
    IHash Create(HashAlgorithm algorithm);
}