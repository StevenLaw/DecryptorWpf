using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests;

[TestClass()]
public class SHA512HashTests
{
    private const string FAIL_HASH = "c85fb94f63c5b9f93e495fb57589656d31b4411ec793a11fde64039fdaa2da859d14d41672414864691637f8265072f64e238d7ce277437927ca7ccbfa869c";
    private const string FILE_HASH = "d35f04e14d00621a1cb0649a25b69644e7ac569fcd244bfd51e87e1d0570213e9a3d411f6441544d42c44cc81be9ac2e437314718f22b4d67eaa5671afdd2166";
    private const string FILENAME = "Testing.txt";
    private const string RESULT_HASH = "cce3233e201810a61541ecc0e2a69bda7de751d80f35c2fcd70b19bd0612543ae18885a1d7d32f3c460cc5cab91268a93b3d04431d3b14d6eee1e44954a4513b";
    private const string SAMPLE = "This is sample text";

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FILE_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new SHA512Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        bool matches = await handler.CheckHashAsync(stream, RESULT_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, RESULT_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new SHA512Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        bool matches = await handler.CheckHashAsync(stream, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task CheckHashFailTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        string hash = await handler.GetFileHashAsync(FILENAME);
        bool matches = await handler.CheckFileHashAsync(FILENAME, hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task GetAndCheckHashStreamTest()
    {
        // Arrange
        var handler = new SHA512Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        string hash = await handler.GetHashAsync(stream);
        stream.Position = 0;
        bool matches = await handler.CheckHashAsync(stream, hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task GetAndCheckHashTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        string hash = await handler.GetHashAsync(SAMPLE);
        bool matches = await handler.CheckHashAsync(SAMPLE, hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod()]
    public async Task GetFileHashAsyncTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        string hash = await handler.GetFileHashAsync(FILENAME);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new SHA512Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));

        // Act
        string hash = await handler.GetHashAsync(stream);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncTest()
    {
        // Arrange
        var handler = new SHA512Hash();

        // Act
        string hash = await handler.GetHashAsync(SAMPLE);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}