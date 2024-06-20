using Facade.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Azuretests.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StorageQueueController : ControllerBase
    {
        private readonly IStorageAccount _storageAccount;

        public StorageQueueController(IStorageAccount storageAccount)
        {
            _storageAccount = storageAccount;
        }

        [HttpGet]
        public string SendMessage(string message)
        {
            return _storageAccount.SendMessage(message);
        }
        
        [HttpGet]
        public string GetMessages()
        {
            return _storageAccount.GetMessages();
        }

        [HttpGet]
        public string ProcessMessages()
        {
            return _storageAccount.ProcessMessages();
        }
    }
}
