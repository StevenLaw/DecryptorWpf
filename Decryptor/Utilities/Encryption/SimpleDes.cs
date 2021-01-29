using Decryptor.Interfaces;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption
{
    public class SimpleDes : ISimpleEncryption
    {
        private const int keyIVSize = 8;
        private readonly SecureString encryptionKey = null;

        private readonly byte[] salt = new byte[]
        {
            0xc2, 0xdd, 0x5c, 0xe8, 0x4a,
            0x8d, 0x08, 0x52, 0xbf, 0x8e,
            0x82, 0x2f, 0xeb
        };

        public SimpleDes(SecureString key)
        {
            encryptionKey = key;
        }

        public async Task<string> DecryptAsync(string cypherText)
        {
            byte[] cypherBytes = Convert.FromBase64String(cypherText);
            using DES des = DES.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                                salt);
            des.Key = pdb.GetBytes(keyIVSize);
            des.IV = pdb.GetBytes(keyIVSize);
            using var ms = new MemoryStream(cypherBytes);
            using var cs = new CryptoStream(ms,
                                            des.CreateDecryptor(),
                                            CryptoStreamMode.Read);
            byte[] fromEncrypt = new byte[cypherBytes.Length];
            await cs.ReadAsync(fromEncrypt.AsMemory(0, fromEncrypt.Length));
            cs.Close();
            return Encoding.UTF8.GetString(fromEncrypt).Trim('\0');
        }

        public async Task<string> EncryptAsync(string clearText)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            using DES des = DES.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                                salt);
            des.Key = pdb.GetBytes(keyIVSize);
            des.IV = pdb.GetBytes(keyIVSize);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms,
                                            des.CreateEncryptor(),
                                            CryptoStreamMode.Write);
            await cs.WriteAsync(clearBytes.AsMemory(0, clearBytes.Length));
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}