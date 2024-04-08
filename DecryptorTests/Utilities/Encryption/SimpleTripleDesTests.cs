using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests;

[TestClass()]
public class SimpleTripleDesTests
{
    private readonly SecureString _key;
    private const string INSECURE_KEY = "This is an insecure key for testing";
    private const string SAMPLE = "This is a sample";
    private const string RESULT_1 = "woW0IrHPTTZayl67KmNJUeQwFOEoLJxe";
    private const string RESULT_2 = "IB3M3Mr3Y2CTgBqXSA6/YmqYfOEOGuNd";
    private const string TEST_FILE = "Testing.txt";
    private const string CRYPT_FILE_1 = "Testing1.txt.3ds";
    private const string CRYPT_FILE_2 = "Testing2.txt.3ds";
    private const string TEST_CRYPT_FILE_1 = "Crypt1.txt.3ds";
    private const string TEST_CRYPT_FILE_2 = "Crypt2.txt.3ds";
    private const string OUT_FILE_1 = "Out1.txt";
    private const string OUT_FILE_2 = "Out1.txt";

    public SimpleTripleDesTests()
    {
        _key = new SecureString();
        foreach (char c in INSECURE_KEY)
        {
            _key.AppendChar(c);
        }
        _key.MakeReadOnly();
    }

    [TestInitialize]
    public void InitTests()
    {
        if (File.Exists(CRYPT_FILE_1))
            File.Delete(CRYPT_FILE_1);
        if (File.Exists(CRYPT_FILE_2))
            File.Delete(CRYPT_FILE_2);
        if (File.Exists(OUT_FILE_1))
            File.Delete(OUT_FILE_1);
        if (File.Exists(OUT_FILE_2))
            File.Delete(OUT_FILE_2);
    }

    [TestMethod]
    public async Task EncryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string encrypted1 = await tDes1.EncryptAsync(SAMPLE);
        string encrypted2 = await tDes2.EncryptAsync(SAMPLE);
        //Debug.WriteLine(encrypted);

