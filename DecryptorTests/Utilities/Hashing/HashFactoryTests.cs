using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Decryptor.Utilities.Hashing.Tests
{
    [TestClass()]
    public class HashFactoryTests
    {
        [TestMethod()]
        public void CreateBCryptTest()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.BCrypt), typeof(BCryptHash));
        }

        [TestMethod()]
        public void CreateScryptTest()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.Scrypt), typeof(ScryptHash));
        }

        [TestMethod()]
        public void CreateArgon2Test()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.Argon2), typeof(ArgonHash));
        }

        [TestMethod()]
        public void CreateMD5Test()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.MD5), typeof(MD5Hash));
        }

        [TestMethod()]
        public void CreateSHA1Test()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.SHA1), typeof(SHA1Hash));
        }

        [TestMethod()]
        public void CreateSHA256Test()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.SHA256), typeof(SHA256Hash));
        }

        [TestMethod()]
        public void CreateSHA512Test()
        {
            Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.SHA512), typeof(SHA512Hash));
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateNoneTest()
        {
            HashFactory.Create(HashAlgorithm.None);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateBadTest()
        {
            HashFactory.Create((HashAlgorithm)100);
        }
    }
}