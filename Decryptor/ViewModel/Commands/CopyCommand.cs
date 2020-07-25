using System;
using System.Windows;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class CopyCommand : ICommand
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

        public CopyCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(VM.Result);
        }

        public void Execute(object parameter)
        {
            Clipboard.SetText(VM.Result);
        }
    }
}
