using Decryptor.Core.Utilities.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Hashing;

[TestClass()]
public class SHA256HashTests
{
    private const string _failHash = "6fe7d7112caaba1b1bc7bfa656974ac416cc525a7286117494f5b29dfec1f77";
    private const string _fileHash = "58939fa578b702c8c18d85b10c49b68fabbef1cb819da2a6ecc475a843fb4b69";
    private const string _filename = "Testing.txt";
    private const string _resultHash = "bf67a78f571d7ecad67ad2a5ea064edc969def57649c69fbb22eb72f4c56f87a";
    private const string _sample = "This is sample text";

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new SHA256Hash();

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _fileHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new SHA256Hash();

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new SHA256Hash();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sample));

        // Act
        bool matches = await handler.CheckHashAsync(stream, _resultHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncTest()
    {
        // Arrange
        var handler = new SHA256Hash();

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _resultHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new SHA256Hash();
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
        var handler = new SHA256Hash();

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new SHA256Hash();

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
        var handler = new SHA256Hash();
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
        var handler = new SHA256Hash();

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
        var handler = new SHA256Hash();

        // Act
        string hash = await handler.GetFileHashAsync(_filename);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new SHA256Hash();
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
        var handler = new SHA256Hash();

        // Act
        string hash = await handler.GetHashAsync(_sample);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}