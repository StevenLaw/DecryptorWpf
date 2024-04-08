using Decryptor.Utilities;
using Decryptor.Utilities.Hashing;
using System;
using System.IO;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands;

class GenerateHashCommand(DecryptorViewModel vm) : ICommand
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
        return VM.ModeEnum switch
        {
            Enums.Modes.Text => VM.PasswordLength > 0,
            Enums.Modes.File => !string.IsNullOrWhiteSpace(VM.Filename),
            _ => throw new NotImplementedException(),
        };
    }

    public async void Execute(object parameter)
    {
        VM.IsBusy = true;
        var hash = HashFactory.Create(VM.HashAlgorithm);
        try
        {
            VM.Result = VM.ModeEnum switch
            {
                Enums.Modes.Text => await hash.GetHashAsync(VM.Password.ToInsecureString()),
                Enums.Modes.File => await hash.GetFileHashAsync(VM.Filename),
                _ => throw new NotImplementedException(),
            };
        }
        catch (FileNotFoundException ex)
        {
            VM.SendMessage($"Couldn't find file {VM.Filename}", "File not found", MessageType.Error, ex);
        }
        VM.CheckSucceeded = null;
        VM.IsBusy = false;
    }
}
