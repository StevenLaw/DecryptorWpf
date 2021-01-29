using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;
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

        public SimpleDesTests()
        {
            key = new SecureString();
            foreach (char c in insecureKey)
            {
                key.AppendChar(c);
            }
            key.MakeReadOnly();
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
    }
}