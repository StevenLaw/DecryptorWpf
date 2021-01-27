using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class SHA256HashTests
    {
        private const string sample = "This is sample text";

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new SHA256Hash();
            string hash = "bf67a78f571d7ecad67ad2a5ea64edc969def57649c69fbb22eb72f4c56f87a";

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
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

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new SHA256Hash();
            string failHash = "6fe7d7112caaba1b1bc7bfa656974ac416cc525a7286117494f5b29dfec1f77";

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
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
    }
}