using Decryptor.Core.Utilities.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities.Hashing;

[TestClass()]
public class ArgonHashTests
{
    private const int _argonIterations = 4;
    private const int _degrees = 8;
    private const string _failHash = "$a2$8$4$1048576$16$16$rasN2h23lI2QHIo0ATikPQ==$l6ATQJOCSRUyoH2Q8AVbDw==";
    private const string _fileHash = "$a2$8$4$1048576$16$16$bkdfGTkX/0lwNXBAGC2wYw==$VZ9pgywz7sCDfoiHxlt+Pw==";
    private const string _filename = "Testing.txt";
    private const int _hashLength = 16;
    private const int _memorySpace = 1048576;
    private const string _resultHash = "$a2$8$4$1048576$16$16$/X7A9uODAMLIU/lAIKlZ2A==$1QUD0xdIdp9cMqXxOvO6bw==";
    private const int _saltLength = 16;
    private const string _sample = "This is sample text";

    [TestMethod()]
    public async Task CheckFileHashAsyncTest()
    {
        // Arrange
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _fileHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckFileHashFailTest()
    {
        // Arrange
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

        // Act
        bool matches = await handler.CheckFileHashAsync(_filename, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod()]
    public async Task CheckHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);
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
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _resultHash);

        // Assert
        Assert.IsTrue(matches);
    }

    [TestMethod]
    public async Task CheckHashFailStreamTest()
    {
        // Arrange
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);
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
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

        // Act
        bool matches = await handler.CheckHashAsync(_sample, _failHash);

        // Assert
        Assert.IsFalse(matches);
    }

    [TestMethod]
    public async Task GetAndCheckFileHashTest()
    {
        // Arrange
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

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
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);
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
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

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
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

        // Act
        string hash = await handler.GetFileHashAsync(_filename);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }

    [TestMethod()]
    public async Task GetHashAsyncStreamTest()
    {
        // Arrange
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);
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
        var handler = new ArgonHash(_degrees, _argonIterations, _memorySpace, _saltLength, _hashLength);

        // Act
        string hash = await handler.GetHashAsync(_sample);

        // Assert
        Assert.IsTrue(hash.Length > 0);
    }
}