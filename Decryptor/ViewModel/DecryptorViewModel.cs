using Decryptor.Enums;
using Decryptor.Utilities;
using Decryptor.Utilities.Encryption;
using Decryptor.ViewModel.Commands;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void LoadSettings()
        {
            Key = PasswordProtector.DecryptString(Properties.Settings.Default.Key);
            WorkFactor = Properties.Settings.Default.BCryptWorkFactor;
            HashAlgorithm = (HashAlgorithm)Properties.Settings.Default.HashAlgorithm;
            EncryptionAlgorithm = (EncryptionAlgorithm)Properties.Settings.Default.EncryptionAlgorithm;
            ScryptIterations = Properties.Settings.Default.ScryptIterationCount;
            BlockCount = Properties.Settings.Default.ScryptBlockCount;
            ThreadCount = Properties.Settings.Default.ScryptThreadCount;
            DegreesOfParallelism = Properties.Settings.Default.Argon2DegreesOfParallelism;
            Argon2Iterations = Properties.Settings.Default.Argon2Iterations;
            MemorySize = Properties.Settings.Default.Argon2MemorySize;
            Argon2SaltLength = Properties.Settings.Default.Argon2SaltLength;
            Argon2HashLength = Properties.Settings.Default.Argon2HashLength;
            TripleDesKeySize tDesKeySize = (TripleDesKeySize)Properties.Settings.Default.TripleDesKeySize;
            SelectedTripleDesKeySize = tDesKeySize.GetDisplayString();

            DecryptCommand.RaiseCanExecuteChanged();
            EncryptCommand.RaiseCanExecuteChanged();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Key = PasswordProtector.GetEncryptedString(Key);
            Properties.Settings.Default.BCryptWorkFactor = WorkFactor;
            Properties.Settings.Default.HashAlgorithm = (byte)HashAlgorithm;
            Properties.Settings.Default.ScryptIterationCount = ScryptIterations;
            Properties.Settings.Default.ScryptBlockCount = BlockCount;
            Properties.Settings.Default.ScryptThreadCount = ThreadCount;
            Properties.Settings.Default.Argon2DegreesOfParallelism = DegreesOfParallelism;
            Properties.Settings.Default.Argon2Iterations = Argon2Iterations;
            Properties.Settings.Default.Argon2MemorySize = MemorySize;
            Properties.Settings.Default.Argon2SaltLength = Argon2SaltLength;
            Properties.Settings.Default.Argon2HashLength = Argon2HashLength;
            TripleDesKeySize tDesKeySize = TripleDesUtil.Parse(SelectedTripleDesKeySize);
            Properties.Settings.Default.TripleDesKeySize = (byte)tDesKeySize;
            Properties.Settings.Default.Save();

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
