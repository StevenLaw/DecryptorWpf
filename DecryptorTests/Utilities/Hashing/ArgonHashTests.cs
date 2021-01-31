using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class ArgonHashTests
    { 
        private const int argonIterations = 4;
        private const int degrees = 8;
        private const string failHash = "$a2$8$4$1048576$16$16$rasN2h23lI2QHIo0ATikPQ==$l6ATQJOCSRUyoH2Q8AVbDw==";
        private const string fileHash = "$a2$8$4$1048576$16$16$bkdfGTkX/0lwNXBAGC2wYw==$VZ9pgywz7sCDfoiHxlt+Pw==";
        private const string filename = "Testing.txt";
        private const int hashLength = 16;
        private const int memorySpace = 1048576;
        private const string resultHash = "$a2$8$4$1048576$16$16$/X7A9uODAMLIU/lAIKlZ2A==$1QUD0xdIdp9cMqXxOvO6bw==";
        private const int saltLength = 16;
        private const string sample = "This is sample text";

        [TestMethod()]
        public async Task CheckFileHashAsyncTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, fileHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckFileHashFailTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncStreamTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);
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
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            // Act
            bool matches = await handler.CheckHashAsync(sample, resultHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckHashFailStreamTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);
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
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckFileHashTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

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
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);
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
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

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
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            // Act
            string hash = await handler.GetFileHashAsync(filename);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task GetHashAsyncStreamTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);
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
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }
    }
}