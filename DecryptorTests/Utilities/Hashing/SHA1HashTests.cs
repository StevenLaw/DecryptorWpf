using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class SHA1HashTests
    {
        private const string sample = "This is sample text";

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new SHA1Hash();
            string hash = "451d99c8281f579bdf1e2bfa2a63fd23707037";

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task GetHashAsyncTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new SHA1Hash();
            string failHash = "be7e10d1c5dd2ad77f6d5a617372a7bf13cb7bf";

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckHashTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            string hash = await handler.GetHashAsync(sample);
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }
    }
}