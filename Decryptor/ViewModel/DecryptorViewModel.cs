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
        private int _degreesOfParallelism;
        private int _iterations;
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
        public int Iterations
        {
            get => _iterations;
            set
            {
                _iterations = value;
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
            WorkFactor = Properties.Settings.Default.WorkFactor;
            HashAlgorithm = (HashAlgorithm)Properties.Settings.Default.HashAlgorithm;
            DegreesOfParallelism = Properties.Settings.Default.DegreesOfParallelism;
            Iterations = Properties.Settings.Default.Iterations;
            MemorySize = Properties.Settings.Default.MemorySize;

            DecryptCommand.RaiseCanExecuteChanged();
            EncryptCommand.RaiseCanExecuteChanged();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Key = PasswordProtector.GetEncryptedString(Key);
            Properties.Settings.Default.WorkFactor = WorkFactor;
            Properties.Settings.Default.HashAlgorithm = (byte)HashAlgorithm;
            Properties.Settings.Default.DegreesOfParallelism = DegreesOfParallelism;
            Properties.Settings.Default.Iterations = Iterations;
            Properties.Settings.Default.MemorySize = MemorySize;
            Properties.Settings.Default.Save();

            DecryptCommand.RaiseCanExecuteChanged();
            EncryptCommand.RaiseCanExecuteChanged();
        }
    }
}
