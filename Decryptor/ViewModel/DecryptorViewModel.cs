using Decryptor.Core.Enums;
using Decryptor.Core.Utilities;
using Decryptor.Core.Utilities.Encryption;
using Decryptor.Core.Utilities.Hashing;
using Decryptor.Utilities;
using Decryptor.ViewModel.Commands;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;

namespace Decryptor.ViewModel
{
    enum MessageType
    {
        Error,
        Information
    }
    class OnMessageEventArgs : EventArgs
    {
        public OnMessageEventArgs(string message, string Title, MessageType messageType, Exception exception)
        {
            Message = message;
            this.Title = Title;
            MessageType = messageType;
            Exception = exception;
        }

        public string Message { get; set; }
        public string Title { get; }
        public MessageType MessageType { get; }
        public Exception Exception { get; set; }
    }

    class DecryptorViewModel : INotifyPropertyChanged
    {
        #region Observables
        private SecureString _key;
        private string _result;
        private string _text;
        private int _workFactor;
        private SecureString _password;
        private int _passwordLength;
        private HashAlgorithm _hashAlgorithm;
        private EncryptionAlgorithm _encryptionAlgorithm;
        private int _scryptIterations;
        private int _blockCount;
        private int _threadCount;
        private int _degreesOfParallelism;
        private int _argon2Iterations;
        private int _memorySize;
        private int _argon2SaltLength;
        private int _argon2HashLength;
        private bool? _checkSucceeded = null;
        private bool _isBusy = false;
        private int _mode;
        private string _filename;
        private string _checksum;
        private string _outputFile;
        private string _selectedTripleDesKeySize;
        private int _pbkdf2Iterations;
        private int _pbkdf2SaltSize;
        private int _pbkdf2HashSize;
        private string _pbkdf2Prefix;

        public SecureString Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyPropertyChanged();
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                NotifyPropertyChanged();
            }
        }
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                NotifyPropertyChanged();
            }
        }
        public int WorkFactor
        {
            get => _workFactor;
            set
            {
                _workFactor = value;
                NotifyPropertyChanged();
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChanged();
            }
        }
        public int PasswordLength
        {
            get => _passwordLength;
            set
            {
                _passwordLength = value;
                NotifyPropertyChanged();
            }
        }
        public HashAlgorithm HashAlgorithm
        {
            get => _hashAlgorithm;
            set
            {
                _hashAlgorithm = value;
                NotifyPropertyChanged();
            }
        }

        public EncryptionAlgorithm EncryptionAlgorithm
        {
            get => _encryptionAlgorithm;
            set
            {
                _encryptionAlgorithm = value;
                NotifyPropertyChanged();
            }
        }

        public int DegreesOfParallelism
        {
            get => _degreesOfParallelism;
            set
            {
                _degreesOfParallelism = value;
                NotifyPropertyChanged();
            }
        }
        public int Argon2Iterations
        {
            get => _argon2Iterations;
            set
            {
                _argon2Iterations = value;
                NotifyPropertyChanged();
            }
        }
        public int MemorySize
        {
            get => _memorySize;
            set
            {
                _memorySize = value;
                NotifyPropertyChanged();
            }
        }
        public bool? CheckSucceeded
        {
            get => _checkSucceeded;
            set
            {
                _checkSucceeded = value;
                NotifyPropertyChanged();
            }
        }
        public int ScryptIterations
        {
            get => _scryptIterations;
            set
            {
                _scryptIterations = value;
                NotifyPropertyChanged();
            }
        }
        public int BlockCount
        {
            get => _blockCount;
            set
            {
                _blockCount = value;
                NotifyPropertyChanged();
            }
        }
        public int ThreadCount
        {
            get => _threadCount;
            set
            {
                _threadCount = value;
                NotifyPropertyChanged();
            }
        }
        public int Argon2HashLength
        {
            get => _argon2HashLength;
            set
            {
                _argon2HashLength = value;
                NotifyPropertyChanged();
            }
        }
        public int Argon2SaltLength
        {
            get => _argon2SaltLength;
            set
            {
                _argon2SaltLength = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyPropertyChanged();
            }
        }

        public int Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                NotifyPropertyChanged();
            }
        }

        public Modes ModeEnum
        {
            get => (Modes)_mode;
        }

        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                SuggestOutputFilename(value);
                NotifyPropertyChanged();
            }
        }

        public string Checksum
        {
            get => _checksum;
            set
            {
                _checksum = value;
                NotifyPropertyChanged();
            }
        }

        public string OutputFile
        {
            get => _outputFile;
            set
            {
                _outputFile = value;
                NotifyPropertyChanged();
            }
        }

#pragma warning disable CA1822 // Mark members as static
        // Needs to be data bound to.
        public string[] TDesKeySizes => TripleDesUtil.KeySizes;
