using Decryptor.Core.Enums;
using Decryptor.Core.Utilities.Encryption;

namespace Decryptor.Core.Interfaces;

public interface ISettings
{
    int Argon2DegreesOfParallelism { get; set; }
    int Argon2HashLength { get; set; }
    int Argon2Iterations { get; set; }
    int Argon2MemorySize { get; set; }
    int Argon2SaltLength { get; set; }
    int BCryptWorkFactor { get; set; }
    int DegreesOfParallelism { get; set; }
    EncryptionAlgorithm EncryptionAlgorithm { get; set; }
    HashAlgorithm HashAlgorithm { get; set; }
    string Key { get; set; }
    int Pbkdf2HashSize { get; set; }
    int Pbkdf2Iterations { get; set; }
    string Pbkdf2Prefix { get; set; }
    int Pbkdf2SaltSize { get; set; }
    int ScryptBlockCount { get; set; }
    int ScryptIterations { get; set; }
    int ScryptThreadCount { get; set; }
    TripleDesKeySize TripleDesKeySize { get; set; }

    void Load();

    Task LoadAsync();

    void Save();

    Task SaveAsync();
}