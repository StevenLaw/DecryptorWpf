using Decryptor.Core.Interfaces;
using Windows.ApplicationModel.DataTransfer;

namespace Decryptor.WinUI.Utilities
{
    internal class ClipboardManager : IClipboardManager
    {
        public void SetText(string text)
        {
            DataPackage package = new();
            package.SetText(text);
            Clipboard.SetContent(package);
        }
    }
}
