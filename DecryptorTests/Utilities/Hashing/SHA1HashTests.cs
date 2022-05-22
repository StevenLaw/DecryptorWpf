using Decryptor.Core.Utilities.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Hashing;

[TestClass()]
public class SHA1HashTests
{
    private const string _failHash = "be7e10d1c5dd2ad77f6d5a617372a7bf13cb7bf";
    private const string _fileHash = "873cd273cdff8922f7c3e182971ac9c87973b86a";
    private const string _filename = "Testing.txt";
    private const string _hash = "451d99c8281f579bdf1e2b0f0a2a63fd23707037";
    private const string _sample = "This is sample text";

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _fileHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new SHA1Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        bool matches = await handler.CheckHashAsync(stream, _hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new SHA1Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        bool matches = await handler.CheckHashAsync(stream, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task CheckHashFailTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        string hash = await handler.GetFileHashAsync(_filename);
        bool matches = await handler.CheckFileHashAsync(_filename, hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task GetAndCheckHashStreamTest()
    {
        // Arrange
        var handler = new SHA1Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

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
        var handler = new SHA1Hash();

        // Act
        string hash = await handler.GetHashAsync(_sample);
        bool matches = await handler.CheckHashAsync(_sample, hash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod()]
    public async Task GetFileHashAsyncTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        string hash = await handler.GetFileHashAsync(_filename);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new SHA1Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        string hash = await handler.GetHashAsync(stream);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncTest()
    {
        // Arrange
        var handler = new SHA1Hash();

        // Act
        string hash = await handler.GetHashAsync(_sample);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}