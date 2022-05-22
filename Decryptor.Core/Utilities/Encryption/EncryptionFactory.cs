using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using System.Security;

namespace Decryptor.Core.Utilities.Encryption;

public class EncryptionFactory : IEncryptionFactory
{
    private readonly ISettings _settings;

    public EncryptionFactory(ISettings settings)
    {
        _settings = settings;
    }

    public ISimpleEncryption Create(EncryptionAlgorithm algorithm)
    {
        SecureString key = PasswordProtector.DecryptString(_settings.Key);
        var tripleDesKeySize = _settings.TripleDesKeySize;
        return algorithm switch
        {
            EncryptionAlgorithm.None => throw new NotImplementedException(),
            EncryptionAlgorithm.AES => new SimpleAes(key),
            EncryptionAlgorithm.DES => new SimpleDes(key),
            EncryptionAlgorithm.TripleDES => new SimpleTripleDes(key, tripleDesKeySize),
            _ => throw new InvalidOperationException($"{algorithm} is not a valid algorithm"),
        };
    }
}