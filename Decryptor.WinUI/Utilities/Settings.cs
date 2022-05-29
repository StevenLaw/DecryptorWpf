using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Utilities.Encryption;
using System.Threading.Tasks;
using Windows.Storage;

namespace Decryptor.WinUI.Utilities
{
    internal class Settings : ISettings
    {
        private const string _argon2DegreesOfParallelism = "Argon2DegreesOfParallelism";
        private const string _argon2HashLength = "Argon2HashLength";
        private const string _argon2Iterations = "Argon2Iterations";
        private const string _argon2MemorySize = "Argon2MemorySize";
        private const string _argon2SaltLength = "Argon2SaltLength";
        private const string _bCryptWorkFactor = "BCryptWorkFactor";
        private const string _degreesOfParallelism = "DegreesOfParallelism";
        private const string _encryptionAlgorithm = "EncryptionAlgorithm";
        private const string _hashAlgorithm = "HashAlgorithm";
        private const string _key = "Key";
        private const string _pbkdf2HashSize = "Pbkdf2HashSize";
        private const string _pbkdf2Iterations = "Pbkdf2Iterations";
        private const string _pbkdf2Prefix = "Pbkdf2Prefix";
        private const string _pbkdf2SaltSize = "Pbkdf2SaltSize";
        private const string _scryptBlockCount = "ScryptBlockCount";
        private const string _scryptIterations = "ScryptIterations";
        private const string _scriptThreadCount = "ScryptThreadCount";
        private const string _tripleDesKeySize = "TripleDesKeySize";

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
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            Argon2DegreesOfParallelism = localSettings.Values[_argon2DegreesOfParallelism] as int? ?? 8;
            Argon2HashLength = localSettings.Values[_argon2HashLength] as int? ?? 16;
            Argon2Iterations = localSettings.Values[_argon2Iterations] as int? ?? 4;
            Argon2MemorySize = localSettings.Values[_argon2MemorySize] as int? ?? 1048576;
            Argon2SaltLength = localSettings.Values[_argon2SaltLength] as int? ?? 16;
            BCryptWorkFactor = localSettings.Values[_bCryptWorkFactor] as int? ?? 12;
            DegreesOfParallelism = localSettings.Values[_degreesOfParallelism] as int? ?? 8;
            EncryptionAlgorithm = localSettings.Values[_encryptionAlgorithm] as EncryptionAlgorithm? ?? EncryptionAlgorithm.AES;
            HashAlgorithm = localSettings.Values[_hashAlgorithm] as HashAlgorithm? ?? HashAlgorithm.BCrypt;
            Key = localSettings.Values[_key] as string ?? string.Empty;
            Pbkdf2HashSize = localSettings.Values[_pbkdf2HashSize] as int? ?? 20;
            Pbkdf2Iterations = localSettings.Values[_pbkdf2Iterations] as int? ?? 10000;
            Pbkdf2Prefix = localSettings.Values[_pbkdf2Prefix] as string ?? "$PFKDF2$V1$";
            Pbkdf2SaltSize = localSettings.Values[_pbkdf2SaltSize] as int? ?? 16;
            ScryptBlockCount = localSettings.Values[_scryptBlockCount] as int? ?? 8;
            ScryptIterations = localSettings.Values[_scryptIterations] as int? ?? 16384;
            ScryptThreadCount = localSettings.Values[_scriptThreadCount] as int? ?? 1;
            TripleDesKeySize = localSettings.Values[_tripleDesKeySize] as TripleDesKeySize? ?? TripleDesKeySize.b128;
            
        }

        public async Task LoadAsync()
        {
            await Task.Run(Load);
        }

        public void Save()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[_argon2DegreesOfParallelism] = Argon2DegreesOfParallelism;
            localSettings.Values[_argon2HashLength] = Argon2HashLength;
            localSettings.Values[_argon2Iterations] = Argon2Iterations;
            localSettings.Values[_argon2MemorySize] = Argon2MemorySize;
            localSettings.Values[_argon2SaltLength] = Argon2SaltLength;
            localSettings.Values[_bCryptWorkFactor] = BCryptWorkFactor;
            localSettings.Values[_degreesOfParallelism] = DegreesOfParallelism;
            localSettings.Values[_encryptionAlgorithm] = EncryptionAlgorithm;
            localSettings.Values[_hashAlgorithm] = HashAlgorithm;
            localSettings.Values[_key] = Key;
            localSettings.Values[_pbkdf2HashSize] = Pbkdf2HashSize;
            localSettings.Values[_pbkdf2Iterations] = Pbkdf2Iterations;
            localSettings.Values[_pbkdf2Prefix] = Pbkdf2Prefix;
            localSettings.Values[_pbkdf2SaltSize] = Pbkdf2SaltSize;
            localSettings.Values[_scryptBlockCount] = ScryptBlockCount;
            localSettings.Values[_scryptIterations] = ScryptIterations;
            localSettings.Values[_scriptThreadCount] = ScryptThreadCount;
            localSettings.Values[_tripleDesKeySize] = TripleDesKeySize;

        }

        public async Task SaveAsync()
        {
            await Task.Run(Save);
        }
    }
}
