using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Security;

namespace Decryptor.Utilities.Tests;

[TestClass()]
public class PasswordProtectorTests
{
    private const string SAMPLE = "This is a sample string to encrypt/secure";
    private readonly SecureString _secureSample;

    public PasswordProtectorTests()
    {
        _secureSample = new SecureString();
        foreach (char c in SAMPLE)
        {
            _secureSample.AppendChar(c);
        }
        _secureSample.MakeReadOnly();
    }

    [TestMethod()]
    public void EncryptThenDecryptStringTest()
    {
        // Act
        string encrypted = PasswordProtector.GetEncryptedString(_secureSample);
        var decrypted = PasswordProtector.DecryptString(encrypted);
        Debug.WriteLine(encrypted);

        // Assert
        Assert.AreEqual(_secureSample.Length, decrypted.Length);
        Assert.AreEqual(_secureSample.ToInsecureString(), decrypted.ToInsecureString());
    }

    [TestMethod()]
    public void ToInsecureStringTest()
    {
        // Act
        string insecure = _secureSample.ToInsecureString();

        // Assert
        Assert.AreEqual(_secureSample.Length, insecure.Length);
    }

    [TestMethod()]
    public void ToSecureStringTest()
    {
        // Act
        var secure = SAMPLE.ToSecureString();

        // Assert
        Assert.AreEqual(SAMPLE.Length, secure.Length);
    }
}