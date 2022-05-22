using Decryptor.Core.Utilities.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Encryption;

[TestClass()]
public class SimpleAesTests
{
    private const string _cryptFile = "Testing.txt.aes";
    private const string _insecureKey = "This is an insecure key for testing";
    private const string _outFile = "Out.txt";
    private const string _result = "/u7rOUO9GCXJGN269dag0QU26yR4WJkLPvcLjcWoxYc=";
    private const string _sample = "This is a sample for encrypting";
    private const string _testCryptFile = "Crypt.txt.aes";
    private const string _testFile = "Testing.txt";
    private readonly SecureString _key;

    public SimpleAesTests()
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
        var aes = new SimpleAes(_key);

        // Act
        await aes.DecryptAsync(_testCryptFile, _outFile);
        await aes.EncryptAsync(_outFile, _cryptFile);

        // Assert
        Assert.AreEqual(File.ReadAllText(_testCryptFile), File.ReadAllText(_cryptFile));
    }

    [TestMethod]
    public async Task DecryptEncryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        using var inStream = new MemoryStream(Convert.FromBase64String(_result));

        // Act
        byte[] decrypted = await aes.DecryptAsync(inStream);
        using var outStream = new MemoryStream(decrypted);
        byte[] encrypted = await aes.EncryptAsync(outStream);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(_result, encStr);
    }

    [TestMethod]
    public async Task DecryptEncryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string decrypted = await aes.DecryptAsync(_result);
        string encrypted = await aes.EncryptAsync(decrypted);

        // Assert
        Assert.AreEqual(_result, encrypted);
    }

    [TestMethod()]
    public async Task DecryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.DecryptAsync(_testCryptFile, _outFile);

        // Assert
        Assert.IsTrue(File.Exists(_outFile));
    }

    [TestMethod()]
    public async Task DecryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        var bytes = Convert.FromBase64String(_result);
        using var ms = new MemoryStream(bytes);

        // Act
        byte[] decrypted = await aes.DecryptAsync(ms);
        var decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(_sample, decStr);
    }

    [TestMethod()]
    public async Task DecryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string decrypted = await aes.DecryptAsync(_result);

        // Assert
        Assert.AreEqual(_sample, decrypted);
    }

    [TestMethod]
    public async Task EncryptDecryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.EncryptAsync(_testFile, _cryptFile);
        await aes.DecryptAsync(_cryptFile, _outFile);

        // Assert
        Assert.AreEqual(File.ReadAllText(_testFile), File.ReadAllText(_outFile));
    }

    [TestMethod]
    public async Task EncryptDecryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        using var inStream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        byte[] encrypted = await aes.EncryptAsync(inStream);
        using var outStream = new MemoryStream(encrypted);
        byte[] decrypted = await aes.DecryptAsync(outStream);
        string decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(_sample, decStr);
    }

    [TestMethod]
    public async Task EncryptDecryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string encrypted = await aes.EncryptAsync(_sample);
        string decrypted = await aes.DecryptAsync(encrypted);

        // Assert
        Assert.AreEqual(_sample, decrypted);
    }

    [TestMethod()]
    public async Task EncryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.EncryptAsync(_testFile, _cryptFile);

        // Assert
        Assert.IsTrue(File.Exists(_cryptFile));
    }

    [TestMethod()]
    public async Task EncryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        byte[] encrypted = await aes.EncryptAsync(ms);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(_result, encStr);
    }

    [TestMethod()]
    public async Task EncryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string encrypted = await aes.EncryptAsync(_sample);

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