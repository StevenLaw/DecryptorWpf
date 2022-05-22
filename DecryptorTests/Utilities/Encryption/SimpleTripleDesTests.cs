using Decryptor.Core.Utilities.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Encryption;

[TestClass()]
public class SimpleTripleDesTests
{
    private const string _cryptFile1 = "Testing1.txt.3ds";
    private const string _cryptFile2 = "Testing2.txt.3ds";
    private const string _insecureKey = "This is an insecure key for testing";
    private const string _outFile1 = "Out1.txt";
    private const string _outFile2 = "Out1.txt";
    private const string _result1 = "woW0IrHPTTZayl67KmNJUeQwFOEoLJxe";
    private const string _result2 = "IB3M3Mr3Y2CTgBqXSA6/YmqYfOEOGuNd";
    private const string _sample = "This is a sample";
    private const string _testCryptFile1 = "Crypt1.txt.3ds";
    private const string _testCryptFile2 = "Crypt2.txt.3ds";
    private const string _testFile = "Testing.txt";
    private readonly SecureString _key;

    public SimpleTripleDesTests()
    {
        _key = new SecureString();
        foreach (char c in _insecureKey)
        {
            _key.AppendChar(c);
        }
        _key.MakeReadOnly();
    }

    [TestMethod]
    public async Task DecryptEncryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.DecryptAsync(_testCryptFile1, _outFile1);
        await tDes2.DecryptAsync(_testCryptFile2, _outFile2);
        await tDes1.EncryptAsync(_outFile1, _cryptFile1);
        await tDes2.EncryptAsync(_outFile2, _cryptFile2);

