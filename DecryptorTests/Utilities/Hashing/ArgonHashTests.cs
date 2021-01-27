using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class ArgonHashTests
    {
        private const string sample = "This is sample text";
        private const int degrees = 8;
        private const int argonIterations = 4;
        private const int memorySpace = 1048576;
        private const int saltLength = 16;
        private const int hashLength = 16;

        [TestMethod()]
        public void ArgonHashingTest()
        {
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);

            Assert.IsInstanceOfType(handler, typeof(ArgonHash));
        }

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);
            string hash = "$a2$8$4$1048576$16$16$/X7A9uODAMLIU/lAIKlZ2A==$1QUD0xdIdp9cMqXxOvO6bw==";

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
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

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new ArgonHash(degrees, argonIterations, memorySpace, saltLength, hashLength);
            string failHash = "$a2$8$4$1048576$16$16$rasN2h23lI2QHIo0ATikPQ==$l6ATQJOCSRUyoH2Q8AVbDw==";

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
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
    }
}