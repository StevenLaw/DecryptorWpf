using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Tests
{
    [TestClass()]
    public class SimpleAesTests
    {
        private SecureString key;
        private const string insecureKey = "This is an insecure key for testing";
        private const string sample = "This is a sample for encrypting";
        private const string result = "/u7rOUO9GCXJGN269dag0QU26yR4WJkLPvcLjcWoxYc=";

        public SimpleAesTests()
        {
            key = new SecureString();
            foreach (char c in insecureKey)
            {
                key.AppendChar(c);
            }
            key.MakeReadOnly();
        }

        [TestMethod()]
        public async Task EncryptTest()
        {
            // Arange
            var aes = new SimpleAes(key);

            // Act
            string encrypted = await aes.EncryptAsync(sample);
            Debug.WriteLine(encrypted);

            // Assert
            Assert.AreEqual(result, encrypted);
        }

        [TestMethod()]
        public async Task DecryptTest()
        {
            // Arange
            var aes = new SimpleAes(key);

            // Act
            string decrypted = await aes.DecryptAsync(result);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptTest()
        {
            // Arange
            var aes = new SimpleAes(key);

            // Act
            string encrypted = await aes.EncryptAsync(sample);
            string decrypted = await aes.DecryptAsync(encrypted);

            // Assert
            Assert.AreEqual(sample, decrypted);
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
    }
}