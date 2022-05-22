using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Utilities.Encryption;
using System.Threading.Tasks;

namespace Decryptor.Utilities
{
    internal class Settings : ISettings
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
            Key = Properties.Settings.Default.Key;
            BCryptWorkFactor = Properties.Settings.Default.BCryptWorkFactor;
            HashAlgorithm = (HashAlgorithm)Properties.Settings.Default.HashAlgorithm;
            EncryptionAlgorithm = (EncryptionAlgorithm)Properties.Settings.Default.EncryptionAlgorithm;
            ScryptIterations = Properties.Settings.Default.ScryptIterationCount;
            ScryptBlockCount = Properties.Settings.Default.ScryptBlockCount;
            ScryptThreadCount = Properties.Settings.Default.ScryptThreadCount;
            DegreesOfParallelism = Properties.Settings.Default.Argon2DegreesOfParallelism;
            Argon2Iterations = Properties.Settings.Default.Argon2Iterations;
            Argon2MemorySize = Properties.Settings.Default.Argon2MemorySize;
            Argon2SaltLength = Properties.Settings.Default.Argon2SaltLength;
            Argon2HashLength = Properties.Settings.Default.Argon2HashLength;
            TripleDesKeySize = (TripleDesKeySize)Properties.Settings.Default.TripleDesKeySize;
            Pbkdf2HashSize = Properties.Settings.Default.Pbkdf2HashSize;
            Pbkdf2Iterations = Properties.Settings.Default.Pbkdf2Iterations;
            Pbkdf2Prefix = Properties.Settings.Default.Pbkdf2Prefix;
            Pbkdf2SaltSize = Properties.Settings.Default.Pbkdf2SaltSize;
        }

        public Task LoadAsync()
        {
            return Task.Run(() =>
            {
                Key = Properties.Settings.Default.Key;
                BCryptWorkFactor = Properties.Settings.Default.BCryptWorkFactor;
                HashAlgorithm = (HashAlgorithm)Properties.Settings.Default.HashAlgorithm;
                EncryptionAlgorithm = (EncryptionAlgorithm)Properties.Settings.Default.EncryptionAlgorithm;
                ScryptIterations = Properties.Settings.Default.ScryptIterationCount;
                ScryptBlockCount = Properties.Settings.Default.ScryptBlockCount;
                ScryptThreadCount = Properties.Settings.Default.ScryptThreadCount;
                DegreesOfParallelism = Properties.Settings.Default.Argon2DegreesOfParallelism;
                Argon2Iterations = Properties.Settings.Default.Argon2Iterations;
                Argon2MemorySize = Properties.Settings.Default.Argon2MemorySize;
                Argon2SaltLength = Properties.Settings.Default.Argon2SaltLength;
                Argon2HashLength = Properties.Settings.Default.Argon2HashLength;
                TripleDesKeySize = (TripleDesKeySize)Properties.Settings.Default.TripleDesKeySize;
                Pbkdf2HashSize = Properties.Settings.Default.Pbkdf2HashSize;
                Pbkdf2Iterations = Properties.Settings.Default.Pbkdf2Iterations;
                Pbkdf2Prefix = Properties.Settings.Default.Pbkdf2Prefix;
                Pbkdf2SaltSize = Properties.Settings.Default.Pbkdf2SaltSize;
            });
        }

        public void Save()
        {
            Properties.Settings.Default.Key = Key;
            Properties.Settings.Default.BCryptWorkFactor = BCryptWorkFactor;
            Properties.Settings.Default.HashAlgorithm = (byte)HashAlgorithm;
            Properties.Settings.Default.ScryptIterationCount = ScryptIterations;
            Properties.Settings.Default.ScryptBlockCount = ScryptBlockCount;
            Properties.Settings.Default.ScryptThreadCount = ScryptThreadCount;
            Properties.Settings.Default.Argon2DegreesOfParallelism = DegreesOfParallelism;
            Properties.Settings.Default.Argon2Iterations = Argon2Iterations;
            Properties.Settings.Default.Argon2MemorySize = Argon2MemorySize;
            Properties.Settings.Default.Argon2SaltLength = Argon2SaltLength;
            Properties.Settings.Default.Argon2HashLength = Argon2HashLength;
            Properties.Settings.Default.TripleDesKeySize = (byte)TripleDesKeySize;
            Properties.Settings.Default.Pbkdf2HashSize = Pbkdf2HashSize;
            Properties.Settings.Default.Pbkdf2Iterations = Pbkdf2Iterations;
            Properties.Settings.Default.Pbkdf2Prefix = Pbkdf2Prefix;
            Properties.Settings.Default.Pbkdf2SaltSize = Pbkdf2SaltSize;
            Properties.Settings.Default.Save();
        }

        public Task SaveAsync()
        {
            return Task.Run(() =>
            {
                Properties.Settings.Default.Key = Key;
                Properties.Settings.Default.BCryptWorkFactor = BCryptWorkFactor;
                Properties.Settings.Default.HashAlgorithm = (byte)HashAlgorithm;
                Properties.Settings.Default.ScryptIterationCount = ScryptIterations;
                Properties.Settings.Default.ScryptBlockCount = ScryptBlockCount;
                Properties.Settings.Default.ScryptThreadCount = ScryptThreadCount;
                Properties.Settings.Default.Argon2DegreesOfParallelism = DegreesOfParallelism;
                Properties.Settings.Default.Argon2Iterations = Argon2Iterations;
                Properties.Settings.Default.Argon2MemorySize = Argon2MemorySize;
                Properties.Settings.Default.Argon2SaltLength = Argon2SaltLength;
                Properties.Settings.Default.Argon2HashLength = Argon2HashLength;
                Properties.Settings.Default.TripleDesKeySize = (byte)TripleDesKeySize;
                Properties.Settings.Default.Pbkdf2HashSize = Pbkdf2HashSize;
                Properties.Settings.Default.Pbkdf2Iterations = Pbkdf2Iterations;
                Properties.Settings.Default.Pbkdf2Prefix = Pbkdf2Prefix;
                Properties.Settings.Default.Pbkdf2SaltSize = Pbkdf2SaltSize;
                Properties.Settings.Default.Save();
            });
        }
    }
}
