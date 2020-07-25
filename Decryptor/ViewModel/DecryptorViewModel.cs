using Decryptor.Utilities;
using Decryptor.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

namespace Decryptor.ViewModel
{
    class DecryptorViewModel : INotifyPropertyChanged
    {
        #region Observables
        private SecureString _key;
        private string _result;
        private string _text;
        private int _workFactor;
        private SecureString _password;
        private HashAlgorithm _hashAlgorithm;
        private int _scryptIterations;
        private int _blockCount;
        private int _threadCount;
        private int _degreesOfParallelism;
        private int _argon2Iterations;
        private int _memorySize;
        private bool? _checkSucceeded = null;

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
        public HashAlgorithm HashAlgorithm
        {
            get => _hashAlgorithm;
            set 
            {
                _hashAlgorithm = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

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
            ScryptIterations = Properties.Settings.Default.ScryptIterationCount;
            BlockCount = Properties.Settings.Default.ScryptBlockCount;
            ThreadCount = Properties.Settings.Default.ScryptThreadCount;
            DegreesOfParallelism = Properties.Settings.Default.Argon2DegreesOfParallelism;
            Argon2Iterations = Properties.Settings.Default.Argon2Iterations;
            MemorySize = Properties.Settings.Default.Argon2MemorySize;

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
            Properties.Settings.Default.Save();

            DecryptCommand.RaiseCanExecuteChanged();
            EncryptCommand.RaiseCanExecuteChanged();
        }
    }
}
