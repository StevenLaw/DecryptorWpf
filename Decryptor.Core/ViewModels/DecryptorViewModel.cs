using Decryptor.Core.Enums;
using Decryptor.Core.Interfaces;
using Decryptor.Core.Messages;
using Decryptor.Core.Utilities;
using Decryptor.Core.Utilities.Encryption;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Core.ViewModels
{
    public class DecryptorViewModel : ObservableRecipient // ObservableObject
    {
        #region Observable Properties
        private bool? _checkSucceeded;
        private string _checksum = string.Empty;
        private string _filename = string.Empty;
        private bool _isBusy;
        [AlsoNotifyChangeFor(nameof(ModeEnum))]
        private int _mode;
        private string _outputFile = string.Empty;
        private string _password = string.Empty;
        private string _result = string.Empty;

        private string _text = string.Empty;

        public bool? CheckSucceeded
        {
            get => _checkSucceeded;
            set
            {
                SetProperty(ref _checkSucceeded, value);
                ClearCommand.NotifyCanExecuteChanged();
            }
        }

        public string Checksum
        {
            get => _checksum;
            set
            {
                SetProperty(ref _checksum, value);
                CheckHashCommand.NotifyCanExecuteChanged();
            }
        }

        public string Filename
        {
            get => _filename;
            set
            {
                SetProperty(ref _filename, value);
                SuggestOutputFilename(value);
                CheckHashCommand.NotifyCanExecuteChanged();
                DecryptCommand.NotifyCanExecuteChanged();
                EncryptCommand.NotifyCanExecuteChanged();
                GenerateHashCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Key => Settings.Key;

        public int Mode
        {
            get => _mode;
            set
            {
                SetProperty(ref _mode, value);
                DecryptCommand.NotifyCanExecuteChanged();
                EncryptCommand.NotifyCanExecuteChanged();
            }
        }

        public Modes ModeEnum => (Modes)_mode;

        public string OutputFile
        {
            get => _outputFile;
            set
            {
                SetProperty(ref _outputFile, value);
                DecryptCommand.NotifyCanExecuteChanged();
                EncryptCommand.NotifyCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                CheckHashCommand.NotifyCanExecuteChanged();
                GenerateHashCommand.NotifyCanExecuteChanged();
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                SetProperty(ref _result, value);
                CopyCommand.NotifyCanExecuteChanged();
                ClearCommand.NotifyCanExecuteChanged();
            }
        }
        public string Text
        {
            get => _text;
            set 
            {
                SetProperty(ref _text, value);
                CheckHashCommand.NotifyCanExecuteChanged();
                DecryptCommand.NotifyCanExecuteChanged();
                EncryptCommand.NotifyCanExecuteChanged();
            }
        }
        #endregion

        #region Injected Properties
        public IAlertService AlertService { get; }
        public IClipboardManager ClipboardManager { get; }
        public IEncryptionFactory EncryptionFactory { get; }
        public IHashFactory HashFactory { get; }
        public IPasswordProtector PasswordProtector { get; }
        public ISettings Settings { get; }
        #endregion

        #region Commands
        public IAsyncRelayCommand CheckHashCommand { get; }
        public IRelayCommand ClearCommand { get; }
        public IRelayCommand CopyCommand { get; }
        public IAsyncRelayCommand DecryptCommand { get; }
        public IAsyncRelayCommand EncryptCommand { get; }
        public IAsyncRelayCommand GenerateHashCommand { get; }
        #endregion

        public DecryptorViewModel(ISettings settings,
                                  IEncryptionFactory encryptionFactory,
                                  IHashFactory hashFactory,
                                  IClipboardManager clipboardManager,
                                  IAlertService alertService,
                                  IPasswordProtector passwordProtector)
        {
            // Initialize Injectables
            Settings = settings;
            Settings.Load();
            EncryptionFactory = encryptionFactory;
            HashFactory = hashFactory;
            ClipboardManager = clipboardManager;
            AlertService = alertService;
            PasswordProtector = passwordProtector;

            // Initialize Commands
            CheckHashCommand = new AsyncRelayCommand(CheckHashExecute, CheckHashCanExecute);
            ClearCommand = new RelayCommand(ClearExecute, ClearCanExecute);
            CopyCommand = new RelayCommand(CopyExecute, CopyCanExecute);
            DecryptCommand = new AsyncRelayCommand(DecryptExecute, DecryptCanExecute);
            EncryptCommand = new AsyncRelayCommand(EncryptExecute, EncryptCanExecute);
            GenerateHashCommand = new AsyncRelayCommand(GenerateHashExecute, GenerateHashCanExecute);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            Messenger.Register<DecryptorViewModel, SettingsChangedMessage>(this, (r, m) => OnPropertyChanged(nameof(Key)));
        }

        private bool CheckHashCanExecute()
        {
            return ModeEnum switch
            {
                Modes.Text => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Text),
                Modes.File => !string.IsNullOrWhiteSpace(Filename) && !string.IsNullOrEmpty(Checksum),
                _ => throw new NotImplementedException(),
            };
        }

        private async Task CheckHashExecute()
        {
            try
            {
                var hash = HashFactory.Create(Settings.HashAlgorithm);
                IsBusy = true;
                CheckSucceeded = ModeEnum switch
                {
                    Modes.Text => await hash.CheckHashAsync(PasswordProtector.DecryptString(Password), Text),
                    Modes.File => await hash.CheckFileHashAsync(Filename, Checksum),
                    _ => throw new NotImplementedException(),
                };
            }
            catch (FileNotFoundException ex)
            {
                await AlertService.ShowError($"Couldn't find file {Filename}", "File not found", ex);
            }
            catch (Exception ex)
            {
                await AlertService.ShowError("Unexpected Error", "Error", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ClearCanExecute()
        {
            return !string.IsNullOrEmpty(Result) || CheckSucceeded != null;
        }

        private void ClearExecute()
        {
            Result = string.Empty;
            CheckSucceeded = null;
        }

        private bool CopyCanExecute()
        {
            return !string.IsNullOrEmpty(Result);
        }

        private bool DecryptCanExecute()
        {
            return ModeEnum switch
            {
                Modes.Text => !string.IsNullOrEmpty(Key)
                    && !string.IsNullOrWhiteSpace(Text),
                Modes.File => !string.IsNullOrEmpty(Key)
                    && !string.IsNullOrWhiteSpace(Filename)
                    && !string.IsNullOrWhiteSpace(OutputFile),
                _ => throw new NotImplementedException(),
            };
        }

        private async Task DecryptExecute()
        {
            try
            {
                var encryption = EncryptionFactory.Create(Settings.EncryptionAlgorithm);
                IsBusy = true;
                switch (ModeEnum)
                {
                    case Modes.Text:
                        Result = await encryption.DecryptAsync(Text);
                        break;
                    case Modes.File:
                        await encryption.DecryptAsync(Filename, OutputFile);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                await AlertService.ShowError($"{Path.GetFileName(Filename)} was decrypted as {Path.GetFileName(OutputFile)}", "Success", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool EncryptCanExecute()
        {
            return ModeEnum switch
            {
                Modes.Text => !string.IsNullOrEmpty(Key)
                    && !string.IsNullOrWhiteSpace(Text),
                Modes.File => !string.IsNullOrEmpty(Key)
                    && !string.IsNullOrWhiteSpace(Filename)
                    && !string.IsNullOrWhiteSpace(OutputFile),
                _ => throw new NotImplementedException(),
            };
        }

        private async Task EncryptExecute()
        {
            try
            {
                var encryption = EncryptionFactory.Create(Settings.EncryptionAlgorithm);
                IsBusy = true;
                switch (ModeEnum)
                {
                    case Modes.Text:
                        Result = await encryption.EncryptAsync(Text);
                        break;
                    case Modes.File:
                        await encryption.EncryptAsync(Filename, OutputFile);
                        await AlertService.ShowMessage($"{Path.GetFileName(Filename)} was encrypted as {Path.GetFileName(OutputFile)}", "Success");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await AlertService.ShowError("This text could not be encrypted using this key", "Wrong Key", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool GenerateHashCanExecute()
        {
            return ModeEnum switch
            {
                Modes.Text => !string.IsNullOrEmpty(Password),
                Modes.File => !string.IsNullOrWhiteSpace(Filename),
                _ => throw new NotImplementedException(),
            };
        }

        private async Task GenerateHashExecute()
        {
            try
            {
                var hash = HashFactory.Create(Settings.HashAlgorithm);
                IsBusy = true;
                Result = ModeEnum switch
                {
                    Modes.Text => await hash.GetHashAsync(PasswordProtector.DecryptString(Password)),
                    Modes.File => await hash.GetFileHashAsync(Filename),
                    _ => throw new NotImplementedException(),
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CopyExecute()
        {
            ClipboardManager.SetText(Result);
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
                OutputFile = $"{value}.{Settings.EncryptionAlgorithm.GetDefaultExt()}";
        }
    }
}
