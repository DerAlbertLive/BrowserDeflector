using Xunit;

namespace Deflector.Tests
{
    public class ConfigurationFixture
    {
        public ConfigurationFixture()
        {
            var loader = new ConfigurationLoader();
            Configuration = ConfigurationLoader.Load("TestConfiguration.json");
        }

        public DeflectorConfiguration Configuration { get; }
    }

    [CollectionDefinition("Configuration")]
    public class ConfigurationCollection : ICollectionFixture<ConfigurationFixture>
    {

    }
}