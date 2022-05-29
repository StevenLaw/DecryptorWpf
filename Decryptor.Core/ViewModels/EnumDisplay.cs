namespace Decryptor.Core.ViewModels
{
    public struct EnumDisplay<T> where T : Enum
    {
        public string Text { get; set; }
        public T Value { get; set; }
    }
}
