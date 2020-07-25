using Microsoft.VisualStudio.TestTools.UnitTesting;
using Decryptor.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Interop;
using System.Diagnostics;

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

        private HashManager GetHashManager(HashAlgorithm algorithm)
        {
            return new HashManager(algorithm, workFactor, scryptIterations, blockSize, threadCount, degrees, argonIterations, memorySpace);
        }

        [TestMethod()]
        public void BCryptGetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public void BCryptCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public void BCryptCheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public void BCryptGetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.BCrypt);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public void ScryptGetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public void ScryptCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);
            string hash = "$s2$16384$8$1$PNqOiuvttsZIc8aXI9HoC6qeHBk7wLeJgbf2VihYBwY=$9Zeb3/g+xb3Pc2ypxysBiUwKQ6SRQ/yqoy4xw/Qhp4g=";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        public void ScryptCheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);
            string failHash = "$s2$16384$8$1$MFPbkrwEu7NEK6NjJYPa0XU7tTHQm95WlpoGV8ro0SE=$+xALFipj8Rr+Its/xRcsNx2ZM5rJovAs+NgTDEG2iJg=";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public void ScryptGetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Scrypt);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void Argon2GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void Argon2CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void Argon2CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void Argon2GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.Argon2);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void MD5GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void MD5CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void MD5CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void MD5GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.MD5);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA1GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA1CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA1CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA1GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA1);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA256GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA256CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA256CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA256GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA256);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA512GetHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA512CheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA512CheckHashFailTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SHA512GetAndCheckHashTest()
        {
            // Arrange
            var hm = GetHashManager(HashAlgorithm.SHA512);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }
    }
}