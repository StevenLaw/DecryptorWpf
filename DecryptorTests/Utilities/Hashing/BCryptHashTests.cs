using Decryptor.Core.Utilities.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Hashing;

[TestClass()]
public class BCryptHashTests
{
    private const string _failHash = "$2a$12$mnoK5//Ad5pFy1XnpLY1aeaN43B/7AtPaTIlbQujVNzY6x09P.ceG";
    private const string _fileHash = "$2a$12$.V7xrlvJETTocuQjqGAN6O2Fqz.3CoOsepgSRCPFUB538yqrUsHKG";
    private const string _filename = "Testing.txt";
    private const string _resultHash = "$2a$12$0W3HDzb4kSDKs4dUagmEzeVzFVQfR.IQga3NJn5lK5jwdeRUI.jZO";
    private const string _sample = "This is sample text";
    private const int _workFactor = 12;

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new BCryptHash(_workFactor);

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _fileHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new BCryptHash(_workFactor);

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new BCryptHash(_workFactor);
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
        var handler = new BCryptHash(_workFactor);

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _resultHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new BCryptHash(_workFactor);
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
        var handler = new BCryptHash(_workFactor);

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new BCryptHash(_workFactor);

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
        var handler = new BCryptHash(_workFactor);
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
        var handler = new BCryptHash(_workFactor);

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
        var handler = new BCryptHash(_workFactor);

        // Act
        string hash = await handler.GetFileHashAsync(_filename);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new BCryptHash(_workFactor);
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
        var handler = new BCryptHash(_workFactor);

        // Act
        string hash = await handler.GetHashAsync(_sample);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}