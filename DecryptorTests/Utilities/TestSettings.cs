using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Utilities.Encryption;
using System;
using System.Threading.Tasks;

namespace DecryptorTests.Utilities;

internal class TestSettings : ISettings
{
    public int Argon2DegreesOfParallelism { get; set; }
    public int Argon2HashLength { get; set; }
    public int Argon2Iterations { get; set; }
    public int Argon2MemorySize { get; set; }
    public int Argon2SaltLength { get; set; }
    public int BCryptWorkFactor { get; set; }
    public int DegreesOfParallelism { get; set; }
    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }
    public HashAlgorithm HashAlgorithm { get; set; }
    public string Key { get; set; }
    public int Pbkdf2HashSize { get; set; }
    public int Pbkdf2Iterations { get; set; }
    public string Pbkdf2Prefix { get; set; }
    public int Pbkdf2SaltSize { get; set; }
    public int ScryptBlockCount { get; set; }
    public int ScryptIterations { get; set; }
    public int ScryptThreadCount { get; set; }
    public TripleDesKeySize TripleDesKeySize { get; set; }

    public void Load()
    {
        throw new NotImplementedException();
    }

    public Task LoadAsync()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }
}