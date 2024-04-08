using System;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands;

class ClearCommand(DecryptorViewModel vm) : ICommand
{
    public DecryptorViewModel VM { get; set; } = vm;
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
