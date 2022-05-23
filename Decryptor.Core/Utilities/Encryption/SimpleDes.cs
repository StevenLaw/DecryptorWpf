using Decryptor.Core.Interfaces;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Decryptor.Core.Utilities.Encryption;

public class SimpleDes : ISimpleEncryption
{
    private const int _keyIVSize = 8;
    private readonly SecureString _encryptionKey = PasswordUtilities.ToSecureString(string.Empty);

    private readonly byte[] _salt = new byte[]
    {
        0xc2, 0xdd, 0x5c, 0xe8, 0x4a,
        0x8d, 0x08, 0x52, 0xbf, 0x8e,
        0x82, 0x2f, 0xeb
    };

    public SimpleDes(SecureString key)
    {
        _encryptionKey = key;
    }

    public async Task<string> DecryptAsync(string cypherText)
    {
        byte[] cypherBytes = Convert.FromBase64String(cypherText);
        using var ms = new MemoryStream(cypherBytes);
        var fromEncrypt = await DecryptAsync(ms);
        return Encoding.UTF8.GetString(fromEncrypt);
    }

    public async Task<byte[]> DecryptAsync(Stream cypherStream)
    {
        using DES des = DES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(), _salt);
        des.Key = pdb.GetBytes(_keyIVSize);
        des.IV = pdb.GetBytes(_keyIVSize);
        using var cs = new CryptoStream(cypherStream,
                                        des.CreateDecryptor(),
                                        CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[cypherStream.Length];
        int num = 0;
        while (num < fromEncrypt.Length)
        {
            int bytesRead = await cs.ReadAsync(fromEncrypt.AsMemory(num, fromEncrypt.Length - num));
            if (bytesRead == 0) break;
            num += bytesRead;
        }
        cs.Close();
        return TrimNullByte(fromEncrypt);
    }

    public async Task DecryptAsync(string cypherFile, string clearFile)
    {
        using var inputStream = new FileStream(cypherFile, FileMode.Open);
        var fromEncrypt = await DecryptAsync(inputStream);
        await File.WriteAllBytesAsync(clearFile, fromEncrypt);
    }

    public async Task<string> EncryptAsync(string clearText)
    {
        byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
        using DES des = DES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                            _salt);
        des.Key = pdb.GetBytes(_keyIVSize);
        des.IV = pdb.GetBytes(_keyIVSize);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms,
                                        des.CreateEncryptor(),
                                        CryptoStreamMode.Write);
        await cs.WriteAsync(clearBytes.AsMemory(0, clearBytes.Length));
        cs.Close();
        return Convert.ToBase64String(ms.ToArray());
    }

    public async Task<byte[]> EncryptAsync(Stream clearStream)
    {
        using DES des = DES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(), _salt);
        des.Key = pdb.GetBytes(_keyIVSize);
        des.IV = pdb.GetBytes(_keyIVSize);
        using var cs = new CryptoStream(clearStream,
                                        des.CreateEncryptor(),
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