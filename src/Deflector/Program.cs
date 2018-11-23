using System;
using Microsoft.Extensions.Configuration;

namespace BrowserOpener
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
