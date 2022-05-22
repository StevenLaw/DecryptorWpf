using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;

namespace Decryptor.Core.Utilities.Hashing;

public class HashFactory : IHashFactory
{
    private readonly ISettings _settings;

    public HashFactory(ISettings settings)
    {
        _settings = settings;
    }

    public IHash Create(HashAlgorithm algorithm) =>
        algorithm switch
        {
            HashAlgorithm.None => throw new NotImplementedException(),
            HashAlgorithm.BCrypt => new BCryptHash(_settings.BCryptWorkFactor),
            HashAlgorithm.Scrypt => new ScryptHash(
                _settings.ScryptIterations,
                _settings.ScryptBlockCount,
                _settings.ScryptThreadCount
            ),
            HashAlgorithm.Argon2 => new ArgonHash(
                _settings.Argon2DegreesOfParallelism,
                _settings.Argon2Iterations,
                _settings.Argon2MemorySize,
                _settings.Argon2SaltLength,
                _settings.Argon2HashLength
            ),
            HashAlgorithm.PBKDF2 => new Pbkdf2Hash(
                _settings.Pbkdf2Iterations,
                _settings.Pbkdf2SaltSize,
                _settings.Pbkdf2HashSize,
                _settings.Pbkdf2Prefix),
            HashAlgorithm.MD5 => new MD5Hash(),
            HashAlgorithm.SHA1 => new SHA1Hash(),
            HashAlgorithm.SHA256 => new SHA256Hash(),
            HashAlgorithm.SHA512 => new SHA512Hash(),
            _ => throw new InvalidOperationException($"{algorithm} is not a valid algorithm"),
        };
}