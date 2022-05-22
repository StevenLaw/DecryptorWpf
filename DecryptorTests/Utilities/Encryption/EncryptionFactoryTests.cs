using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Utilities.Encryption;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DecryptorTests.Utilities.Encryption;

[TestClass()]
public class EncryptionFactoryTests
{
    public EncryptionFactoryTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISettings, TestSettings>();
        services.AddSingleton<IEncryptionFactory, EncryptionFactory>();

        var serviceProvider = services.BuildServiceProvider();

        TestSettings = serviceProvider.GetService<ISettings>();
        EncryptionFactory = serviceProvider.GetService<IEncryptionFactory>();
    }

    public IEncryptionFactory EncryptionFactory { get; set; }
    public ISettings TestSettings { get; set; }

    [TestMethod()]
    public void CreateAesTest()
    {
        Assert.IsInstanceOfType(EncryptionFactory.Create(EncryptionAlgorithm.AES), typeof(SimpleAes));
    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateBadTest()
    {
        EncryptionFactory.Create((EncryptionAlgorithm)100);
    }

    [TestMethod()]
    public void CreateDesTest()
    {
        Assert.IsInstanceOfType(EncryptionFactory.Create(EncryptionAlgorithm.DES), typeof(SimpleDes));
    }

    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void CreateNoneTest()
    {
        EncryptionFactory.Create(EncryptionAlgorithm.None);
    }

    [TestMethod]
    public void CreateTripleDesTest()
    {
        var s3Des1 = EncryptionFactory.Create(EncryptionAlgorithm.TripleDES);
        TestSettings.TripleDesKeySize = TripleDesKeySize.b128;
        var s3Des2 = EncryptionFactory.Create(EncryptionAlgorithm.TripleDES);
        TestSettings.TripleDesKeySize = TripleDesKeySize.b192;
        var s3Des3 = EncryptionFactory.Create(EncryptionAlgorithm.TripleDES);
        Assert.IsInstanceOfType(s3Des1, typeof(SimpleTripleDes));
        Assert.IsInstanceOfType(s3Des2, typeof(SimpleTripleDes));
        Assert.IsInstanceOfType(s3Des3, typeof(SimpleTripleDes));
        Assert.AreEqual((s3Des1 as SimpleTripleDes).KeySizeMode, TripleDesKeySize.b128);
        Assert.AreEqual((s3Des2 as SimpleTripleDes).KeySizeMode, TripleDesKeySize.b128);
        Assert.AreEqual((s3Des3 as SimpleTripleDes).KeySizeMode, TripleDesKeySize.b192);
    }
}