// Ignore Spelling: Pbkdf

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing.Tests;

[TestClass()]
public class Pbkdf2HashTests
{
    private const string FAIL_HASH = "$TEST$V1$10000$k3AF2eDUs/P7eTHWP1UPbZ8DEeROYxnAemGWlScnuxU3dq0S";
    private const string FILE_HASH = "$TEST$V1$10000$r/FrbrwsQxTV/2oVG7hr+WtKkX3FJJ/lgLwLx8hjuCynNeB9";
    private const string FILENAME = "Testing.txt";
    private const string RESULT_HASH = "$TEST$V1$10000$vb+lw9p6POPFCFqAKq3yjaY8RTldUJF/ANFLxYePJEfjSWYl";
    private const string SAMPLE = "This is sample text";
    private const string PREFIX = "$TEST$V1$";
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 20;
    private const int ITERATIONS = 10000;


    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FILE_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

        // Act
        bool matches = await handler.CheckFileHashAsync(FILENAME, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);
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
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, RESULT_HASH);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);
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
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

        // Act
        bool matches = await handler.CheckHashAsync(SAMPLE, FAIL_HASH);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

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
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);
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
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

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
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

        // Act
        string hash = await handler.GetFileHashAsync(FILENAME);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);
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
        var handler = new Pbkdf2Hash(ITERATIONS, SALT_SIZE, HASH_SIZE, PREFIX);

        // Act
        string hash = await handler.GetHashAsync(SAMPLE);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}