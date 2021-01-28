using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Decryptor.Utilities.Encryption.Tests
{
    [TestClass()]
    public class EncryptionFactoryTests
    {
        [TestMethod()]
        public void CreateAesTest()
        {
            Assert.IsInstanceOfType(EncryptionFactory.Create(EncryptionAlgorithm.AES), typeof(SimpleAes));
        }
        [TestMethod()]
        public void CreateDesTest()
        {
            Assert.IsInstanceOfType(EncryptionFactory.Create(EncryptionAlgorithm.DES), typeof(SimpleDes));
        }
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateNoneTest()
        {
            EncryptionFactory.Create(EncryptionAlgorithm.None);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateBadTest()
        {
            EncryptionFactory.Create((EncryptionAlgorithm)100);
        }
    }
}