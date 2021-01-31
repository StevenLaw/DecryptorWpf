using Decryptor.Enums;
using System;

namespace Decryptor.Utilities.Encryption
{
    public static class EncryptionUtil
    {
        public static string GetDefaultExt(this EncryptionAlgorithm algorithm) =>
            algorithm switch
            {
                EncryptionAlgorithm.None => throw new NotImplementedException(),
                EncryptionAlgorithm.AES => "aes",
                EncryptionAlgorithm.DES => "des",
                _ => throw new NotImplementedException(),
            };
    }
}
