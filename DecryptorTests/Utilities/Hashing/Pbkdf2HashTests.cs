using Microsoft.VisualStudio.TestTools.UnitTesting;
using Decryptor.Utilities.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class Pbkdf2HashTests
    {
        private const string failHash = "$TEST$V1$10000$k3AF2eDUs/P7eTHWP1UPbZ8DEeROYxnAemGWlScnuxU3dq0S";
        private const string fileHash = "$TEST$V1$10000$r/FrbrwsQxTV/2oVG7hr+WtKkX3FJJ/lgLwLx8hjuCynNeB9";
        private const string filename = "Testing.txt";
        private const string resultHash = "$TEST$V1$10000$vv+lw9p6POPFCFqAKq3yjaY8RTldUJF/ANFLxYePJEfjSWYl";
        private const string sample = "This is sample text";
        private const string _prefix = "$TEST$V1$";
        private const int _saltSize = 16;
        private const int _hashSize = 20;
        private const int _iterations = 10000;


        [TestMethod()]
        public async Task CheckFileHashAsyncTest()
        {
            // Arrange
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, fileHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckFileHashFailTest()
        {
            // Arrange
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

            // Act
            bool matches = await handler.CheckFileHashAsync(filename, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod()]
        public async Task CheckHashAsyncStreamTest()
        {
            // Arrange
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);
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
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

            // Act
            bool matches = await handler.CheckHashAsync(sample, resultHash);

            // Assert
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public async Task CheckHashFailStreamTest()
        {
            // Arrange
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);
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
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

            // Act
            bool matches = await handler.CheckHashAsync(sample, failHash);

            // Assert
            Assert.IsFalse(matches);
        }

        [TestMethod]
        public async Task GetAndCheckFileHashTest()
        {
            // Arrange
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

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
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);
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
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

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
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

            // Act
            string hash = await handler.GetFileHashAsync(filename);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod()]
        public async Task GetHashAsyncStreamTest()
        {
            // Arrange
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);
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
            var handler = new Pbkdf2Hash(_iterations, _saltSize, _hashSize, _prefix);

            // Act
            string hash = await handler.GetHashAsync(sample);

            // Assert
            Assert.IsTrue(hash.Length > 0);
        }
    }
}