using Decryptor.Core.ViewModels;
using Decryptor.WinUI.Pages.SettingsPages;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Decryptor.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsMainPage : Page
    {
        public SettingsViewModel ViewModel => (SettingsViewModel)DataContext;

        public SettingsMainPage()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<SettingsViewModel>();
            ViewModel.Close += ViewModel_Close;
        }

        private void ViewModel_Close(object sender, EventArgs e)
        {
            App.MainWindow.ForceNavigateToText();
        }

        private void SettingsNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem item && item.Tag is not null)
            {
                SettingsNavView_Navigate(item);
            }
        }



        private void SettingsNavView_Navigate(NavigationViewItem navigationViewItem)
        {
            switch (navigationViewItem.Tag)
            {
                case "general":
                    ContentFrame.Navigate(typeof(GeneralEncryptionPage));
                    break;
                case "tripledes":
                    ContentFrame.Navigate(typeof(TripleDesPage));
                    break;
                case "bcrypt":
                    ContentFrame.Navigate(typeof(BCryptPage));
                    break;
                case "scrypt":
                    ContentFrame.Navigate(typeof(ScryptPage));
                    break;
                case "argon2":
                    ContentFrame.Navigate(typeof(Argon2Page));
                    break;
                case "pbkdf2":
                    ContentFrame.Navigate(typeof(Pbkdf2Page));
                    break;
                default:
                    break;
            }
        }

        private void SettingsNavView_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsNavView.SelectedItem = General;
            ContentFrame.Navigate(typeof(GeneralEncryptionPage));
        }
    }
}
