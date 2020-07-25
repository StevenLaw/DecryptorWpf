using Decryptor.Utilities;
using Decryptor.ViewModel;
using System;
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
            SetCheck();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
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
            SetCheck();
        }

        private void SetCheck()
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
        }

        private void AboutCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
    }
}
