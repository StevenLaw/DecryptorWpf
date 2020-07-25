using Microsoft.VisualStudio.TestTools.UnitTesting;
using Decryptor.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Diagnostics;

namespace Decryptor.Utilities.Tests
{
    [TestClass()]
    public class PasswordProtectorTests
    {
        private const string sample = "This is a sample string to encrypt/secure";
        private SecureString secureSample;

        public PasswordProtectorTests()
        {
            secureSample = new SecureString();
            foreach (char c in sample)
            {
                secureSample.AppendChar(c);
            }
            secureSample.MakeReadOnly();
        }

        [TestMethod()]
        public void EncryptThenDecryptStringTest()
        {
            // Act
            string encrypted = PasswordProtector.GetEncryptedString(secureSample);
            var decrypted = PasswordProtector.DecryptString(encrypted);
            Debug.WriteLine(encrypted);

            // Assert
            Assert.AreEqual(secureSample.Length, decrypted.Length);
            Assert.AreEqual(secureSample.ToInsecureString(), decrypted.ToInsecureString());
        }

        [TestMethod()]
        public void ToInsecureStringTest()
        {
            // Act
            string insecure = secureSample.ToInsecureString();

            // Assert
            Assert.AreEqual(secureSample.Length, insecure.Length);
        }

        [TestMethod()]
        public void ToSecureStringTest()
        {
            // Act
            var secure = sample.ToSecureString();

            // Assert
            Assert.AreEqual(sample.Length, secure.Length);
        }
    }
}