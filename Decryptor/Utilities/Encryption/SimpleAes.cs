﻿using Decryptor.Interfaces;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption
{
    public class SimpleAes : ISimpleEncryption
    {
        private const int IVSize = 16;
        private const int keySize = 32;
        private readonly SecureString encryptionKey;

        private readonly byte[] salt = new byte[]
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
            encryptionKey = key;
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
            using Aes aes = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                             salt);
            aes.Key = pdb.GetBytes(keySize);
            aes.IV = pdb.GetBytes(IVSize);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            await cs.WriteAsync(cypherBytes.AsMemory(0, cypherBytes.Length));
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
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
            var pdb = new Rfc2898DeriveBytes(encryptionKey.ToInsecureString(),
                                             salt);
            aes.Key = pdb.GetBytes(keySize);
            aes.IV = pdb.GetBytes(IVSize);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms,
                                            aes.CreateEncryptor(),
                                            CryptoStreamMode.Write);
            await cs.WriteAsync(clearBytes.AsMemory(0, clearBytes.Length));
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}