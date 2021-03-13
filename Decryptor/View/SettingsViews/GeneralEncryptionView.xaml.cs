using System.Windows;
using System.Windows.Controls;

namespace Decryptor.View.SettingsViews
{
    /// <summary>
    /// Interaction logic for GeneralEncryptionView.xaml
    /// </summary>
    public partial class GeneralEncryptionView : UserControl
    {
        public GeneralEncryptionView()
        {
            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                key.ShowPassword(checkBox.IsChecked == true);
            }
        }
    }
}
