using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Encryption.Tests;

[TestClass()]
public class SimpleDesTests
{
    private readonly SecureString _key;
    private const string INSECURE_KEY = "This is an insecure key for testing";
    private const string SAMPLE = "This is a sample";
    private const string RESULT = "yMGP0assV+WeqXFbQxfXaEG+L3ASlsNv";
    private const string TEST_FILE = "Testing.txt";
    private const string CRYPT_FILE = "Testing.txt.des";
    private const string TEST_CRYPT_FILE = "Crypt.txt.des";
    private const string OUT_FILE = "Out.txt";

    public SimpleDesTests()
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

    [TestMethod]
    public async Task EncryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string encrypted = await des.EncryptAsync(SAMPLE);
        //Debug.WriteLine(encrypted);

        // Assert
        Assert.AreEqual(RESULT, encrypted);
    }

    [TestMethod()]
    public async Task EncryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        byte[] encrypted = await des.EncryptAsync(ms);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(RESULT, encStr);
    }

    [TestMethod()]
    public async Task EncryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.EncryptAsync(TEST_FILE, CRYPT_FILE);

        // Assert
        Assert.IsTrue(File.Exists(CRYPT_FILE));
    }

    [TestMethod]
    public async Task DecryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string decrypted = await des.DecryptAsync(RESULT);

        // Assert
        Assert.AreEqual(SAMPLE, decrypted);
    }

    [TestMethod()]
    public async Task DecryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        var bytes = Convert.FromBase64String(RESULT);
        using var ms = new MemoryStream(bytes);

        // Act
        byte[] decrypted = await des.DecryptAsync(ms);
        var decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(SAMPLE, decStr);
    }

    [TestMethod()]
    public async Task DecryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.DecryptAsync(TEST_CRYPT_FILE, OUT_FILE);

        // Assert
        Assert.IsTrue(File.Exists(OUT_FILE));
    }

    [TestMethod]
    public async Task EncryptDecryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string encrypted = await des.EncryptAsync(SAMPLE);
        string decrypted = await des.DecryptAsync(encrypted);

        // Assert
        Assert.AreEqual(SAMPLE, decrypted);
    }

    [TestMethod]
    public async Task EncryptDecryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        using var inStream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        byte[] encrypted = await des.EncryptAsync(inStream);
        using var outStream = new MemoryStream(encrypted);
        byte[] decrypted = await des.DecryptAsync(outStream);
        string decStr = Encoding.UTF8.GetString(decrypted);

        // Assert
        Assert.AreEqual(SAMPLE, decStr);
    }

    [TestMethod]
    public async Task EncryptDecryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.EncryptAsync(TEST_FILE, CRYPT_FILE);
        await des.DecryptAsync(CRYPT_FILE, OUT_FILE);

        // Assert
        Assert.AreEqual(File.ReadAllText(TEST_FILE), File.ReadAllText(OUT_FILE));
    }

    [TestMethod]
    public async Task DecryptEncryptTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        string decrypted = await des.DecryptAsync(RESULT);
        string encrypted = await des.EncryptAsync(decrypted);

        // Assert
        Assert.AreEqual(RESULT, encrypted);
    }

    [TestMethod]
    public async Task DecryptEncryptStreamTest()
    {
        // Arrange
        var des = new SimpleDes(_key);
        using var inStream = new MemoryStream(Convert.FromBase64String(RESULT));

        // Act
        byte[] decrypted = await des.DecryptAsync(inStream);
        using var outStream = new MemoryStream(decrypted);
        byte[] encrypted = await des.EncryptAsync(outStream);
        string encStr = Convert.ToBase64String(encrypted);

        // Assert
        Assert.AreEqual(RESULT, encStr);
    }

    [TestMethod]
    public async Task DecryptEncryptFileTest()
    {
        // Arrange
        var des = new SimpleDes(_key);

        // Act
        await des.DecryptAsync(TEST_CRYPT_FILE, OUT_FILE);
        await des.EncryptAsync(OUT_FILE, CRYPT_FILE);

        // Assert
        Assert.AreEqual(File.ReadAllText(TEST_CRYPT_FILE), File.ReadAllText(CRYPT_FILE));
    }
}