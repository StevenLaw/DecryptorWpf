using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Decryptor.View;

/// <summary>
/// Interaction logic for AboutWindow.xaml
/// </summary>
public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        var assembly = Assembly.GetExecutingAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        lblProduct.Content = versionInfo.ProductName;
        lblVersion.Content = $"Version: {versionInfo.ProductVersion}";
        lblCopyright.Content = versionInfo.LegalCopyright;
        tblkDescription.Text = versionInfo.Comments;
    }

    private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
        _ = Process.Start(new ProcessStartInfo(e.Uri.ToString()) { UseShellExecute = true });
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
