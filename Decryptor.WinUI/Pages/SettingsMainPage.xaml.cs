using Decryptor.WinUI.Pages.SettingsPages;
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
        public SettingsMainPage()
        {
            this.InitializeComponent();
        }

        private void SettingsNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // find NavigationViewItem with Content that equals InvokedItem
            //var item = sender.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(x => (string)x.Content == (string)args.InvokedItem);
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
            foreach (NavigationViewItemBase item in SettingsNavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag is not null && item.Tag.ToString() == "general")
                {
                    SettingsNavView.SelectedItem = item;
                    ContentFrame.Navigate(typeof(GeneralEncryptionPage));
                    break;
                }
            }
        }
    }
}
