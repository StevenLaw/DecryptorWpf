namespace Decryptor.Utilities.Hashing
{
    public enum HashAlgorithm : byte
    {
        None = 0x0,
        BCrypt = 0x1,
        Scrypt = 0x2,
        Argon2 = 0x3,
        MD5 = 0x4,
        SHA1 = 0x5,
        SHA256 = 0x6,
        SHA512 = 0x7
    }
}