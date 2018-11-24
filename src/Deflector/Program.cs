using Microsoft.Extensions.Configuration;

namespace Deflector
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("Configuration.json", optional: false);

            var configuration = configurationBuilder.Build();
        }
    }
}
