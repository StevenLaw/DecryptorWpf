using Decryptor.Core.Utilities.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Hashing;

[TestClass()]
public class ScryptHashTests
{
    private const int _blockSize = 8;
    private const string _failHash = "$s2$16384$8$1$MFPbkrwEu7NEK6NjJYPa0XU7tTHQm95WlpoGV8ro0SE=$+xALFipj8Rr+Its/xRcsNx2ZM5rJovAs+NgTDEG2iJg=";
    private const string _fileHash = "$s2$16384$8$1$0dJgCE+9gArMgPWwakcWW4V+yjmKfT58S6fR9Hpg5VA=$er5aULaVNnDphkbf0WxOirK9LrTLiZ6m/o+MIBFi7VE=";
    private const string _filename = "Testing.txt";
    private const string _resultHash = "$s2$16384$8$1$PNqOiuvttsZIc8aXI9HoC6qeHBk7wLeJgbf2VihYBwY=$9Zeb3/g+xb3Pc2ypxysBiUwKQ6SRQ/yqoy4xw/Qhp4g=";
    private const string _sample = "This is sample text";
    private const int _scryptIterations = 16384;
    private const int _threadCount = 1;

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _fileHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);
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
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _resultHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);
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
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

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
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);
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
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

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
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

        // Act
        string hash = await handler.GetFileHashAsync(_filename);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);
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
        var handler = new ScryptHash(_scryptIterations, _blockSize, _threadCount);

        // Act
        string hash = await handler.GetHashAsync(_sample);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}