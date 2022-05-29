using Decryptor.Core.ViewModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Decryptor.WinUI.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TripleDesPage : Page
    {
        public SettingsViewModel ViewModel => (SettingsViewModel)DataContext;

        public TripleDesPage()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<SettingsViewModel>();
        }
    }
}
