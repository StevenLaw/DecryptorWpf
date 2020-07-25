using Decryptor.Utilities;
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

        public void Execute(object parameter)
        {
            var aes = new SimpleAes(VM.Key);
            try
            {
                VM.Result = aes.Encrypt(VM.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow,
                                "This text could not be encrypted using this key",
                                "Wrong Key",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
