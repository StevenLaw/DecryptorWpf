using Decryptor.Utilities;
using Decryptor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Decryptor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DecryptorViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = DataContext as DecryptorViewModel;
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
    }
}
