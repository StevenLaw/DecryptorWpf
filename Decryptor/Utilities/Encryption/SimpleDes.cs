using Decryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption
{
    public class SimpleDes : ISimpleEncryption
    {
        private readonly byte[] key;
        private readonly byte[] iv;

        public SimpleDes()
        {
            using DES des = DES.Create();
            key = des.Key;
            iv = des.IV;
        }

        public SimpleDes(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
        }

        public async Task<string> DecryptAsync(string cypherText)
        {
            byte[] cypherBytes = Convert.FromBase64String(cypherText);
            using DES des = DES.Create();
            des.Key = key;
            des.IV = iv;
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
            des.Key = key;
            des.IV = iv;
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
