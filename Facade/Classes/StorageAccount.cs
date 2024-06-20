using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
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
        public string GetMessages()
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
                    // Peek at messages in the queue
                    PeekedMessage[] peekedMessages = queueClient.PeekMessages(maxMessages: 30);

                    foreach (PeekedMessage peekedMessage in peekedMessages)
                    {
                        vl += $"Message: {peekedMessage.MessageText} |||";
                    }
                }
                catch (Exception ex)
                {
                    vl = ex.Message;

                }
            }
            return $"Messages retrieved! {vl}";
        }

        public string ProcessMessages()
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
                    // Peek at messages in the queue
                    QueueMessage[] queueMessages = queueClient.ReceiveMessages(maxMessages: 30);

                    foreach (QueueMessage msg in queueMessages)
                    {
                        vl += $"Processed Message: {msg.MessageText} |||";
                        queueClient.DeleteMessage(msg.MessageId, msg.PopReceipt);
                    }
                }
                catch (Exception ex)
                {
                    vl = ex.Message;

                }
            }
            return $"Message Process Works! {vl}";
        }
    }
}
