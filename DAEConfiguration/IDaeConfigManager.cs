using Microsoft.Extensions.Configuration;

namespace DaeConfiguration
{
    public interface IDaeConfigManager
    {
        string FedhaPOACS { get; }

        string EncryptionDecryptionAlgorithm { get; }

        string EncryptionDecryptionKey { get; }

        string EmailID { get; }

        string AccountKey { get; }

        string GetConnectionString(string connectionName);

        IConfigurationSection GetConfigurationSection(string Key);
    }
}