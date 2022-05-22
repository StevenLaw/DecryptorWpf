using Decryptor.Core.Utilities.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Encryption;

[TestClass()]
public class SimpleDesTests
{
    private const string _cryptFile = "Testing.txt.des";
    private const string _insecureKey = "This is an insecure key for testing";
    private const string _outFile = "Out.txt";
    private const string _result = "yMGP0assV+WeqXFbQxfXaEG+L3ASlsNv";
    private const string _sample = "This is a sample";
    private const string _testCryptFile = "Crypt.txt.des";
    private const string _testFile = "Testing.txt";
    private readonly SecureString _key;

    public SimpleDesTests()
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
        var des = new SimpleDes(_key);

        // Act
        await des.DecryptAsync(_testCryptFile, _outFile);
        await des.EncryptAsync(_outFile, _cryptFile);

        // Assert
        Assert.AreEqual(File.ReadAllText(_testCryptFile), File.ReadAllText(_cryptFile));
    }

    [TestMethod]
    public async Task DecryptEncryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        using var inStream = new MemoryStream(Convert.FromBase64String(_result));

        // Act
        byte[] decrypted = await des.DecryptAsync(inStream);
        using var outStream = new MemoryStream(decrypted);
        byte[] encrypted = await des.EncryptAsync(outStream);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(_result, encStr);
    }

    [TestMethod]
    public async Task DecryptEncryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string decrypted = await des.DecryptAsync(_result);
        string encrypted = await des.EncryptAsync(decrypted);

        // Assert
        Assert.AreEqual(_result, encrypted);
    }

    [TestMethod()]
    public async Task DecryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.DecryptAsync(_testCryptFile, _outFile);

        // Assert
        Assert.IsTrue(File.Exists(_outFile));
    }

    [TestMethod()]
    public async Task DecryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        var bytes = Convert.FromBase64String(_result);
        using var ms = new MemoryStream(bytes);

        // Act
        byte[] decrypted = await des.DecryptAsync(ms);
        var decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(_sample, decStr);
    }

    [TestMethod]
    public async Task DecryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string decrypted = await des.DecryptAsync(_result);

        // Assert
        Assert.AreEqual(_sample, decrypted);
    }

    [TestMethod]
    public async Task EncryptDecryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.EncryptAsync(_testFile, _cryptFile);
        await des.DecryptAsync(_cryptFile, _outFile);

        // Assert
        Assert.AreEqual(File.ReadAllText(_testFile), File.ReadAllText(_outFile));
    }

    [TestMethod]
    public async Task EncryptDecryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        using var inStream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        byte[] encrypted = await des.EncryptAsync(inStream);
        using var outStream = new MemoryStream(encrypted);
        byte[] decrypted = await des.DecryptAsync(outStream);
        string decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(_sample, decStr);
    }

    [TestMethod]
    public async Task EncryptDecryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string encrypted = await des.EncryptAsync(_sample);
        string decrypted = await des.DecryptAsync(encrypted);

        // Assert
        Assert.AreEqual(_sample, decrypted);
    }

    [TestMethod()]
    public async Task EncryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.EncryptAsync(_testFile, _cryptFile);

        // Assert
        Assert.IsTrue(File.Exists(_cryptFile));
    }

    [TestMethod()]
    public async Task EncryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        byte[] encrypted = await des.EncryptAsync(ms);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(_result, encStr);
    }

    [TestMethod]
    public async Task EncryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string encrypted = await des.EncryptAsync(_sample);
        //Debug.WriteLine(encrypted);

        // Assert
        Assert.AreEqual(_result, encrypted);
    }

    [TestInitialize]
    public void InitTests()
    {
        if (File.Exists(_cryptFile))
            File.Delete(_cryptFile);
        if (File.Exists(_outFile))
            File.Delete(_outFile);
    }
}