﻿using Decryptor.Core.Enums;
using Decryptor.Core.Utilities.Encryption;
using System;
using System.IO;
using System.Windows.Input;

namespace Decryptor.ViewModel.Commands
{
    class EncryptCommand : ICommand
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

        public EncryptCommand(DecryptorViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return VM.ModeEnum switch
            {
                Modes.Text => VM.Key != null && VM.Key.Length > 0 && 
                                    !string.IsNullOrWhiteSpace(VM.Text),
                Modes.File => VM.Key != null && VM.Key.Length > 0 &&
                                    !string.IsNullOrWhiteSpace(VM.Filename) &&
                                    !string.IsNullOrWhiteSpace(VM.OutputFile),
                _ => throw new NotImplementedException(),
            };
        }

        public async void Execute(object parameter)
        {
            var encryption = VM.EncryptionFactory.Create(VM.EncryptionAlgorithm);
            try
            {
                VM.IsBusy = true;
                switch (VM.ModeEnum)
                {
                    case Modes.Text:
                        VM.Result = await encryption.EncryptAsync(VM.Text);
                        break;
                    case Modes.File:
                        await encryption.EncryptAsync(VM.Filename, VM.OutputFile);
                        VM.SendMessage($"{Path.GetFileName(VM.Filename)} was encrypted as {Path.GetFileName(VM.OutputFile)}", "Success");
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                VM.SendMessage("This text could not be encrypted using this key", "Wrong Key", MessageType.Error, ex);
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
}