        // Assert
        Assert.AreEqual(File.ReadAllText(_testCryptFile1), File.ReadAllText(_cryptFile1));
        Assert.AreEqual(File.ReadAllText(_testCryptFile2), File.ReadAllText(_cryptFile2));
    }

    [TestMethod]
    public async Task DecryptEncryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        using var inStream1 = new MemoryStream(Convert.FromBase64String(_result1));
        using var inStream2 = new MemoryStream(Convert.FromBase64String(_result2));

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
        Assert.AreEqual(_result1, encStr1);
        Assert.AreEqual(_result2, encStr2);
        Assert.AreEqual(decStr1, decStr2);
        Assert.AreNotEqual(encStr1, encStr2);
    }

    [TestMethod]
    public async Task DecryptEncryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string decrypted1 = await tDes1.DecryptAsync(_result1);
        string decrypted2 = await tDes2.DecryptAsync(_result2);
        string encrypted1 = await tDes1.EncryptAsync(decrypted1);
        string encrypted2 = await tDes2.EncryptAsync(decrypted2);

        // Assert
        Assert.AreEqual(_result1, encrypted1);
        Assert.AreEqual(_result2, encrypted2);
        Assert.AreEqual(decrypted1, decrypted2);
        Assert.AreNotEqual(encrypted1, encrypted2);
    }

    [TestMethod()]
    public async Task DecryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.DecryptAsync(_testCryptFile1, _outFile1);
        await tDes2.DecryptAsync(_testCryptFile2, _outFile2);

        // Assert
        Assert.IsTrue(File.Exists(_outFile1));
        Assert.IsTrue(File.Exists(_outFile2));
        Assert.AreEqual(File.ReadAllText(_outFile1), File.ReadAllText(_outFile2));
    }

    [TestMethod()]
    public async Task DecryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        var bytes1 = Convert.FromBase64String(_result1);
        var bytes2 = Convert.FromBase64String(_result2);
        using var ms1 = new MemoryStream(bytes1);
        using var ms2 = new MemoryStream(bytes2);

        // Act
        byte[] decrypted1 = await tDes1.DecryptAsync(ms1);
        byte[] decrypted2 = await tDes2.DecryptAsync(ms2);
        var decStr1 = Encoding.UTF8.GetString(decrypted1);
        var decStr2 = Encoding.UTF8.GetString(decrypted2);

        // Assert
        Assert.AreEqual(_sample, decStr1);
        Assert.AreEqual(_sample, decStr2);
        Assert.AreEqual(decStr1, decStr2);
    }

    [TestMethod]
    public async Task DecryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string decrypted1 = await tDes1.DecryptAsync(_result1);
        string decrypted2 = await tDes2.DecryptAsync(_result2);

        // Assert
        Assert.AreEqual(_sample, decrypted1);
        Assert.AreEqual(_sample, decrypted2);
    }

    [TestMethod]
    public async Task EncryptDecryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.EncryptAsync(_testFile, _cryptFile1);
        await tDes2.EncryptAsync(_testFile, _cryptFile2);
        await tDes1.DecryptAsync(_cryptFile1, _outFile1);
        await tDes2.DecryptAsync(_cryptFile2, _outFile2);

        // Assert
        Assert.AreEqual(File.ReadAllText(_testFile), File.ReadAllText(_outFile1));
        Assert.AreEqual(File.ReadAllText(_testFile), File.ReadAllText(_outFile2));
    }

    [TestMethod]
    public async Task EncryptDecryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        using var inStream1 = new MemoryStream(Encoding.UTF8.GetBytes(_sample));
        using var inStream2 = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

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
        Assert.AreEqual(_sample, decStr1);
        Assert.AreEqual(_sample, decStr2);
        Assert.AreNotEqual(encrypted1, encrypted2);
        Assert.AreEqual(decStr1, decStr2);
    }

    [TestMethod]
    public async Task EncryptDecryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string encrypted1 = await tDes1.EncryptAsync(_sample);
        string encrypted2 = await tDes2.EncryptAsync(_sample);
        string decrypted1 = await tDes1.DecryptAsync(encrypted1);
        string decrypted2 = await tDes2.DecryptAsync(encrypted2);

        // Assert
        Assert.AreEqual(_sample, decrypted1);
        Assert.AreEqual(_sample, decrypted2);
        Assert.AreNotEqual(encrypted1, encrypted2);
        Assert.AreEqual(decrypted1, decrypted2);
    }

    [TestMethod()]
    public async Task EncryptFileTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        await tDes1.EncryptAsync(_testFile, _cryptFile1);
        await tDes2.EncryptAsync(_testFile, _cryptFile2);

        // Assert
        Assert.IsTrue(File.Exists(_cryptFile1));
        Assert.IsTrue(File.Exists(_cryptFile2));
        Assert.AreNotEqual(File.ReadAllBytes(_cryptFile1), File.ReadAllBytes(_cryptFile2));
    }

    [TestMethod()]
    public async Task EncryptStreamTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);
        using var ms1 = new MemoryStream(Encoding.UTF8.GetBytes(_sample));
        using var ms2 = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        byte[] encrypted1 = await tDes1.EncryptAsync(ms1);
        byte[] encrypted2 = await tDes2.EncryptAsync(ms2);
        string encStr1 = Convert.ToBase64String(encrypted1);
        string encStr2 = Convert.ToBase64String(encrypted2);

        // Assert
        Assert.AreEqual(_result1, encStr1);
        Assert.AreEqual(_result2, encStr2);
        Assert.AreNotEqual(encStr1, encStr2);
    }

    [TestMethod]
    public async Task EncryptTest()
    {
        // Arrange
        var tDes1 = new SimpleTripleDes(_key, TripleDesKeySize.b128);
        var tDes2 = new SimpleTripleDes(_key, TripleDesKeySize.b192);

        // Act
        string encrypted1 = await tDes1.EncryptAsync(_sample);
        string encrypted2 = await tDes2.EncryptAsync(_sample);
        //Debug.WriteLine(encrypted);

        // Assert
        Assert.AreEqual(_result1, encrypted1);
        Assert.AreEqual(_result2, encrypted2);
        Assert.AreNotEqual(encrypted1, encrypted2);
    }

    [TestInitialize]
    public void InitTests()
    {
        if (File.Exists(_cryptFile1))
            File.Delete(_cryptFile1);
        if (File.Exists(_cryptFile2))
            File.Delete(_cryptFile2);
        if (File.Exists(_outFile1))
            File.Delete(_outFile1);
        if (File.Exists(_outFile2))
            File.Delete(_outFile2);
    }
}