using Decryptor.View.SettingsViews;
using Decryptor.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Decryptor.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly DecryptorViewModel vm;

        public SettingsWindow()
        {
            InitializeComponent();

            vm = DataContext as DecryptorViewModel;
            ccSettings.Content = new GeneralEncryptionView();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveSettings();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            vm.LoadSettings();
            Close();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ccSettings != null && e.NewValue is TreeViewItem item)
            {
                switch (item.Header)
                {
                    case "General":
                        ccSettings.Content = new GeneralEncryptionView();
                        break;
                    case "Triple DES":
                        ccSettings.Content = new TripleDesView();
                        break;
                    case "BCrypt":
                        ccSettings.Content = new BCryptView();
                        break;
                    case "Scrypt":
                        ccSettings.Content = new ScryptView();
                        break;
                    case "Argon2":
                        ccSettings.Content = new ArgonView();
                        break;
                    case "PBKDF2":
                        ccSettings.Content = new Pbkdf2View();
                        break;
                }
            }
        }
    }
}
