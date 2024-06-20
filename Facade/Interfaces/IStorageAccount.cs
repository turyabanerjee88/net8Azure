namespace Facade.Interfaces
{
    public interface IStorageAccount
    {
        string SendMessage(string message);
        string GetMessages();
        string ProcessMessages();
    }
}
