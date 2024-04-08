using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests;

[TestClass()]
public class SimpleAesTests
{
    private readonly SecureString _key;
    private const string INSECURE_KEY = "This is an insecure key for testing";
    private const string SAMPLE = "This is a sample for encrypting";
    private const string RESULT = "/u7rOUO9GCXJGN269dag0QU26yR4WJkLPvcLjcWoxYc=";
    private const string TEST_FILE = "Testing.txt";
    private const string CRYPT_FILE = "Testing.txt.aes";
    private const string TEST_CRYPT_FILE = "Crypt.txt.aes";
    private const string OUT_FILE = "Out.txt";

    public SimpleAesTests()
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
        if (File.Exists(CRYPT_FILE))
            File.Delete(CRYPT_FILE);
        if (File.Exists(OUT_FILE))
            File.Delete(OUT_FILE);
    }

    [TestMethod()]
    public async Task EncryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string encrypted = await aes.EncryptAsync(SAMPLE);

        // Assert
        Assert.AreEqual(RESULT, encrypted);
    }

    [TestMethod()]
    public async Task EncryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        byte[] encrypted = await aes.EncryptAsync(ms);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(RESULT, encStr);
    }

    [TestMethod()]
    public async Task EncryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.EncryptAsync(TEST_FILE, CRYPT_FILE);

        // Assert
        Assert.IsTrue(File.Exists(CRYPT_FILE));
    }

    [TestMethod()]
    public async Task DecryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string decrypted = await aes.DecryptAsync(RESULT);

        // Assert
        Assert.AreEqual(SAMPLE, decrypted);
    }

    [TestMethod()]
    public async Task DecryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        var bytes = Convert.FromBase64String(RESULT);
        using var ms = new MemoryStream(bytes);

        // Act
        byte[] decrypted = await aes.DecryptAsync(ms);
        var decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(SAMPLE, decStr);
    }

    [TestMethod()]
    public async Task DecryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.DecryptAsync(TEST_CRYPT_FILE, OUT_FILE);

        // Assert
        Assert.IsTrue(File.Exists(OUT_FILE));
    }

    [TestMethod]
    public async Task EncryptDecryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string encrypted = await aes.EncryptAsync(SAMPLE);
        string decrypted = await aes.DecryptAsync(encrypted);

        // Assert
        Assert.AreEqual(SAMPLE, decrypted);
    }

    [TestMethod]
    public async Task EncryptDecryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        using var inStream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        byte[] encrypted = await aes.EncryptAsync(inStream);
        using var outStream = new MemoryStream(encrypted);
        byte[] decrypted = await aes.DecryptAsync(outStream);
        string decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(SAMPLE, decStr);
    }

    [TestMethod]
    public async Task EncryptDecryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.EncryptAsync(TEST_FILE, CRYPT_FILE);
        await aes.DecryptAsync(CRYPT_FILE, OUT_FILE);

        // Assert
        Assert.AreEqual(File.ReadAllText(TEST_FILE), File.ReadAllText(OUT_FILE));
    }

    [TestMethod]
    public async Task DecryptEncryptTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        string decrypted = await aes.DecryptAsync(RESULT);
        string encrypted = await aes.EncryptAsync(decrypted);

        // Assert
        Assert.AreEqual(RESULT, encrypted);
    }

    [TestMethod]
    public async Task DecryptEncryptStreamTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);
        using var inStream = new MemoryStream(Convert.FromBase64String(RESULT));

        // Act
        byte[] decrypted = await aes.DecryptAsync(inStream);
        using var outStream = new MemoryStream(decrypted);
        byte[] encrypted = await aes.EncryptAsync(outStream);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(RESULT, encStr);
    }

    [TestMethod]
    public async Task DecryptEncryptFileTest()
    {
        // Arrange
        var aes = new SimpleAes(_key);

        // Act
        await aes.DecryptAsync(TEST_CRYPT_FILE, OUT_FILE);
        await aes.EncryptAsync(OUT_FILE, CRYPT_FILE);

        // Assert
        Assert.AreEqual(File.ReadAllText(TEST_CRYPT_FILE), File.ReadAllText(CRYPT_FILE));
    }
}