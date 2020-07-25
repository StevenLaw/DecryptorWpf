using System;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class ClearCommand : ICommand
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

        public ClearCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(VM.Result) || VM.CheckSucceeded != null;
        }

        public void Execute(object parameter)
        {
            VM.Result = string.Empty;
            VM.CheckSucceeded = null;
        }
    }
}
