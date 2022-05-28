using Decryptor.WinUI.Pages;
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

namespace Decryptor.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsMainPage));
                //NavView.Visibility = Visibility.Collapsed;
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                //var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                //NavView_Navigate(item);
                if (args.InvokedItemContainer is NavigationViewItem item)
                {
                    NavView_Navigate(item);
                }
            }
        }

        private void NavView_Navigate(NavigationViewItem navigationViewItem)
        {
            switch (navigationViewItem.Tag)
            {
                case "text":
                    ContentFrame.Navigate(typeof(TextPage));
                    break;
                case "file":
                    ContentFrame.Navigate(typeof(FilePage));
                    break;
                default:
                    break;
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "text")
                {
                    NavView.SelectedItem = item;
                    ContentFrame.Navigate(typeof(TextPage));
                    break;
                }
            }
        }
    }
}
