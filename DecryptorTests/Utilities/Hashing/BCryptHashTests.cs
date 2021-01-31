using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class BCryptHashTests
    {
        private const string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";
        private const string fileHash = "$2a$12$.V7xrlvJETTocuQjqGAN6O2Fqz.3CoOsepgSRCPFUB538yqrUsHKG";
        private const string filename = "Testing.txt";
        private const string resultHash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";
        private const string sample = "This is sample text";
        private const int workFactor = 12;

        [TestMethod()]
        public async Task CheckFileHashAsyncTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, fileHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckFileHashFailTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncStreamTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);
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
            var handler = new BCryptHash(workFactor);

            // Act
            bool matches = await handler.CheckHashAsync(sample, resultHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckHashFailStreamTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);
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
            var handler = new BCryptHash(workFactor);

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckFileHashTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);

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
            var handler = new BCryptHash(workFactor);
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
            var handler = new BCryptHash(workFactor);

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
            var handler = new BCryptHash(workFactor);

            // Act
            string hash = await handler.GetFileHashAsync(filename);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task GetHashAsyncStreamTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);
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
            var handler = new BCryptHash(workFactor);

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }
    }
}