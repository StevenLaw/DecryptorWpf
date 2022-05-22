using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Utilities.Hashing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DecryptorTests.Utilities.Hashing;

[TestClass()]
public class HashFactoryTests
{
    public HashFactoryTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISettings, TestSettings>();
        services.AddSingleton<IHashFactory, HashFactory>();

        var serviceProvider = services.BuildServiceProvider();

        Settings = serviceProvider.GetService<ISettings>();
        HashFactory = serviceProvider.GetService<IHashFactory>();
    }

    public IHashFactory HashFactory { get; set; }
    public ISettings Settings { get; set; }

    [TestMethod()]
    public void CreateArgon2Test()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.Argon2), typeof(ArgonHash));
    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateBadTest()
    {
        HashFactory.Create((HashAlgorithm)100);
    }

    [TestMethod()]
    public void CreateBCryptTest()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.BCrypt), typeof(BCryptHash));
    }

    [TestMethod()]
    public void CreateMD5Test()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.MD5), typeof(MD5Hash));
    }

    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void CreateNoneTest()
    {
        HashFactory.Create(HashAlgorithm.None);
    }

    [TestMethod()]
    public void CreateScryptTest()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.Scrypt), typeof(ScryptHash));
    }

    [TestMethod()]
    public void CreateSHA1Test()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.SHA1), typeof(SHA1Hash));
    }

    [TestMethod()]
    public void CreateSHA256Test()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.SHA256), typeof(SHA256Hash));
    }

    [TestMethod()]
    public void CreateSHA512Test()
    {
        Assert.IsInstanceOfType(HashFactory.Create(HashAlgorithm.SHA512), typeof(SHA512Hash));
    }
}