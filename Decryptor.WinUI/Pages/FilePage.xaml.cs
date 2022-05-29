using Decryptor.Core.ViewModels;
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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Decryptor.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FilePage : Page
    {
        public DecryptorViewModel ViewModel => (DecryptorViewModel)DataContext;

        public FilePage()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<DecryptorViewModel>();
        }

        private void TextBox_DragOver(object _, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems)
                && sender is TextBox textBox)
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;
                    textBox.Text = storageFile.Path;
                }
            }
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                FileOpenPicker openPicker = new()
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                };

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

                openPicker.FileTypeFilter.Add("*");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file is not null)
                {
                    switch (button.Tag as string)
                    {
                        case "InputFilePicker":
                            ViewModel.Filename = file.Path;
                            break;
                        case "OutputFilePicker":
                            ViewModel.OutputFile = file.Path;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

    }
}
