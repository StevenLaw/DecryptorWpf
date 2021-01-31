using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class SHA1HashTests
    {
        private const string failHash = "be7e10d1c5dd2ad77f6d5a617372a7bf13cb7bf";
        private const string fileHash = "873cd273cdff8922f7c3e182971ac9c87973b86a";
        private const string filename = "Testing.txt";
        private const string hash = "451d99c8281f579bdf1e2bfa2a63fd23707037";
        private const string sample = "This is sample text";

        [TestMethod()]
        public async Task CheckFileHashAsyncTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, fileHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckFileHashFailTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncStreamTest()
        {
            // Arrange
            var handler = new SHA1Hash();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            bool matches = await handler.CheckHashAsync(stream, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckHashFailStreamTest()
        {
            // Arrange
            var handler = new SHA1Hash();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            bool matches = await handler.CheckHashAsync(stream, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckFileHashTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            string hash = await handler.GetFileHashAsync(filename);
            bool matches = await handler.CheckFileHashAsync(filename, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task GetAndCheckHashStreamTest()
        {
            // Arrange
            var handler = new SHA1Hash();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            string hash = await handler.GetHashAsync(stream);
            stream.Position = 0;
            bool matches = await handler.CheckHashAsync(stream, hash);

            // Assert
            Assert.IsTrue(matches);
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

        [TestMethod()]
        public async Task GetFileHashAsyncTest()
        {
            // Arrange
            var handler = new SHA1Hash();

            // Act
            string hash = await handler.GetFileHashAsync(filename);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task GetHashAsyncStreamTest()
        {
            // Arrange
            var handler = new SHA1Hash();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            string hash = await handler.GetHashAsync(stream);

            // Assert
            Assert.IsTrue(hash.Length > 0);
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
    }
}