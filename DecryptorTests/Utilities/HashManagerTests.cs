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
        private const int degrees = 8;
        private const int iterations = 4;
        private const int memorySpace = 1048576;

        [TestMethod()]
        public void BCryptGetHashTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.BCrypt, workFactor, degrees, iterations, memorySpace);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public void BCryptCheckHashTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.BCrypt, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.BCrypt, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.BCrypt, workFactor, degrees, iterations, memorySpace);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ScryptGetHashTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.Scrypt, workFactor, degrees, iterations, memorySpace);

            // Act
            string hash = hm.GetHash(sample);

            // Assert
            //Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ScryptCheckHashTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.Scrypt, workFactor, degrees, iterations, memorySpace);
            string hash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";

            // Act
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ScryptCheckHashFailTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.Scrypt, workFactor, degrees, iterations, memorySpace);
            string failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";

            // Act
            bool matches = hm.CheckHash(sample, failHash);

            // Assert
            //Assert.IsFalse(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ScryptGetAndCheckHashTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.Scrypt, workFactor, degrees, iterations, memorySpace);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void Argon2GetHashTest()
        {
            // Arrange
            var hm = new HashManager(HashAlgorithm.Argon2, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.Argon2, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.Argon2, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.Argon2, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.MD5, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.MD5, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.MD5, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.MD5, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.SHA1, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.SHA1, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.SHA1, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.SHA1, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.SHA256, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.SHA256, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.SHA256, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.SHA256, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.SHA512, workFactor, degrees, iterations, memorySpace);

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
            var hm = new HashManager(HashAlgorithm.SHA512, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.SHA512, workFactor, degrees, iterations, memorySpace);
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
            var hm = new HashManager(HashAlgorithm.SHA512, workFactor, degrees, iterations, memorySpace);

            // Act
            string hash = hm.GetHash(sample);
            bool matches = hm.CheckHash(sample, hash);

            // Assert
            //Assert.IsTrue(matches);
        }
    }
}