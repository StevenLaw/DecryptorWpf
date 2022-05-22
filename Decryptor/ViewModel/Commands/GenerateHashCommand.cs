using Decryptor.Core.Enums;
using Decryptor.Core.Utilities;
using Decryptor.Core.Utilities.Hashing;
using System;
using System.IO;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class GenerateHashCommand : ICommand
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

        public GenerateHashCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return VM.ModeEnum switch
            {
                Modes.Text => VM.PasswordLength > 0,
                Modes.File => !string.IsNullOrWhiteSpace(VM.Filename),
                _ => throw new NotImplementedException(),
            };
        }

        public async void Execute(object parameter)
        {
            VM.IsBusy = true;
            var hash = VM.HashFactory.Create(VM.HashAlgorithm);
            try
            {
                VM.Result = VM.ModeEnum switch
                {
                    Modes.Text => await hash.GetHashAsync(VM.Password.ToInsecureString()),
                    Modes.File => await hash.GetFileHashAsync(VM.Filename),
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
}