        // Assert
        Assert.AreEqual(RESULT_1, encrypted1);
        Assert.AreEqual(RESULT_2, encrypted2);
        Assert.AreNotEqual(encrypted1, encrypted2);
    }

    [TestMethod()]
    public async Task EncryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        using var ms1 = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));
        using var ms2 = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        byte[] encrypted1 = await tDes1.EncryptAsync(ms1);
        byte[] encrypted2 = await tDes2.EncryptAsync(ms2);
        string encStr1 = Convert.ToBase64String(encrypted1);
        string encStr2 = Convert.ToBase64String(encrypted2);

        // Assert
        Assert.AreEqual(RESULT_1, encStr1);
        Assert.AreEqual(RESULT_2, encStr2);
        Assert.AreNotEqual(encStr1, encStr2);
    }

    [TestMethod()]
    public async Task EncryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.EncryptAsync(TEST_FILE, CRYPT_FILE_1);
        await tDes2.EncryptAsync(TEST_FILE, CRYPT_FILE_2);

        // Assert
        Assert.IsTrue(File.Exists(CRYPT_FILE_1));
        Assert.IsTrue(File.Exists(CRYPT_FILE_2));
        Assert.AreNotEqual(File.ReadAllBytes(CRYPT_FILE_1), File.ReadAllBytes(CRYPT_FILE_2));
    }

    [TestMethod]
    public async Task DecryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string decrypted1 = await tDes1.DecryptAsync(RESULT_1);
        string decrypted2 = await tDes2.DecryptAsync(RESULT_2);

        // Assert
        Assert.AreEqual(SAMPLE, decrypted1);
        Assert.AreEqual(SAMPLE, decrypted2);
    }

    [TestMethod()]
    public async Task DecryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        var bytes1 = Convert.FromBase64String(RESULT_1);
        var bytes2 = Convert.FromBase64String(RESULT_2);
        using var ms1 = new MemoryStream(bytes1);
        using var ms2 = new MemoryStream(bytes2);

        // Act
        byte[] decrypted1 = await tDes1.DecryptAsync(ms1);
        byte[] decrypted2 = await tDes2.DecryptAsync(ms2);
        var decStr1 = Encoding.UTF8.GetString(decrypted1);
        var decStr2 = Encoding.UTF8.GetString(decrypted2);

        // Assert
        Assert.AreEqual(SAMPLE, decStr1);
        Assert.AreEqual(SAMPLE, decStr2);
        Assert.AreEqual(decStr1, decStr2);
    }

    [TestMethod()]
    public async Task DecryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.DecryptAsync(TEST_CRYPT_FILE_1, OUT_FILE_1);
        await tDes2.DecryptAsync(TEST_CRYPT_FILE_2, OUT_FILE_2);

        // Assert
        Assert.IsTrue(File.Exists(OUT_FILE_1));
        Assert.IsTrue(File.Exists(OUT_FILE_2));
        Assert.AreEqual(File.ReadAllText(OUT_FILE_1), File.ReadAllText(OUT_FILE_2));
    }

    [TestMethod]
    public async Task EncryptDecryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string encrypted1 = await tDes1.EncryptAsync(SAMPLE);
        string encrypted2 = await tDes2.EncryptAsync(SAMPLE);
        string decrypted1 = await tDes1.DecryptAsync(encrypted1);
        string decrypted2 = await tDes2.DecryptAsync(encrypted2);

        // Assert
        Assert.AreEqual(SAMPLE, decrypted1);
        Assert.AreEqual(SAMPLE, decrypted2);
        Assert.AreNotEqual(encrypted1, encrypted2);
        Assert.AreEqual(decrypted1, decrypted2);
    }

    [TestMethod]
    public async Task EncryptDecryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        using var inStream1 = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));
        using var inStream2 = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

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
        Assert.AreEqual(SAMPLE, decStr1);
        Assert.AreEqual(SAMPLE, decStr2);
        Assert.AreNotEqual(encrypted1, encrypted2);
        Assert.AreEqual(decStr1, decStr2);
    }

    [TestMethod]
    public async Task EncryptDecryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.EncryptAsync(TEST_FILE, CRYPT_FILE_1);
        await tDes2.EncryptAsync(TEST_FILE, CRYPT_FILE_2);
        await tDes1.DecryptAsync(CRYPT_FILE_1, OUT_FILE_1);
        await tDes2.DecryptAsync(CRYPT_FILE_2, OUT_FILE_2);

        // Assert
        Assert.AreEqual(File.ReadAllText(TEST_FILE), File.ReadAllText(OUT_FILE_1));
        Assert.AreEqual(File.ReadAllText(TEST_FILE), File.ReadAllText(OUT_FILE_2));
    }

    [TestMethod]
    public async Task DecryptEncryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string decrypted1 = await tDes1.DecryptAsync(RESULT_1);
        string decrypted2 = await tDes2.DecryptAsync(RESULT_2);
        string encrypted1 = await tDes1.EncryptAsync(decrypted1);
        string encrypted2 = await tDes2.EncryptAsync(decrypted2);

        // Assert
        Assert.AreEqual(RESULT_1, encrypted1);
        Assert.AreEqual(RESULT_2, encrypted2);
        Assert.AreEqual(decrypted1, decrypted2);
        Assert.AreNotEqual(encrypted1, encrypted2);
    }

    [TestMethod]
    public async Task DecryptEncryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        using var inStream1 = new MemoryStream(Convert.FromBase64String(RESULT_1));
        using var inStream2 = new MemoryStream(Convert.FromBase64String(RESULT_2));

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
        Assert.AreEqual(RESULT_1, encStr1);
        Assert.AreEqual(RESULT_2, encStr2);
        Assert.AreEqual(decStr1, decStr2);
        Assert.AreNotEqual(encStr1, encStr2);
    }

    [TestMethod]
    public async Task DecryptEncryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.DecryptAsync(TEST_CRYPT_FILE_1, OUT_FILE_1);
        await tDes2.DecryptAsync(TEST_CRYPT_FILE_2, OUT_FILE_2);
        await tDes1.EncryptAsync(OUT_FILE_1, CRYPT_FILE_1);
        await tDes2.EncryptAsync(OUT_FILE_2, CRYPT_FILE_2);

        // Assert
        Assert.AreEqual(File.ReadAllText(TEST_CRYPT_FILE_1), File.ReadAllText(CRYPT_FILE_1));
        Assert.AreEqual(File.ReadAllText(TEST_CRYPT_FILE_2), File.ReadAllText(CRYPT_FILE_2));
    }
}