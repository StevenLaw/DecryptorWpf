using Decryptor.Utilities;
using System;
using System.Windows;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class DecryptCommand : ICommand
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

        public DecryptCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return VM.Key != null && VM.Key.Length > 0 && 
                   VM.Text != null && VM.Text.Length > 0;
        }

        public async void Execute(object parameter)
        {
            SimpleAes aes = new SimpleAes(VM.Key);
            try
            {
                VM.Result = await aes.DecryptAsync(VM.Text);
                VM.CheckSucceeded = null;
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow,
                                "This text was not encrypted using this key",
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
