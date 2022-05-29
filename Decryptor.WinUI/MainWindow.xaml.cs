using Decryptor.Core.Enums;
using Decryptor.Core.ViewModels;
using Decryptor.WinUI.Pages;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
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
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Decryptor.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public DecryptorViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<DecryptorViewModel>();

            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "BlueKey.ico"));
        }

        public void ForceNavigateToText()
        {
            ContentFrame.Navigate(typeof(TextPage));
            ViewModel.Mode = (int)Modes.Text;
        }

        public FileOpenPicker GetFileOpenPicker()
        {
            FileOpenPicker openPicker = new()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            return openPicker;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsMainPage));
            }
            else
            {
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
                    ViewModel.Mode = (int)Modes.Text;
                    break;
                case "file":
                    ContentFrame.Navigate(typeof(FilePage));
                    ViewModel.Mode = (int)Modes.File;
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
