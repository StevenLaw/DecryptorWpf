using Decryptor.Utilities;
using Decryptor.Utilities.Hashing;
using System;
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
            return VM.PasswordLength > 0;
        }

        public async void Execute(object parameter)
        {
            VM.IsBusy = true;
            var hash = HashFactory.Create(VM.HashAlgorithm);
            VM.Result = await hash.GetHashAsync(VM.Password.ToInsecureString());
            VM.CheckSucceeded = null;
            VM.IsBusy = false;
        }
    }
}
