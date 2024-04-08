// Ignore Spelling: Scrypt

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests;

[TestClass()]
public class ScryptHashTests
{
    private const int BLOCK_SIZE = 8;
    private const string FAIL_HASH = "$s2$16384$8$1$MFPbkrwEu7NEK6NjJYPa0XU7tTHQm95WlpoGV8ro0SE=$+xALFipj8Rr+Its/xRcsNx2ZM5rJovAs+NgTDEG2iJg=";
    private const string FILE_HASH = "$s2$16384$8$1$0dJgCE+9gArMgPWwakcWW4V+yjmKfT58S6fR9Hpg5VA=$er5aULaVNnDphkbf0WxOirK9LrTLiZ6m/o+MIBFi7VE=";
    private const string FILENAME = "Testing.txt";
    private const string RESULT_HASH = "$s2$16384$8$1$PNqOiuvttsZIc8aXI9HoC6qeHBk7wLeJgbf2VihYBwY=$9Zeb3/g+xb3Pc2ypxysBiUwKQ6SRQ/yqoy4xw/Qhp4g=";
    private const string SAMPLE = "This is sample text";
    private const int SCRYPT_ITERATIONS = 16384;
    private const int THREAD_COUNT = 1;

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FILE_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);
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
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, RESULT_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);
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
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

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
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);
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
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

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
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

        // Act
        string hash = await handler.GetFileHashAsync(FILENAME);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);
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
        var handler = new ScryptHash(SCRYPT_ITERATIONS, BLOCK_SIZE, THREAD_COUNT);

        // Act
        string hash = await handler.GetHashAsync(SAMPLE);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}