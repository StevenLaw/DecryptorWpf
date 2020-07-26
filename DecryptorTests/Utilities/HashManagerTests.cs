using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Tests
{
    [TestClass()]
    public class HashManagerTests
    {
        private const string sample = "This is sample text";
        private const int workFactor = 12;
        private const int scryptIterations = 16384;
        private const int blockSize = 8;
        private const int threadCount = 1;
        private const int degrees = 8;
        private const int argonIterations = 4;
        private const int memorySpace = 1048576;
        private const int saltLength = 16;
        private const int hashLength = 16;

        private HashManager GetHashManager(HashAlgorithm algorithm)
        {
            return new HashManager(algorithm, workFactor, scryptIterations, blockSize, threadCount, degrees,
                                   argonIterations, memorySpace, saltLength, hashLength);
        }

        [TestMethod()]
        public async Task BCryptGetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task BCryptCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task BCryptCheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task BCryptGetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task ScryptGetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task ScryptCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);
            string hash = "$s2$16384$8$1$PNqOiuvttsZIc8aXI9HoC6qeHBk7wLeJgbf2VihYBwY=$9Zeb3/g+xb3Pc2ypxysBiUwKQ6SRQ/yqoy4xw/Qhp4g=";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task ScryptCheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);
            string failHash = "$s2$16384$8$1$MFPbkrwEu7NEK6NjJYPa0XU7tTHQm95WlpoGV8ro0SE=$+xALFipj8Rr+Its/xRcsNx2ZM5rJovAs+NgTDEG2iJg=";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task ScryptGetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task Argon2GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task Argon2CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);
            string hash = "$a2$8$4$1048576$16$16$/X7A9uODAMLIU/lAIKlZ2A==$1QUD0xdIdp9cMqXxOvO6bw==";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task Argon2CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);
            string failHash = "$a2$8$4$1048576$16$16$rasN2h23lI2QHIo0ATikPQ==$l6ATQJOCSRUyoH2Q8AVbDw==";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task Argon2GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task MD5GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task MD5CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);
            string hash = "636351fcb9197f5e75b84562858bbb1";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task MD5CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);
            string failHash = "7ff3e75ce6aca348bc513ed3d5882946";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task MD5GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task SHA1GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task SHA1CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);
            string hash = "451d99c8281f579bdf1e2bfa2a63fd23707037";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public async Task SHA1CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);
            string failHash = "be7e10d1c5dd2ad77f6d5a617372a7bf13cb7bf";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task SHA1GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA256GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA256CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA256CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA256GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA512GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);

            // Act
            string hash = await hm.GetHashAsync(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA512CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA512CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = await hm.CheckHashAsync(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task SHA512GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);

            // Act
            string hash = await hm.GetHashAsync(sample);
            bool matches = await hm.CheckHashAsync(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }
    }
}