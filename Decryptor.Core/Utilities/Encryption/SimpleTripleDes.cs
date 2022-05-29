using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Decryptor.Core.Utilities.Encryption;

public class SimpleTripleDes : ISimpleEncryption
{
    private readonly SecureString _encryptionKey;
    private readonly int _ivSize = 8;
    private readonly int _keySize;

    private readonly byte[] _salt = new byte[]
    {
        0xf4, 0x9a, 0x63, 0x44, 0x9f,
        0xfe, 0xac, 0xd4, 0x4d, 0xca,
        0xee, 0x43, 0x3f
    };

    public SimpleTripleDes(SecureString key, TripleDesKeySize keySizeMode)
    {
        _encryptionKey = key;
        KeySizeMode = keySizeMode;
        _keySize = keySizeMode switch
        {
            TripleDesKeySize.b128 => 128 / 8,
            TripleDesKeySize.b192 => 192 / 8,
            _ => throw new NotImplementedException(),
        };
    }

    public TripleDesKeySize KeySizeMode { get; set; }

    public async Task<string> DecryptAsync(string cypherText)
    {
        byte[] cypherBytes = Convert.FromBase64String(cypherText);
        using var ms = new MemoryStream(cypherBytes);
        var fromEncrypt = await DecryptAsync(ms);
        return Encoding.UTF8.GetString(fromEncrypt);
    }

    public async Task<byte[]> DecryptAsync(Stream cypherStream)
    {
        using TripleDES des = TripleDES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                            _salt);
        des.Key = pdb.GetBytes(_keySize);
        des.IV = pdb.GetBytes(_ivSize);
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
        using TripleDES des = TripleDES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                            _salt);
        des.Key = pdb.GetBytes(_keySize);
        des.IV = pdb.GetBytes(_ivSize);
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
        using TripleDES des = TripleDES.Create();
        var pdb = new Rfc2898DeriveBytes(_encryptionKey.ToInsecureString(),
                                            _salt);
        des.Key = pdb.GetBytes(_keySize);
        des.IV = pdb.GetBytes(_ivSize);
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

public static class TripleDesUtil
{
    private const string _disp128 = "128 byte key";
    private const string _disp192 = "192 byte key";
    private const string _invalidKeyError = "Invalid key size";

    public static string[] KeySizes =>
        Enum.GetValues<TripleDesKeySize>()
        .Select(x => x.GetDisplayString())
        .ToArray();

    public static string GetDisplayString(this TripleDesKeySize value) =>
        value switch
        {
            TripleDesKeySize.b128 => _disp128,
            TripleDesKeySize.b192 => _disp192,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), _invalidKeyError),
        };

    public static TripleDesKeySize Parse(string value) =>
        value switch
        {
            _disp128 => TripleDesKeySize.b128,
            _disp192 => TripleDesKeySize.b192,
            _ => Enum.TryParse(value, out TripleDesKeySize result)
            ? result
            : throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), _invalidKeyError),
        };
}