using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace Azuretests.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SecretController : ControllerBase
    {
        private readonly ILogger<SecretController> _logger;
        private readonly IConfiguration _configuration;

        public SecretController(ILogger<SecretController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetSecret")]
        public string GetSecret(string name)
        {
            string keyVaultName = _configuration.GetValue<string>("kvname");
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var vl = string.Empty;
            try
            {
                var secret = client.GetSecret(name);
                vl = secret.Value.Value.ToString();
            }
            catch (Exception ex)
            {
                vl = ex.Message;

            }            
            return $"Get Secret Works! {(vl)}, Kv name - {keyVaultName}";
        }
    }
}
