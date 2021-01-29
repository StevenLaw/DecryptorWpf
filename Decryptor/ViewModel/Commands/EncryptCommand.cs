using Decryptor.Utilities;
using Decryptor.Utilities.Encryption;
using System;
using System.Windows;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class EncryptCommand : ICommand
    {
        public DecryptorViewModel VM { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public EncryptCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return VM.Key != null && VM.Key.Length > 0 && 
                !string.IsNullOrWhiteSpace(VM.Text);
        }

        public async void Execute(object parameter)
        {
            var encryption = EncryptionFactory.Create(VM.EncryptionAlgorithm);
            try
            {
                VM.IsBusy = true;
                VM.Result = await encryption.EncryptAsync(VM.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow,
                                "This text could not be encrypted using this key",
                                "Wrong Key",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            finally
            {
                VM.CheckSucceeded = null;
                VM.IsBusy = false;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
