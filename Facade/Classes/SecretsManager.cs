using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Facade.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Facade.Classes
{
    public class SecretsManager : ISecretsManager
    {
        private readonly IConfiguration _configuration;
        public SecretsManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetSecret(string name)
        {
            string keyVaultName = _configuration.GetSection("kvname")?.Value ??
                string.Empty;
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

        public void SetSecret(string name, string value)
        {
            string keyVaultName = _configuration.GetSection("kvname")?.Value ??
                string.Empty;
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            client.SetSecret(name, value);
        }
    }
}
