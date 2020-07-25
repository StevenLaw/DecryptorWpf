using Decryptor.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
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
            return VM.Password != null && VM.Password.Length > 0;
        }

        public void Execute(object parameter)
        {
            var hm = HashManager.NewHashManager(VM.HashAlgorithm);
            VM.Result = hm.GetHash(VM.Password.ToInsecureString());
        }
    }
}
