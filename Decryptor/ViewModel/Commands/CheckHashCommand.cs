﻿using Decryptor.Utilities;
using System;
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
            return VM.Password != null && VM.Password.Length > 0 && !string.IsNullOrEmpty(VM.Text);
        }

        public async void Execute(object parameter)
        {
            var hm = HashManager.NewHashManager(VM.HashAlgorithm);
            VM.IsBusy = true;
            VM.CheckSucceeded = await hm.CheckHashAsync(VM.Password.ToInsecureString(), VM.Text);
            VM.IsBusy = false;
        }
    }
}
