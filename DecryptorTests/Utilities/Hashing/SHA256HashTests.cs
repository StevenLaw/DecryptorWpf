using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class SHA256HashTests
    {
        private const string failHash = "6fe7d7112caaba1b1bc7bfa656974ac416cc525a7286117494f5b29dfec1f77";
        private const string fileHash = "58939fa578b72c8c18d85b1c49b68fabbef1cb819da2a6ecc475a843fb4b69";
        private const string filename = "Testing.txt";
        private const string resultHash = "bf67a78f571d7ecad67ad2a5ea64edc969def57649c69fbb22eb72f4c56f87a";
        private const string sample = "This is sample text";

        [TestMethod()]
        public async Task CheckFileHashAsyncTest()
        {
            // Arrange
            var handler = new SHA256Hash();

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, fileHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckFileHashFailTest()
        {
            // Arrange
            var handler = new SHA256Hash();

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncStreamTest()
        {
            // Arrange
            var handler = new SHA256Hash();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            bool matches = await handler.CheckHashAsync(stream, resultHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new SHA256Hash();

            // Act
            bool matches = await handler.CheckHashAsync(sample, resultHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckHashFailStreamTest()
        {
            // Arrange
            var handler = new SHA256Hash();
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
            var handler = new SHA256Hash();

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckFileHashTest()
        {
            // Arrange
            var handler = new SHA256Hash();

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
            var handler = new SHA256Hash();
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
            var handler = new SHA256Hash();

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
            var handler = new SHA256Hash();

            // Act
            string hash = await handler.GetFileHashAsync(filename);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task GetHashAsyncStreamTest()
        {
            // Arrange
            var handler = new SHA256Hash();
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
            var handler = new SHA256Hash();

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }
    }
}