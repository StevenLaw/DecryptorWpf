using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests
{
    [TestClass()]
    public class SimpleAesTests
    {
        private readonly SecureString key;
        private const string insecureKey = "This is an insecure key for testing";
        private const string sample = "This is a sample for encrypting";
        private const string result = "/u7rOUO9GCXJGN269dag0QU26yR4WJkLPvcLjcWoxYc=";
        private const string testFile = "Testing.txt";
        private const string cryptFile = "Testing.txt.aes";
        private const string testCryptFile = "Crypt.txt.aes";
        private const string outFile = "Out.txt";

        public SimpleAesTests()
        {
            key = new SecureString();
            foreach (char c in insecureKey)
            {
                key.AppendChar(c);
            }
            key.MakeReadOnly();
        }

        [TestInitialize]
        public void InitTests()
        {
            if (File.Exists(cryptFile))
                File.Delete(cryptFile);
            if (File.Exists(outFile))
                File.Delete(outFile);
        }

        [TestMethod()]
        public async Task EncryptTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            string encrypted = await aes.EncryptAsync(sample);

            // Assert
            Assert.AreEqual(result, encrypted);
        }

        [TestMethod()]
        public async Task EncryptStreamTest()
        {
            // Arrange
            var aes = new SimpleAes(key);
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            byte[] encrypted = await aes.EncryptAsync(ms);
            string encStr = Convert.ToBase64String(encrypted);

            // Assert
            Assert.AreEqual(result, encStr);
        }

        [TestMethod()]
        public async Task EncryptFileTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            await aes.EncryptAsync(testFile, cryptFile);

            // Assert
            Assert.IsTrue(File.Exists(cryptFile));
        }

        [TestMethod()]
        public async Task DecryptTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            string decrypted = await aes.DecryptAsync(result);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod()]
        public async Task DecryptStreamTest()
        {
            // Arrange
            var aes = new SimpleAes(key);
            var bytes = Convert.FromBase64String(result);
            using var ms = new MemoryStream(bytes);

            // Act
            byte[] decrypted = await aes.DecryptAsync(ms);
            var decStr = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.AreEqual(sample, decStr);
        }

        [TestMethod()]
        public async Task DecryptFileTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            await aes.DecryptAsync(testCryptFile, outFile);

            // Assert
            Assert.IsTrue(File.Exists(outFile));
        }

        [TestMethod]
        public async Task EncryptDecryptTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            string encrypted = await aes.EncryptAsync(sample);
            string decrypted = await aes.DecryptAsync(encrypted);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptStreamTest()
        {
            // Arrange
            var aes = new SimpleAes(key);
            using var inStream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            byte[] encrypted = await aes.EncryptAsync(inStream);
            using var outStream = new MemoryStream(encrypted);
            byte[] decrypted = await aes.DecryptAsync(outStream);
            string decStr = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.AreEqual(sample, decStr);
        }

        [TestMethod]
        public async Task EncryptDecryptFileTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            await aes.EncryptAsync(testFile, cryptFile);
            await aes.DecryptAsync(cryptFile, outFile);

            // Assert
            Assert.AreEqual(File.ReadAllText(testFile), File.ReadAllText(outFile));
        }

        [TestMethod]
        public async Task DecryptEncryptTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            string decrypted = await aes.DecryptAsync(result);
            string encrypted = await aes.EncryptAsync(decrypted);

            // Assert
            Assert.AreEqual(result, encrypted);
        }

        [TestMethod]
        public async Task DecryptEncryptStreamTest()
        {
            // Arrange
            var aes = new SimpleAes(key);
            using var inStream = new MemoryStream(Convert.FromBase64String(result));

            // Act
            byte[] decrypted = await aes.DecryptAsync(inStream);
            using var outStream = new MemoryStream(decrypted);
            byte[] encrypted = await aes.EncryptAsync(outStream);
            string encStr = Convert.ToBase64String(encrypted);

            // Assert
            Assert.AreEqual(result, encStr);
        }

        [TestMethod]
        public async Task DecryptEncryptFileTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            await aes.DecryptAsync(testCryptFile, outFile);
            await aes.EncryptAsync(outFile, cryptFile);

            // Assert
            Assert.AreEqual(File.ReadAllText(testCryptFile), File.ReadAllText(cryptFile));
        }
    }
}