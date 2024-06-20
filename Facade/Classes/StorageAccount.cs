using Azure.Identity;
using Azure.Storage.Queues;
using Facade.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Facade.Classes
{
    public class StorageAccount : IStorageAccount
    {
        private readonly IConfiguration _configuration;
        public StorageAccount(IConfiguration configuration)
        {
           _configuration = configuration;
        }
        public string SendMessage(string message)
        {
            string storageAccountName = _configuration.GetSection("StorageAccountName")?.Value ??
                string.Empty;
            var queueName = _configuration.GetSection("QueueName").Value;
            var vl = string.Empty;
            if (!string.IsNullOrEmpty(storageAccountName))
            {
                try
                {
                    QueueClient queueClient = new QueueClient(
                    new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
                    new DefaultAzureCredential());
                    for (int i = 0; i < 10; i++)
                    {
                        queueClient.SendMessage($"{message}-{i}");
                    }
                }
                catch (Exception ex)
                {
                    vl = ex.Message;

                }
            }
            return $"New Works! Exception:- {vl}";
            throw new NotImplementedException();
        }
    }
}
