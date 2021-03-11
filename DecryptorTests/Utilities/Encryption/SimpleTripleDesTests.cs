using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests
{
    [TestClass()]
    public class SimpleTripleDesTests
    {
        private readonly SecureString key;
        private const string insecureKey = "This is an insecure key for testing";
        private const string sample = "This is a sample";
        private const string result1 = "woW0IrHPTTZayl67KmNJUeQwFOEoLJxe";
        private const string result2 = "IB3M3Mr3Y2CTgBqXSA6/YmqYfOEOGuNd";
        private const string testFile = "Testing.txt";
        private const string cryptFile1 = "Testing1.txt.3ds";
        private const string cryptFile2 = "Testing2.txt.3ds";
        private const string testCryptFile1 = "Crypt1.txt.3ds";
        private const string testCryptFile2 = "Crypt2.txt.3ds";
        private const string outFile1 = "Out1.txt";
        private const string outFile2 = "Out1.txt";

        public SimpleTripleDesTests()
        {
            key = new SecureString();
            foreach (char c in insecureKey)
            {
                key.AppendChar(c);
            }
            key.MakeReadOnly();
        }

        [TestInitialize]
        public void InitTests()
        {
            if (File.Exists(cryptFile1))
                File.Delete(cryptFile1);
            if (File.Exists(cryptFile2))
                File.Delete(cryptFile2);
            if (File.Exists(outFile1))
                File.Delete(outFile1);
            if (File.Exists(outFile2))
                File.Delete(outFile2);
        }

        [TestMethod]
        public async Task EncryptTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            string encrypted1 = await tDes1.EncryptAsync(sample);
            string encrypted2 = await tDes2.EncryptAsync(sample);
            //Debug.WriteLine(encrypted);

            // Assert
            Assert.AreEqual(result1, encrypted1);
            Assert.AreEqual(result2, encrypted2);
            Assert.AreNotEqual(encrypted1, encrypted2);
        }

        [TestMethod()]
        public async Task EncryptStreamTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);
            using var ms1 = new MemoryStream(Encoding.UTF8.GetBytes(sample));
            using var ms2 = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            byte[] encrypted1 = await tDes1.EncryptAsync(ms1);
            byte[] encrypted2 = await tDes2.EncryptAsync(ms2);
            string encStr1 = Convert.ToBase64String(encrypted1);
            string encStr2 = Convert.ToBase64String(encrypted2);

            // Assert
            Assert.AreEqual(result1, encStr1);
            Assert.AreEqual(result2, encStr2);
            Assert.AreNotEqual(encStr1, encStr2);
        }

        [TestMethod()]
        public async Task EncryptFileTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            await tDes1.EncryptAsync(testFile, cryptFile1);
            await tDes2.EncryptAsync(testFile, cryptFile2);

            // Assert
            Assert.IsTrue(File.Exists(cryptFile1));
            Assert.IsTrue(File.Exists(cryptFile2));
            Assert.AreNotEqual(File.ReadAllBytes(cryptFile1), File.ReadAllBytes(cryptFile2));
        }

        [TestMethod]
        public async Task DecryptTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            string decrypted1 = await tDes1.DecryptAsync(result1);
            string decrypted2 = await tDes2.DecryptAsync(result2);

            // Assert
            Assert.AreEqual(sample, decrypted1);
            Assert.AreEqual(sample, decrypted2);
        }

        [TestMethod()]
        public async Task DecryptStreamTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);
            var bytes1 = Convert.FromBase64String(result1);
            var bytes2 = Convert.FromBase64String(result2);
            using var ms1 = new MemoryStream(bytes1);
            using var ms2 = new MemoryStream(bytes2);

            // Act
            byte[] decrypted1 = await tDes1.DecryptAsync(ms1);
            byte[] decrypted2 = await tDes2.DecryptAsync(ms2);
            var decStr1 = Encoding.UTF8.GetString(decrypted1);
            var decStr2 = Encoding.UTF8.GetString(decrypted2);

            // Assert
            Assert.AreEqual(sample, decStr1);
            Assert.AreEqual(sample, decStr2);
            Assert.AreEqual(decStr1, decStr2);
        }

        [TestMethod()]
        public async Task DecryptFileTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            await tDes1.DecryptAsync(testCryptFile1, outFile1);
            await tDes2.DecryptAsync(testCryptFile2, outFile2);

            // Assert
            Assert.IsTrue(File.Exists(outFile1));
            Assert.IsTrue(File.Exists(outFile2));
            Assert.AreEqual(File.ReadAllText(outFile1), File.ReadAllText(outFile2));
        }

        [TestMethod]
        public async Task EncryptDecryptTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            string encrypted1 = await tDes1.EncryptAsync(sample);
            string encrypted2 = await tDes2.EncryptAsync(sample);
            string decrypted1 = await tDes1.DecryptAsync(encrypted1);
            string decrypted2 = await tDes2.DecryptAsync(encrypted2);

            // Assert
            Assert.AreEqual(sample, decrypted1);
            Assert.AreEqual(sample, decrypted2);
            Assert.AreNotEqual(encrypted1, encrypted2);
            Assert.AreEqual(decrypted1, decrypted2);
        }

        [TestMethod]
        public async Task EncryptDecryptStreamTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);
            using var inStream1 = new MemoryStream(Encoding.UTF8.GetBytes(sample));
            using var inStream2 = new MemoryStream(Encoding.UTF8.GetBytes(sample));

            // Act
            byte[] encrypted1 = await tDes1.EncryptAsync(inStream1);
            byte[] encrypted2 = await tDes2.EncryptAsync(inStream2);
            using var outStream1 = new MemoryStream(encrypted1);
            using var outStream2 = new MemoryStream(encrypted2);
            byte[] decrypted1 = await tDes1.DecryptAsync(outStream1);
            byte[] decrypted2 = await tDes2.DecryptAsync(outStream2);
            string decStr1 = Encoding.UTF8.GetString(decrypted1);
            string decStr2 = Encoding.UTF8.GetString(decrypted2);

            // Assert
            Assert.AreEqual(sample, decStr1);
            Assert.AreEqual(sample, decStr2);
            Assert.AreNotEqual(encrypted1, encrypted2);
            Assert.AreEqual(decStr1, decStr2);
        }

        [TestMethod]
        public async Task EncryptDecryptFileTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            await tDes1.EncryptAsync(testFile, cryptFile1);
            await tDes2.EncryptAsync(testFile, cryptFile2);
            await tDes1.DecryptAsync(cryptFile1, outFile1);
            await tDes2.DecryptAsync(cryptFile2, outFile2);

            // Assert
            Assert.AreEqual(File.ReadAllText(testFile), File.ReadAllText(outFile1));
            Assert.AreEqual(File.ReadAllText(testFile), File.ReadAllText(outFile2));
        }

        [TestMethod]
        public async Task DecryptEncryptTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            string decrypted1 = await tDes1.DecryptAsync(result1);
            string decrypted2 = await tDes2.DecryptAsync(result2);
            string encrypted1 = await tDes1.EncryptAsync(decrypted1);
            string encrypted2 = await tDes2.EncryptAsync(decrypted2);

            // Assert
            Assert.AreEqual(result1, encrypted1);
            Assert.AreEqual(result2, encrypted2);
            Assert.AreEqual(decrypted1, decrypted2);
            Assert.AreNotEqual(encrypted1, encrypted2);
        }

        [TestMethod]
        public async Task DecryptEncryptStreamTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);
            using var inStream1 = new MemoryStream(Convert.FromBase64String(result1));
            using var inStream2 = new MemoryStream(Convert.FromBase64String(result2));

            // Act
            byte[] decrypted1 = await tDes1.DecryptAsync(inStream1);
            byte[] decrypted2 = await tDes2.DecryptAsync(inStream2);
            string decStr1 = Convert.ToBase64String(decrypted1);
            string decStr2 = Convert.ToBase64String(decrypted2);
            using var outStream1 = new MemoryStream(decrypted1);
            using var outStream2 = new MemoryStream(decrypted2);
            byte[] encrypted1 = await tDes1.EncryptAsync(outStream1);
            byte[] encrypted2 = await tDes2.EncryptAsync(outStream2);
            string encStr1 = Convert.ToBase64String(encrypted1);
            string encStr2 = Convert.ToBase64String(encrypted2);

            // Assert
            Assert.AreEqual(result1, encStr1);
            Assert.AreEqual(result2, encStr2);
            Assert.AreEqual(decStr1, decStr2);
            Assert.AreNotEqual(encStr1, encStr2);
        }

        [TestMethod]
        public async Task DecryptEncryptFileTest()
        {
            // Arrange
            var tDes1 = new SimpleTripleDes(key, TripleDesKeySize.b128);
            var tDes2 = new SimpleTripleDes(key, TripleDesKeySize.b192);

            // Act
            await tDes1.DecryptAsync(testCryptFile1, outFile1);
            await tDes2.DecryptAsync(testCryptFile2, outFile2);
            await tDes1.EncryptAsync(outFile1, cryptFile1);
            await tDes2.EncryptAsync(outFile2, cryptFile2);

            // Assert
            Assert.AreEqual(File.ReadAllText(testCryptFile1), File.ReadAllText(cryptFile1));
            Assert.AreEqual(File.ReadAllText(testCryptFile2), File.ReadAllText(cryptFile2));
        }
    }
}