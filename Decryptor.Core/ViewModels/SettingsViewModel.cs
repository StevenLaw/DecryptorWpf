using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Messages;
using Decryptor.Core.Utilities.Encryption;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Decryptor.Core.ViewModels
{
    public struct EnumDisplay<T> where T : Enum
    {
        public string Text { get; set; }
        public T Value { get; set; }
    }

    public class SettingsViewModel : ObservableRecipient
    {
        #region Observable Properties
        private int _argon2DegreesOfParallelism;
        private int _argon2HashLength;
        private int _argon2Iterations;
        private int _argon2MemorySize;
        private int _argon2SaltLength;
        private int _bCryptWorkFactor;
        private EncryptionAlgorithm _encryptionAlgorithm;
        private HashAlgorithm _hashAlgorithm;
        private string _key = string.Empty;
        private int _pbkdf2HashSize;
        private int _pbkdf2Iterations;
        private string _pbkdf2Prefix = string.Empty;
        private int _pbkdf2SaltSize;
        private int _scryptBlockCount;
        private int _scryptIterations;
        private int _scryptThreadCount;
        private TripleDesKeySize _tripleDesKeySize;

        public int Argon2DegreesOfParallelism
        {
            get => _argon2DegreesOfParallelism;
            set => SetProperty(ref _argon2DegreesOfParallelism, value);
        }
        public int Argon2HashLength
        {
            get => _argon2HashLength;
            set => SetProperty(ref _argon2HashLength, value);
        }
        public int Argon2Iterations
        {
            get => _argon2Iterations;
            set => SetProperty(ref _argon2Iterations, value);
        }
        public int Argon2MemorySize
        {
            get => _argon2MemorySize;
            set => SetProperty(ref _argon2MemorySize, value);
        }
        public int Argon2SaltLength
        {
            get => _argon2SaltLength;
            set => SetProperty(ref _argon2SaltLength, value);
        }
        public int BCryptWorkFactor
        {
            get => _bCryptWorkFactor;
            set => SetProperty(ref _bCryptWorkFactor, value);
        }
        public EncryptionAlgorithm EncryptionAlgorithm
        {
            get => _encryptionAlgorithm;
            set => SetProperty(ref _encryptionAlgorithm, value);
        }
        public HashAlgorithm HashAlgorithm
        {
            get => _hashAlgorithm;
            set => SetProperty(ref _hashAlgorithm, value);
        }
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }
        public int Pbkdf2HashSize
        {
            get => _pbkdf2HashSize;
            set => SetProperty(ref _pbkdf2HashSize, value);
        }
        public int Pbkdf2Iterations
        {
            get => _pbkdf2Iterations;
            set => SetProperty(ref _pbkdf2Iterations, value);
        }
        public string Pbkdf2Prefix
        {
            get => _pbkdf2Prefix;
            set => SetProperty(ref _pbkdf2Prefix, value);
        }
        public int Pbkdf2SaltSize
        {
            get => _pbkdf2SaltSize;
            set => SetProperty(ref _pbkdf2SaltSize, value);
        }
        public int ScryptBlockCount
        {
            get => _scryptBlockCount;
            set => SetProperty(ref _scryptBlockCount, value);
        }
        public int ScryptIterations
        {
            get => _scryptIterations;
            set => SetProperty(ref _scryptIterations, value);
        }
        public int ScryptThreadCount
        {
            get => _scryptThreadCount;
            set => SetProperty(ref _scryptThreadCount, value);
        }
        public TripleDesKeySize TripleDesKeySize
        {
            get => _tripleDesKeySize;
            set => SetProperty(ref _tripleDesKeySize, value);
        }
        #endregion

        #region Enum Lists
        public EnumDisplay<EncryptionAlgorithm>[] EncryptionAlgorithms => Enum.GetValues(typeof(EncryptionAlgorithm))
            .Cast<EncryptionAlgorithm>()
            .Select(e => new EnumDisplay<EncryptionAlgorithm>()
            {
                Text = e switch
                {
                    EncryptionAlgorithm.TripleDES => "Triple DES",
                    _ => e.ToString()
                },
                Value = e
            })
            .ToArray();
        public EnumDisplay<HashAlgorithm>[] HashAlgorithms => Enum.GetValues(typeof(HashAlgorithm))
            .Cast<HashAlgorithm>()
            .Select(h => new EnumDisplay<HashAlgorithm>()
            {
                Text = h.ToString(),
                Value = h
            })
            .ToArray();
        public EnumDisplay<TripleDesKeySize>[] TripleDesKeySizes => Enum.GetValues(typeof(TripleDesKeySize))
            .Cast<TripleDesKeySize>()
            .Select(t => new EnumDisplay<TripleDesKeySize>()
            {
                Text = t.ToString(),
                Value = t
            })
            .ToArray();
        #endregion

        #region Injected Properties
        public ISettings Settings { get; }
        #endregion

        #region Command
        public IAsyncRelayCommand SaveCommand { get; }
        public IRelayCommand CancelCommand { get; }
        #endregion

        #region Events
        public event EventHandler? Close;
        #endregion

        public SettingsViewModel(ISettings settings)
        {
            Settings = settings;

            SaveCommand = new AsyncRelayCommand(SaveExecute);
            CancelCommand = new RelayCommand(CancelExecute);
        }

        public async Task Load()
        {
            await Settings.LoadAsync();
            Argon2DegreesOfParallelism = Settings.Argon2DegreesOfParallelism;
            Argon2HashLength = Settings.Argon2HashLength;
            Argon2Iterations = Settings.Argon2Iterations;
            Argon2MemorySize = Settings.Argon2MemorySize;
            Argon2SaltLength = Settings.Argon2SaltLength;
            BCryptWorkFactor = Settings.BCryptWorkFactor;
            EncryptionAlgorithm = Settings.EncryptionAlgorithm;
            HashAlgorithm = Settings.HashAlgorithm;
            Key = Settings.Key;
            Pbkdf2HashSize = Settings.Pbkdf2HashSize;
            Pbkdf2Iterations = Settings.Pbkdf2Iterations;
            Pbkdf2Prefix = Settings.Pbkdf2Prefix;
            Pbkdf2SaltSize = Settings.Pbkdf2SaltSize;
            ScryptBlockCount = Settings.ScryptBlockCount;
            ScryptIterations = Settings.ScryptIterations;
            ScryptThreadCount = Settings.ScryptThreadCount;
            TripleDesKeySize = Settings.TripleDesKeySize;
        }

        private void CancelExecute()
        {
            Close?.Invoke(this, EventArgs.Empty);
        }

        private async Task SaveExecute()
        {
            Settings.Argon2DegreesOfParallelism = Argon2DegreesOfParallelism;
            Settings.Argon2HashLength = Argon2HashLength;
            Settings.Argon2Iterations = Argon2Iterations;
            Settings.Argon2MemorySize = Argon2MemorySize;
            Settings.Argon2SaltLength = Argon2SaltLength;
            Settings.BCryptWorkFactor = BCryptWorkFactor;
            Settings.EncryptionAlgorithm = EncryptionAlgorithm;
            Settings.HashAlgorithm = HashAlgorithm;
            Settings.Key = Key;
            Settings.Pbkdf2HashSize = Pbkdf2HashSize;
            Settings.Pbkdf2Iterations = Pbkdf2Iterations;
            Settings.Pbkdf2Prefix = Pbkdf2Prefix;
            Settings.Pbkdf2SaltSize = Pbkdf2SaltSize;
            Settings.ScryptBlockCount = ScryptBlockCount;
            Settings.ScryptIterations = ScryptIterations;
            Settings.ScryptThreadCount = ScryptThreadCount;
            Settings.TripleDesKeySize = TripleDesKeySize;
            await Settings.SaveAsync();
            Messenger.Send(new SettingsChangedMessage(Settings));
            Close?.Invoke(this, EventArgs.Empty);
        }
    }
}