#pragma warning restore CA1822 // Mark members as static

        public string SelectedTripleDesKeySize
        {
            get => _selectedTripleDesKeySize;
            set
            {
                _selectedTripleDesKeySize = value;
                NotifyPropertyChanged();
            }
        }

        public int Pbkdf2Iterations
        {
            get => _pbkdf2Iterations;
            set
            {
                _pbkdf2Iterations = value;
                NotifyPropertyChanged();
            }
        }

        public int Pbkdf2SaltSize
        {
            get => _pbkdf2SaltSize;
            set
            {
                _pbkdf2SaltSize = value;
                NotifyPropertyChanged();
            }
        }

        public int Pbkdf2HashSize
        {
            get => _pbkdf2HashSize;
            set
            {
                _pbkdf2HashSize = value;
                NotifyPropertyChanged();
            }
        }

        public string Pbkdf2Prefix
        {
            get => _pbkdf2Prefix;
            set
            {
                _pbkdf2Prefix = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public CheckHashCommand CheckHashCommand { get; set; }
        public ClearCommand ClearCommand { get; set; }
        public CopyCommand CopyCommand { get; set; }
        public DecryptCommand DecryptCommand { get; set; }
        public EncryptCommand EncryptCommand { get; set; }
        public GenerateHashCommand GenerateHashCommand { get; set; }
        #endregion

        #region ctor
        public DecryptorViewModel()
        {
            // Initialize settings and factory
            Settings = new();
            EncryptionFactory = new(Settings);
            HashFactory = new(Settings);

            // Initialize commands
            CheckHashCommand = new CheckHashCommand(this);
            ClearCommand = new ClearCommand(this);
            CopyCommand = new CopyCommand(this);
            DecryptCommand = new DecryptCommand(this);
            EncryptCommand = new EncryptCommand(this);
            GenerateHashCommand = new GenerateHashCommand(this);
            LoadSettings();
        }
        #endregion

        #region Events
        public delegate void OnMessageHandler(object sender, OnMessageEventArgs args);
        public event OnMessageHandler OnMessage;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public Settings Settings { get; }
        public EncryptionFactory EncryptionFactory { get; }
        public HashFactory HashFactory { get; }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void LoadSettings()
        {
            Settings.Load();
            Key = PasswordProtector.DecryptString(Settings.Key);
            WorkFactor = Settings.BCryptWorkFactor;
            HashAlgorithm = Settings.HashAlgorithm;
            EncryptionAlgorithm = Settings.EncryptionAlgorithm;
            ScryptIterations = Settings.ScryptIterations;
            BlockCount = Settings.ScryptBlockCount;
            ThreadCount = Settings.ScryptThreadCount;
            DegreesOfParallelism = Settings.Argon2DegreesOfParallelism;
            Argon2Iterations = Settings.Argon2Iterations;
            MemorySize = Settings.Argon2MemorySize;
            Argon2SaltLength = Settings.Argon2SaltLength;
            Argon2HashLength = Settings.Argon2HashLength;
            TripleDesKeySize tDesKeySize = Settings.TripleDesKeySize;
            SelectedTripleDesKeySize = tDesKeySize.GetDisplayString();
            Pbkdf2HashSize = Settings.Pbkdf2HashSize;
            Pbkdf2Iterations = Settings.Pbkdf2Iterations;
            Pbkdf2Prefix = Settings.Pbkdf2Prefix;
            Pbkdf2SaltSize = Settings.Pbkdf2SaltSize;

            DecryptCommand.RaiseCanExecuteChanged();
            EncryptCommand.RaiseCanExecuteChanged();
        }

        public void SaveSettings()
        {
            Settings.Key = PasswordProtector.GetEncryptedString(Key);
            Settings.BCryptWorkFactor = WorkFactor;
            Settings.HashAlgorithm = HashAlgorithm;
            Settings.ScryptIterations = ScryptIterations;
            Settings.ScryptBlockCount = BlockCount;
            Settings.ScryptThreadCount = ThreadCount;
            Settings.Argon2DegreesOfParallelism = DegreesOfParallelism;
            Settings.Argon2Iterations = Argon2Iterations;
            Settings.Argon2MemorySize = MemorySize;
            Settings.Argon2SaltLength = Argon2SaltLength;
            Settings.Argon2HashLength = Argon2HashLength;
            TripleDesKeySize tDesKeySize = TripleDesUtil.Parse(SelectedTripleDesKeySize);
            Settings.TripleDesKeySize = tDesKeySize;
            Settings.Pbkdf2HashSize = Pbkdf2HashSize;
            Settings.Pbkdf2Iterations = Pbkdf2Iterations;
            Settings.Pbkdf2Prefix = Pbkdf2Prefix;
            Settings.Pbkdf2SaltSize = Pbkdf2SaltSize;
            Settings.Save();

            DecryptCommand.RaiseCanExecuteChanged();
            EncryptCommand.RaiseCanExecuteChanged();
        }

        public void SendMessage(string message, string title = null, MessageType messageType = MessageType.Information, Exception ex = null)
        {
            OnMessage?.Invoke(this, new OnMessageEventArgs(message, title, messageType, ex));
        }

        private void SuggestOutputFilename(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                OutputFile = string.Empty;
                return;
            }
            var ext = Path.GetExtension(value);
            if (ext.IsEncryptionExtension())
                OutputFile = Path.ChangeExtension(value, null);
            else
                OutputFile = $"{value}.{EncryptionAlgorithm.GetDefaultExt()}";
        }
    }
}
