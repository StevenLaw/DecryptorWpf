using Decryptor.Core.Interfaces;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Decryptor.Core.Utilities.Encryption;

public class SimpleAes : ISimpleEncryption
{
    private const int _iVSize = 16;
    private const int _keySize = 32;
    private readonly SecureString _encryptionKey;

    private readonly byte[] _salt = new byte[]
    {
        0x49, 0x76, 0x61, 0x6E, 0x20,
        0x4D, 0x65, 0x64, 0x76, 0x65,
        0x64, 0x65, 0x76
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleAes"/> class.
    /// </summary>
    /// <param name="key">The key.</param>
    public SimpleAes(SecureString key)
    {
        _encryptionKey = key;
    }

    /// <summary>
    /// Decrypts the specified cypher text.
    /// </summary>
    /// <param name="cypherText">The cypher text.</param>
    /// <returns>
    /// the decrypted text.
    /// </returns>
    public async Task<string> DecryptAsync(string cypherText)
    {
        byte[] cypherBytes = Convert.FromBase64String(cypherText);
        using var ms = new MemoryStream(cypherBytes);
        var fromEncrypt = await DecryptAsync(ms);
        return Encoding.UTF8.GetString(fromEncrypt);
    }

    public async Task<byte[]> DecryptAsync(Stream cypherStream)
    {
        using Aes aes = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                         _salt);
        aes.Key = pdb.GetBytes(_keySize);
        aes.IV = pdb.GetBytes(_iVSize);
        using var cs = new CryptoStream(cypherStream,
                                        aes.CreateDecryptor(),
                                        CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[cypherStream.Length];
        int num = 0;
        while (num < fromEncrypt.Length)
        {
            int bytesRead = await cs.ReadAsync(fromEncrypt.AsMemory(num, fromEncrypt.Length - num));
            if (bytesRead == 0) break;
            num += bytesRead;
        }
        await cs.ReadAsync(fromEncrypt.AsMemory(0, fromEncrypt.Length));
        cs.Close();
        return TrimNullByte(fromEncrypt);
    }

    public async Task DecryptAsync(string cypherFile, string clearFile)
    {
        using var inputStream = new FileStream(cypherFile, FileMode.Open);
        var fromEncrypt = await DecryptAsync(inputStream);
        await File.WriteAllBytesAsync(clearFile, fromEncrypt);
    }

    /// <summary>
    /// Encrypts the specified clear text.
    /// </summary>
    /// <param name="clearText">The clear text.</param>
    /// <returns>
    /// the encrypted text.
    /// </returns>
    public async Task<string> EncryptAsync(string clearText)
    {
        byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
        using Aes aes = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                         _salt);
        aes.Key = pdb.GetBytes(_keySize);
        aes.IV = pdb.GetBytes(_iVSize);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms,
                                        aes.CreateEncryptor(),
                                        CryptoStreamMode.Write);
        await cs.WriteAsync(clearBytes.AsMemory(0, clearBytes.Length));
        cs.Close();
        return Convert.ToBase64String(ms.ToArray());
    }

    public async Task<byte[]> EncryptAsync(Stream clearStream)
    {
        using Aes aes = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                         _salt);
        aes.Key = pdb.GetBytes(_keySize);
        aes.IV = pdb.GetBytes(_iVSize);
        using var cs = new CryptoStream(clearStream,
                                        aes.CreateEncryptor(),
                                        CryptoStreamMode.Read);
        using var br = new BinaryReader(cs);
        using var ms = new MemoryStream();
        await cs.CopyToAsync(ms);
        cs.Close();
        return ms.ToArray();
    }

    public async Task EncryptAsync(string clearFile, string cypherFile)
    {
        using var fs = new FileStream(clearFile, FileMode.Open);
        var bytes = await EncryptAsync(fs);
        await File.WriteAllBytesAsync(cypherFile, bytes);
    }

    private static byte[] TrimNullByte(byte[] bytes)
    {
        int count = 0;
        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            if (bytes[i] == 0)
                count++;
            else break;
        }
        return bytes[0..(bytes.Length - count)];
    }
}