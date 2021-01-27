using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class BCryptHashTests
    {
        private const string sample = "This is sample text";
        private const int workFactor = 12;

        [TestMethod()]
        public void BCryptHashHandlerTest()
        {
            var handler = new BCryptHash(workFactor);

            Assert.IsInstanceOfType(handler, typeof(BCryptHash));
        }

        [TestMethod()]
        public async Task CheckHashAsyncTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = await handler.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
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

        [TestMethod]
        public async Task CheckHashFailTest()
        {
            // Arrange
            var handler = new BCryptHash(workFactor);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
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
    }
}