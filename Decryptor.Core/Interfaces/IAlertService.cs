namespace Decryptor.Core.Interfaces
{
    public enum MessageType
    {
        Error,
        Information
    }

    public interface IAlertService
    {
        Task ShowMessage(string message, string title);
        Task ShowError(string message, string title, Exception? exception = null);
    }
}