using Microsoft.Extensions.Configuration;
using Xunit;

namespace BrowserOpener.Tests
{
    public class ConfigurationFixture
    {
        public ConfigurationFixture()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("TestConfiguration.json", optional: false);

            var configuration = configurationBuilder.Build();
            var browserOpenerConfiguration = new BrowserOpenerConfiguration();

            configuration.Bind(browserOpenerConfiguration);
            Configuration = browserOpenerConfiguration;
        }

        public BrowserOpenerConfiguration Configuration { get; }
    }

    [CollectionDefinition("Configuration")]
    public class ConfigurationCollection : ICollectionFixture<ConfigurationFixture>
    {

    }
}