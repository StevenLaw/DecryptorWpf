using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class ScryptHashTests
    {
        private const string sample = "This is sample text";
        private const int scryptIterations = 16384;
        private const int blockSize = 8;
        private const int threadCount = 1;

        [TestMethod()]
        public void ScryptHashHandlerTest()
        {
            var handler = new ScryptHash(scryptIterations, blockSize, threadCount);

            Assert.IsInstanceOfType(handler, typeof(ScryptHash));
        }

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new ScryptHash(scryptIterations, blockSize, threadCount);
            string hash = "$s2$16384$8$1$PNqOiuvttsZIc8aXI9HoC6qeHBk7wLeJgbf2VihYBwY=$9Zeb3/g+xb3Pc2ypxysBiUwKQ6SRQ/yqoy4xw/Qhp4g=";

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task GetHashAsyncTest()
        {
            // Arrange
            var handler = new ScryptHash(scryptIterations, blockSize, threadCount);

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new ScryptHash(scryptIterations, blockSize, threadCount);
            string failHash = "$s2$16384$8$1$MFPbkrwEu7NEK6NjJYPa0XU7tTHQm95WlpoGV8ro0SE=$+xALFipj8Rr+Its/xRcsNx2ZM5rJovAs+NgTDEG2iJg=";

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckHashTest()
        {
            // Arrange
            var handler = new ScryptHash(scryptIterations, blockSize, threadCount);

            // Act
            string hash = await handler.GetHashAsync(sample);
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }
    }
}