using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class SHA512HashTests
    {
        private const string sample = "This is sample text";

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new SHA512Hash();
            string hash = "cce3233e201810a61541ecc0e2a69bda7de751d8f35c2fcd7b19bd612543ae18885a1d7d32f3c46cc5cab91268a93b3d4431d3b14d6eee1e44954a4513b";

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task GetHashAsyncTest()
        {
            // Arrange
            var handler = new SHA512Hash();

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new SHA512Hash();
            string failHash = "c85fb94f63c5b9f93e495fb57589656d31b4411ec793a11fde64039fdaa2da859d14d41672414864691637f8265072f64e238d7ce277437927ca7ccbfa869c";

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckHashTest()
        {
            // Arrange
            var handler = new SHA512Hash();

            // Act
            string hash = await handler.GetHashAsync(sample);
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }
    }
}