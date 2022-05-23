using Decryptor.Core.Interfaces;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Decryptor.Core.Messages
{
    public sealed class SettingsChangedMessage : ValueChangedMessage<ISettings>
    {
        public SettingsChangedMessage(ISettings value) : base(value)
        {
        }
    }
}
