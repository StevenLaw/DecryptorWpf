using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests
{
    [TestClass()]
    public class SimpleDesTests
    {
        private readonly SecureString key;
        private const string insecureKey = "This is an insecure key for testing";
        private const string sample = "This is a sample";
        private const string result = "yMGP0assV+WeqXFbQxfXaEG+L3ASlsNv";
        private const string testFile = "Testing.txt";
        private const string cryptFile = "Testing.txt.des";
        private const string testCryptFile = "Crypt.txt.des";
        private const string outFile = "Out.txt";

        public SimpleDesTests()
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

        [TestMethod]
        public async Task EncryptTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            string encrypted = await des.EncryptAsync(sample);
            //Debug.WriteLine(encrypted);

            // Assert
            Assert.AreEqual(result, encrypted);
        }

        [TestMethod()]
        public async Task EncryptStreamTest()
        {
            // Arrange
            var des = new SimpleDes(key);
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            byte[] encrypted = await des.EncryptAsync(ms);
            string encStr = Convert.ToBase64String(encrypted);

            // Assert
            Assert.AreEqual(result, encStr);
        }

        [TestMethod()]
        public async Task EncryptFileTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            await des.EncryptAsync(testFile, cryptFile);

            // Assert
            Assert.IsTrue(File.Exists(cryptFile));
        }

        [TestMethod]
        public async Task DecryptTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            string decrypted = await des.DecryptAsync(result);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod()]
        public async Task DecryptStreamTest()
        {
            // Arrange
            var des = new SimpleDes(key);
            var bytes = Convert.FromBase64String(result);
            using var ms = new MemoryStream(bytes);

            // Act
            byte[] decrypted = await des.DecryptAsync(ms);
            var decStr = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.AreEqual(sample, decStr);
        }

        [TestMethod()]
        public async Task DecryptFileTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            await des.DecryptAsync(testCryptFile, outFile);

            // Assert
            Assert.IsTrue(File.Exists(outFile));
        }

        [TestMethod]
        public async Task EncryptDecryptTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            string encrypted = await des.EncryptAsync(sample);
            string decrypted = await des.DecryptAsync(encrypted);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptStreamTest()
        {
            // Arrange
            var des = new SimpleDes(key);
            using var inStream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            byte[] encrypted = await des.EncryptAsync(inStream);
            using var outStream = new MemoryStream(encrypted);
            byte[] decrypted = await des.DecryptAsync(outStream);
            string decStr = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.AreEqual(sample, decStr);
        }

        [TestMethod]
        public async Task EncryptDecryptFileTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            await des.EncryptAsync(testFile, cryptFile);
            await des.DecryptAsync(cryptFile, outFile);

            // Assert
            Assert.AreEqual(File.ReadAllText(testFile), File.ReadAllText(outFile));
        }

        [TestMethod]
        public async Task DecryptEncryptTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            string decrypted = await des.DecryptAsync(result);
            string encrypted = await des.EncryptAsync(decrypted);

            // Assert
            Assert.AreEqual(result, encrypted);
        }

        [TestMethod]
        public async Task DecryptEncryptStreamTest()
        {
            // Arrange
            var des = new SimpleDes(key);
            using var inStream = new MemoryStream(Convert.FromBase64String(result));

            // Act
            byte[] decrypted = await des.DecryptAsync(inStream);
            using var outStream = new MemoryStream(decrypted);
            byte[] encrypted = await des.EncryptAsync(outStream);
            string encStr = Convert.ToBase64String(encrypted);

            // Assert
            Assert.AreEqual(result, encStr);
        }

        [TestMethod]
        public async Task DecryptEncryptFileTest()
        {
            // Arrange
            var des = new SimpleDes(key);

            // Act
            await des.DecryptAsync(testCryptFile, outFile);
            await des.EncryptAsync(outFile, cryptFile);

            // Assert
            Assert.AreEqual(File.ReadAllText(testCryptFile), File.ReadAllText(cryptFile));
        }
    }
}