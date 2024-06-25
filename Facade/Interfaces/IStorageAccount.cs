namespace Facade.Interfaces
{
    public interface IStorageAccount
    {
        void SendMessageAsync(string message);
        string SendMessage(string message);
        string GetMessages();
        void ProcessMessagesAsync();
        string ProcessMessages();
    }
}
