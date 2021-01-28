using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests
{
    [TestClass()]
    public class SimpleDesTests
    {
        private const string sample = "This is a sample";

        [TestMethod()]
        public async Task EncryptDecryptExternalKeyTest()
        {
            byte[] key = new byte[] { 0x0b, 0x67, 0x66, 0x1e, 0xb3, 0x5e, 0x96, 0x97 };
            byte[] iv = new byte[] { 0xee, 0x95, 0xe6, 0xf7, 0x57, 0xf6, 0x1e, 0xbb };

            // Arrange
            SimpleDes sDes1 = new(key, iv);
            SimpleDes sDes2 = new(key, iv);

            // Act
            var crypt1 = await sDes1.EncryptAsync(sample);
            var crypt2 = await sDes2.EncryptAsync(sample);

            var decrypt1 = await sDes1.DecryptAsync(crypt1);
            var decrypt2 = await sDes2.DecryptAsync(crypt2);

            // Assert
            Assert.AreEqual(crypt1, crypt2);
            Assert.AreEqual(decrypt1, decrypt2);
            Assert.AreEqual(decrypt1, sample);
            Assert.AreEqual(decrypt2, sample);
        }

        [TestMethod()]
        public async Task EncryptDecryptTest()
        {
            // Arrange
            var sDes = new SimpleDes();

            // Act
            var crypt = await sDes.EncryptAsync(sample);
            var decrypt = await sDes.DecryptAsync(crypt);

            // Assert
            Assert.AreEqual(decrypt, sample);
        }
    }
}