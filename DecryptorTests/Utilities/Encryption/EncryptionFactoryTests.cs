using Decryptor.Enums;
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
        [TestMethod]
        public void CreateTripleDesTest()
        {
            var s3Des1 = EncryptionFactory.Create(EncryptionAlgorithm.TripleDES);
            //var s3Des2 = EncryptionFactory.Create(EncryptionAlgorithm.TripleDES, new EncryptionOptions { TripleDesKeySize = TripleDesKeySize.b128 });
            //var s3Des3 = EncryptionFactory.Create(EncryptionAlgorithm.TripleDES, new EncryptionOptions { TripleDesKeySize = TripleDesKeySize.b192 });
            Assert.IsInstanceOfType(s3Des1, typeof(SimpleTripleDes));
            //Assert.IsInstanceOfType(s3Des2, typeof(SimpleTripleDes));
            //Assert.IsInstanceOfType(s3Des3, typeof(SimpleTripleDes));
            Assert.AreEqual((s3Des1 as SimpleTripleDes).KeySizeMode, TripleDesKeySize.b128);
            //Assert.AreEqual((s3Des2 as SimpleTripleDes).KeySizeMode, TripleDesKeySize.b128);
            //Assert.AreEqual((s3Des3 as SimpleTripleDes).KeySizeMode, TripleDesKeySize.b192);
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