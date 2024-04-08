using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests;

[TestClass()]
public class ArgonHashTests
{
    private const int ARGON_ITERATIONS = 4;
    private const int DEGREES = 8;
    private const string FAIL_HASH = "$a2$8$4$1048576$16$16$rasN2h23lI2QHIo0ATikPQ==$l6ATQJOCSRUyoH2Q8AVbDw==";
    private const string FILE_HASH = "$a2$8$4$1048576$16$16$bkdfGTkX/0lwNXBAGC2wYw==$VZ9pgywz7sCDfoiHxlt+Pw==";
    private const string FILENAME = "Testing.txt";
    private const int HASH_LENGTH = 16;
    private const int MEMORY_SPACE = 1048576;
    private const string RESULT_HASH = "$a2$8$4$1048576$16$16$/X7A9uODAMLIU/lAIKlZ2A==$1QUD0xdIdp9cMqXxOvO6bw==";
    private const int SALT_LENGTH = 16;
    private const string SAMPLE = "This is sample text";

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FILE_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);
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
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, RESULT_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);
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
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

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
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);
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
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

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
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

        // Act
        string hash = await handler.GetFileHashAsync(FILENAME);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);
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
        var handler = new ArgonHash(DEGREES, ARGON_ITERATIONS, MEMORY_SPACE, SALT_LENGTH, HASH_LENGTH);

        // Act
        string hash = await handler.GetHashAsync(SAMPLE);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}