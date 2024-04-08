using Decryptor.Interfaces;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption;

public class SimpleDes(SecureString key) : ISimpleEncryption
{
    private const int KEY_IV_SIZE = 8;
    private readonly SecureString _encryptionKey = key;
    private readonly int _iterations = 1000;

    private readonly byte[] _salt =
    [
        0xc2, 0xdd, 0x5c, 0xe8, 0x4a,
        0x8d, 0x08, 0x52, 0xbf, 0x8e,
        0x82, 0x2f, 0xeb
    ];

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
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                         _salt, _iterations, HashAlgorithmName.SHA1);
        des.Key = pdb.GetBytes(KEY_IV_SIZE);
        des.IV = pdb.GetBytes(KEY_IV_SIZE);
        using var cs = new CryptoStream(cypherStream,
                                        des.CreateDecryptor(),
                                        CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[cypherStream.Length];
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

    public async Task<string> EncryptAsync(string clearText)
    {
        byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
        using DES des = DES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                         _salt, _iterations, HashAlgorithmName.SHA1);
        des.Key = pdb.GetBytes(KEY_IV_SIZE);
        des.IV = pdb.GetBytes(KEY_IV_SIZE);
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
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                         _salt, _iterations, HashAlgorithmName.SHA1);
        des.Key = pdb.GetBytes(KEY_IV_SIZE);
        des.IV = pdb.GetBytes(KEY_IV_SIZE);
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