namespace Facade.Interfaces
{
    public interface ISecretsManager
    {
        string GetSecret(string name);
        void SetSecret(string name, string value);
    }
}
