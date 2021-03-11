using Decryptor.Interfaces;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption
{
    public class SimpleTripleDes : ISimpleEncryption
    {
        private readonly SecureString encryptionKey;
        private readonly int keySize;
        private readonly int ivSize = 8;

        private readonly byte[] salt = new byte[]
        {
            0xf4, 0x9a, 0x63, 0x44, 0x9f, 
            0xfe, 0xac, 0xd4, 0x4d, 0xca, 
            0xee, 0x43, 0x3f
        };

        public TripleDesKeySize KeySizeMode { get; set; }

        public SimpleTripleDes(SecureString key, TripleDesKeySize keySizeMode)
        {
            encryptionKey = key;
            KeySizeMode = keySizeMode;
            keySize = keySizeMode switch
            {
                TripleDesKeySize.b128 => 128 / 8,
                TripleDesKeySize.b192 => 192 / 8,
                _ => throw new NotImplementedException(),
            };
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
            using TripleDES des = TripleDES.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                                salt);
            des.Key = pdb.GetBytes(keySize);
            des.IV = pdb.GetBytes(ivSize);
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
            using TripleDES des = TripleDES.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                                salt);
            des.Key = pdb.GetBytes(keySize);
            des.IV = pdb.GetBytes(ivSize);
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
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                                salt);
            des.Key = pdb.GetBytes(keySize);
            des.IV = pdb.GetBytes(ivSize);
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
    public enum TripleDesKeySize
    {
        b128,
        b192
    }
}
