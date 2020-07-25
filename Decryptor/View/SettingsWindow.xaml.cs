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
            SetupIterationComboBox();
        }

        private void SetupIterationComboBox()
        {
            var iterationValues = new List<int>();
            int i = 2;
            while (i > 0)
            {
                iterationValues.Add(i);
                i *= 2;
            }
            cmbIterations.ItemsSource = iterationValues;
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
