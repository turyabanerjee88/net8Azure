using Facade.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Azuretests.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SecretController : ControllerBase
    {
        private readonly ISecretsManager _secretsManager;
        public SecretController(ISecretsManager secretsManager)
        {
            _secretsManager = secretsManager;
        }
        [HttpGet]
        public string GetSecret(string name)
        {
            return _secretsManager.GetSecret(name);
        }
        [HttpGet]
        public string SetSecret(string name,string value)
        {
            _secretsManager.SetSecret(name, value);
            return $"Secret saved with key {name} and value- {_secretsManager.GetSecret(name)}";
        }
    }
}
