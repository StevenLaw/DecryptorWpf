using Decryptor.Enums;
using Decryptor.Utilities.Encryption;
using Decryptor.ViewModel;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Decryptor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DecryptorViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = DataContext as DecryptorViewModel;
            vm.OnMessage += Vm_OnMessage;
            SetCheckEnc();
            SetCheckHash();
        }

        private void Vm_OnMessage(object sender, OnMessageEventArgs args)
        {
            MessageBox.Show(this,
                            args.Message,
                            args.Title,
                            MessageBoxButton.OK,
                            args.MessageType switch
                            {
                                MessageType.Error => MessageBoxImage.Error,
                                MessageType.Information => MessageBoxImage.Information,
                                _ => MessageBoxImage.Question,
                            });
        }

        private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb)
            {
                password.ShowPassword(cb.IsChecked == true);
            }
        }

        private void SettingsCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            (new SettingsWindow()).ShowDialog();
        }

        private void EncMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && ! menuItem.IsChecked)
            {
                var algo = EncryptionUtil.ParseAlgorithm(menuItem.Header.ToString());
                if (algo != EncryptionAlgorithm.None)
                {
                    vm.EncryptionAlgorithm = algo;
                    Properties.Settings.Default.EncryptionAlgorithm = (byte)algo;
                    Properties.Settings.Default.Save();
                }
            }
            SetCheckEnc();
        }

        private void SetCheckEnc()
        {
            var algorithm = vm.EncryptionAlgorithm;
            if (algorithm == EncryptionAlgorithm.AES) miAes.IsChecked = true;
            else miAes.IsChecked = false;
            if (algorithm == EncryptionAlgorithm.DES) miDes.IsChecked = true;
            else miDes.IsChecked = false;
            if (algorithm == EncryptionAlgorithm.TripleDES) miTDes.IsChecked = true;
            else miTDes.IsChecked = false;
        }

        private void HashMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // If the sender is already checked then ignore it
            if (sender is MenuItem menuItem && !menuItem.IsChecked)
            {
                if (Enum.TryParse(menuItem.Header.ToString(), out HashAlgorithm algo))
                {
                    vm.HashAlgorithm = algo;
                    Properties.Settings.Default.HashAlgorithm = (byte)algo;
                    Properties.Settings.Default.Save();
                }
            }
            SetCheckHash();
        }

        private void SetCheckHash()
        {
            var algorithm = vm.HashAlgorithm;
            if (algorithm == HashAlgorithm.Argon2) miArgon2.IsChecked = true;
            else miArgon2.IsChecked = false;
            if (algorithm == HashAlgorithm.BCrypt) miBCrypt.IsChecked = true;
            else miBCrypt.IsChecked = false;
            if (algorithm == HashAlgorithm.MD5) miMD5.IsChecked = true;
            else miMD5.IsChecked = false;
            if (algorithm == HashAlgorithm.Scrypt) miScrypt.IsChecked = true;
            else miScrypt.IsChecked = false;
            if (algorithm == HashAlgorithm.SHA1) miSHA1.IsChecked = true;
            else miSHA1.IsChecked = false;
            if (algorithm == HashAlgorithm.SHA256) miSHA256.IsChecked = true;
            else miSHA256.IsChecked = false;
            if (algorithm == HashAlgorithm.SHA512) miSHA512.IsChecked = true;
            else miSHA512.IsChecked = false;
            if (algorithm == HashAlgorithm.PBKDF2) miPbkdf2.IsChecked = true;
            else miPbkdf2.IsChecked= false;
        }

        private void AboutCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnFileBrowse":
                        var ofd = new OpenFileDialog
                        {
                            Filter = "All files (*.*)|*.*|AES files (*.aes)|*.aes|DES files (*.des)|*.des|Triple DES files (*.3ds)|*.3ds"
                        };
                        if (ofd.ShowDialog(this) == true)
                        {
                            if (!ofd.CheckPathExists)
                            {
                                MessageBox.Show(this, "Unable to find file path.", "Error", MessageBoxButton.OK,
                                                  MessageBoxImage.Error);
                                return;
                            }
                            if (!ofd.CheckFileExists)
                            {
                                MessageBox.Show(this, "Unable to find file.", "Error", MessageBoxButton.OK,
                                                MessageBoxImage.Error);
                                return;
                            }
                            vm.Filename = ofd.FileName;
                        }
                        break;
                    case "btnOutputBrowse":
                        var sfd = new SaveFileDialog
                        {
                            DefaultExt = vm.EncryptionAlgorithm.GetDefaultExt()
                        };
                        if (sfd.ShowDialog(this) == true)
                        {
                            if (!sfd.CheckPathExists)
                            {
                                MessageBox.Show(this, "Unable to find file path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            if (!sfd.CheckFileExists)
                            {
                                var overwrite =  MessageBox.Show(this, "Do you want to overwrite this file?", "Error",
                                                                 MessageBoxButton.YesNo, MessageBoxImage.Error);
                                if (overwrite == MessageBoxResult.No) return;
                            }
                            vm.OutputFile = sfd.FileName;
                        }
                        break;
                }
            }
        }

        private void InputFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                vm.Filename = files[0];
            }
        }

        private void OutputFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                vm.OutputFile = files[0];
            }    
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
    }
}
