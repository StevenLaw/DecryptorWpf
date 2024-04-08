using Decryptor.Enums;
using System;

namespace Decryptor.Utilities.Encryption;

public static class EncryptionUtil
{
    public static bool IsEncryptionExtension(this string value) =>
        value.Equals(".aes", StringComparison.InvariantCultureIgnoreCase) ||
        value.Equals(".des", StringComparison.InvariantCultureIgnoreCase) ||
        value.Equals(".3ds", StringComparison.InvariantCultureIgnoreCase);
    public static string GetDefaultExt(this EncryptionAlgorithm algorithm) =>
        algorithm switch
        {
            EncryptionAlgorithm.None => throw new NotImplementedException(),
            EncryptionAlgorithm.AES => "aes",
            EncryptionAlgorithm.DES => "des",
            EncryptionAlgorithm.TripleDES => "3ds",
            _ => throw new NotImplementedException(),
        };
    public static EncryptionAlgorithm ParseAlgorithm(string value) =>
        value switch
        {
            "Triple DES" => EncryptionAlgorithm.TripleDES,
            _ => Enum.TryParse(value, out EncryptionAlgorithm algorithm)
            ? algorithm
            : EncryptionAlgorithm.None
        };
}
