using Decryptor.Core.Enums;
using Decryptor.Core.Utilities;
using Decryptor.Core.Utilities.Hashing;
using System;
using System.IO;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class CheckHashCommand : ICommand
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

        public CheckHashCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return VM.ModeEnum switch
            {
                Modes.Text => VM.PasswordLength > 0 && !string.IsNullOrEmpty(VM.Text),
                Modes.File => !string.IsNullOrWhiteSpace(VM.Filename) && !string.IsNullOrEmpty(VM.Checksum),
                _ => throw new NotImplementedException(),
            };
        }

        public async void Execute(object parameter)
        {
            var hash = VM.HashFactory.Create(VM.HashAlgorithm);
            VM.IsBusy = true;
            try
            {
                VM.CheckSucceeded = VM.ModeEnum switch
                {
                    Modes.Text => await hash.CheckHashAsync(VM.Password.ToInsecureString(), VM.Text),
                    Modes.File => await hash.CheckFileHashAsync(VM.Filename, VM.Checksum),
                    _ => throw new NotImplementedException(),
                };
            }
            catch (FileNotFoundException ex)
            {
                VM.SendMessage($"Couldn't find file {VM.Filename}", "File not found", MessageType.Error, ex);
            }
            VM.IsBusy = false;
        }
    }
}
