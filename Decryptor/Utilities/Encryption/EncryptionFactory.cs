using Decryptor.Interfaces;
using System;

namespace Decryptor.Utilities.Encryption
{
    public static class EncryptionFactory
    {
        public static ISimpleEncryption Create(EncryptionAlgorithm algorithm) =>
            algorithm switch
            {
                EncryptionAlgorithm.None => throw new NotImplementedException(),
                EncryptionAlgorithm.AES => new SimpleAes(
                    PasswordProtector.DecryptString(Properties.Settings.Default.Key)),
                EncryptionAlgorithm.DES => new SimpleDes(),
                _ => throw new InvalidOperationException($"{algorithm} is not a valid algorithm"),
            };
    }
}