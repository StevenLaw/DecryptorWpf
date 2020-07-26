using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities
{
    public class SimpleAes
    {
        private const int keySize = 32;
        private const int IVSize = 16;

        private readonly SecureString encryptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleAes"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public SimpleAes(SecureString key)
        {
            encryptionKey = key;
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
                                             new byte[]
                                             {
                                                     0x49, 0x76, 0x61, 0x6E, 0x20,
                                                     0x4D, 0x65, 0x64, 0x76, 0x65,
                                                     0x64, 0x65, 0x76
                                             });
            aes.Key = pdb.GetBytes(keySize);
            aes.IV = pdb.GetBytes(IVSize);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms,
                                             aes.CreateEncryptor(),
                                             CryptoStreamMode.Write);
            await cs.WriteAsync(clearBytes, 0, clearBytes.Length);
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
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
                                             new byte[]
                                             {
                                                     0x49, 0x76, 0x61, 0x6E, 0x20,
                                                     0x4D, 0x65, 0x64, 0x76, 0x65,
                                                     0x64, 0x65, 0x76
                                             });
            aes.Key = pdb.GetBytes(keySize);
            aes.IV = pdb.GetBytes(IVSize);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            await cs.WriteAsync(cypherBytes, 0, cypherBytes.Length);
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
