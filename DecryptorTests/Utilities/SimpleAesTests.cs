using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Security;

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
        public void EncryptTest()
        {
            // Arange
            var aes = new SimpleAes(key);

            // Act
            string encrypted = aes.Encrypt(sample);
            Debug.WriteLine(encrypted);

            // Assert
            Assert.AreEqual(result, encrypted);
        }

        [TestMethod()]
        public void DecryptTest()
        {
            // Arange
            var aes = new SimpleAes(key);

            // Act
            string decrypted = aes.Decrypt(result);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod]
        public void EncryptDecryptTest()
        {
            // Arange
            var aes = new SimpleAes(key);

            // Act
            string encrypted = aes.Encrypt(sample);
            string decrypted = aes.Decrypt(encrypted);

            // Assert
            Assert.AreEqual(sample, decrypted);
        }

        [TestMethod]
        public void DecryptEncryptTest()
        {
            // Arrange
            var aes = new SimpleAes(key);

            // Act
            string decrypted = aes.Decrypt(result);
            string encrypted = aes.Encrypt(decrypted);

            // Assert
            Assert.AreEqual(result, encrypted);
        }
    }
}