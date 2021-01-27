using Decryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public static class HashFactory
    {
        public static IHash Create(HashAlgorithm algorithm) =>
            algorithm switch
            {
                HashAlgorithm.None => throw new NotImplementedException(),
                HashAlgorithm.BCrypt => new BCryptHash(Properties.Settings.Default.BCryptWorkFactor),
                HashAlgorithm.Scrypt => new ScryptHash(
                    Properties.Settings.Default.ScryptIterationCount,
                    Properties.Settings.Default.ScryptBlockCount,
                    Properties.Settings.Default.ScryptThreadCount
                ),
                HashAlgorithm.Argon2 => new ArgonHash(
                    Properties.Settings.Default.Argon2DegreesOfParallelism,
                    Properties.Settings.Default.Argon2Iterations,
                    Properties.Settings.Default.Argon2MemorySize,
                    Properties.Settings.Default.Argon2SaltLength,
                    Properties.Settings.Default.Argon2HashLength
                ),
                HashAlgorithm.MD5 => new MD5Hash(),
                HashAlgorithm.SHA1 => new SHA1Hash(),
                HashAlgorithm.SHA256 => new SHA256Hash(),
                HashAlgorithm.SHA512 => new SHA512Hash(),
                _ => throw new InvalidOperationException($"{algorithm} is not a valid algorithm"),
            };
    }
}
