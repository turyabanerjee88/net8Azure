using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace Azuretests.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StorageQueueController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<StorageQueueController> _logger;
        private readonly IConfiguration _configuration;

        public StorageQueueController(ILogger<StorageQueueController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "SendMessage")]
        public async Task<string> SendMessage(string message)
        {
            string storageAccountName = _configuration.GetValue<string>("StorageAccountName");
            var queueName = _configuration.GetValue<string>("QueueName");

            
            var vl = string.Empty;
            try
            {
                QueueClient queueClient = new QueueClient(
                new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
                new DefaultAzureCredential());
                for (int i = 0; i < 10; i++)
                {
                    await queueClient.SendMessageAsync($"{message}-{i}");
                }                
            }
            catch (Exception ex)
            {
                vl = ex.Message;

            }
            return $"New Works! Exception:- {vl}";
        }
        [HttpGet]
        public async Task<string> GetMessages()
        {
            string storageAccountName = _configuration.GetValue<string>("StorageAccountName");
            var queueName = _configuration.GetValue<string>("QueueName");

            var vl = string.Empty;
            try
            {
                QueueClient queueClient = new QueueClient(
                new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
                new DefaultAzureCredential());
                // Peek at messages in the queue
                PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync(maxMessages: 10);

                foreach (PeekedMessage peekedMessage in peekedMessages)
                {
                    vl += $"Message: {peekedMessage.MessageText} |||";
                }
            }
            catch (Exception ex)
            {
                vl = ex.Message;

            }
            return $"Messages retrieved! {vl}";
        }
        [HttpGet]
        public async Task<string> ProcessMessages()
        {
            string storageAccountName = _configuration.GetValue<string>("StorageAccountName");
            var queueName = _configuration.GetValue<string>("QueueName");

            var vl = string.Empty;
            try
            {
                QueueClient queueClient = new QueueClient(
                new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
                new DefaultAzureCredential());
                // Peek at messages in the queue
                QueueMessage[] queueMessages = await queueClient.ReceiveMessagesAsync(maxMessages: 10);

                foreach (QueueMessage msg in queueMessages)
                {
                    vl += $"Processed Message: {msg.MessageText} |||";
                    await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
                }
            }
            catch (Exception ex)
            {
                vl = ex.Message;

            }
            return $"Message Process Works! {vl}";
        }
    }
}
