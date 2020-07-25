using Decryptor.Utilities;
using Decryptor.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                key.ShowPassword(checkBox.IsChecked == true);
            }
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
    }
}
