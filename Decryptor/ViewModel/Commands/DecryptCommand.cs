using Decryptor.Utilities.Encryption;
using System;
using System.IO;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands;

class DecryptCommand(DecryptorViewModel vm) : ICommand
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
            Enums.Modes.Text => VM.Key != null && VM.Key.Length > 0 &&
                                !string.IsNullOrWhiteSpace(VM.Text),
            Enums.Modes.File => VM.Key != null && VM.Key.Length > 0 &&
                                !string.IsNullOrWhiteSpace(VM.Filename) &&
                                !string.IsNullOrWhiteSpace(VM.OutputFile),
            _ => throw new NotImplementedException(),
        };
    }

    public async void Execute(object parameter)
    {
        var encryption = EncryptionFactory.Create(VM.EncryptionAlgorithm);
        try
        {
            VM.IsBusy = true;
            switch (VM.ModeEnum)
            {
                case Enums.Modes.Text:
                    VM.Result = await encryption.DecryptAsync(VM.Text);
                    break;
                case Enums.Modes.File:
                    await encryption.DecryptAsync(VM.Filename, VM.OutputFile);
                    VM.SendMessage($"{Path.GetFileName(VM.Filename)} was decrypted as {Path.GetFileName(VM.OutputFile)}", "Success");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        catch (Exception ex)
        {
            VM.SendMessage("This text was not encrypted using this key", "Wrong Key", MessageType.Error, ex);
        }
        finally
        {
            VM.CheckSucceeded = null;
            VM.IsBusy = false;
        }
    }

    public static void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
