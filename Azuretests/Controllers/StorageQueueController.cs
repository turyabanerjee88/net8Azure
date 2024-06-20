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
        public async Task<string> SendMessage()
        {
            string storageAccountName = _configuration.GetValue<string>("StorageAccountName");
            var queueName = _configuration.GetValue<string>("QueueName");

            
            var vl = string.Empty;
            try
            {
                QueueClient queueClient = new QueueClient(
    new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
    new DefaultAzureCredential());
                await queueClient.SendMessageAsync("First message");
                await queueClient.SendMessageAsync("Second message");
            }
            catch (Exception ex)
            {
                vl = ex.Message;

            }
            return $"New Works! Exception:- {vl}";
        }
    }
}
