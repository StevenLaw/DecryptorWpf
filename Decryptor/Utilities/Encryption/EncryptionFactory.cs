using Decryptor.Enums;
using Decryptor.Interfaces;
using System;
using System.Security;

namespace Decryptor.Utilities.Encryption;

public static class EncryptionFactory
{
    public static ISimpleEncryption Create(EncryptionAlgorithm algorithm)
    {
        SecureString key = PasswordProtector.DecryptString(Properties.Settings.Default.Key);
        var tripleDesKeySize = (TripleDesKeySize)Properties.Settings.Default.TripleDesKeySize;
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