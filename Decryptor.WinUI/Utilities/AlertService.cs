using Decryptor.Core.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.WinUI.Utilities
{
    internal class AlertService : IAlertService
    {
        private readonly IClipboardManager _clipboardManager;
        private readonly FrameworkElement _mainRoot;

        public AlertService(IClipboardManager clipboardManager)
        {
            _clipboardManager = clipboardManager;
            _mainRoot = App.MainRoot;
        }

        public async Task ShowError(string message, string title, Exception exception = null)
        {
            ContentDialog errorDialog = new()
            {
                Title = title,
                Content = message,
                CloseButtonText = "Close",
                PrimaryButtonText = "Copy",
                XamlRoot = _mainRoot.XamlRoot
            };

            ContentDialogResult result = await errorDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                StringBuilder sb = new(message);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendFormat("ErrorType: {0}\n", exception.GetType());
                sb.AppendLine();
                sb.AppendLine(exception.Message);
                sb.AppendLine();
                sb.AppendLine(exception.StackTrace);
                _clipboardManager.SetText(sb.ToString());
            }
        }

        public async Task ShowMessage(string message, string title)
        {
            ContentDialog messageDialog = new()
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
                XamlRoot = _mainRoot.XamlRoot
            };

            await messageDialog.ShowAsync();
        }
    }
}
