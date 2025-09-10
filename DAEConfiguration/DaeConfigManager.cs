using Microsoft.Extensions.Configuration;

namespace DaeConfiguration
{
    public class DaeConfigManager : IDaeConfigManager
    {
        private readonly IConfiguration _configuration;
        
        public DaeConfigManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string FedhaPOACS
        {
            get
            {
                return this._configuration["ConnectionStrings:FedhaPOACS"];
            }
        }


        public string EncryptionDecryptionAlgorithm
        {
            get
            {
                return this._configuration["AppSeettings:EncryptionDecryptionAlgorithm"];
            }
        }


        public string EncryptionDecryptionKey
        {
            get
            {
                return this._configuration["AppSeettings:EncryptionDecryptionKey"];
            }
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString(connectionName);
        }

        public string EmailID
        {
            get
            {
                return this._configuration["AppSeettings:EmailID"];
            }
        }

        public string AccountKey
        {
            get
            {
                return this._configuration["AppSeettings:AccountKey"];
            }
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration.GetSection(key);
        }
    }
}