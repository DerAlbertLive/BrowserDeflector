using System;

namespace Deflector
{
    internal class BrowserSelector
    {
        readonly DeflectorConfiguration _configuration;

        public BrowserSelector(DeflectorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Browser SelectBrowser(string url)
        {
            var domainName = RemoveProtocol(url);

            var destinationDefinition = FindDestination(domainName);

            if (_configuration.Browsers.TryGetValue(destinationDefinition.Browser, out var browser))
            {
                var arguments = browser.GetArguments(destinationDefinition.Parameters);
                if (IsFileNameOnly(browser))
                {
                    return new Browser($"{browser.Path}{url}", null);
                }
                return new Browser(browser.Path, $"{arguments} {url}");
            }
            return new Browser(null, null);
        }

        string RemoveProtocol(string url)
        {
            var uri = new Uri(url);
            return url.Substring(uri.Scheme.Length + 3);
        }

        bool IsFileNameOnly(BrowserDefinition browser)
        {
            return string.Equals(browser.Path, "microsoft-edge:", StringComparison.OrdinalIgnoreCase);
        }

        DestinationDefinition FindDestination(string domainName)
        {
            DestinationDefinition destinationDefinition = _configuration.Default;

            foreach (var definition in _configuration.Destinations)
            {
                if (domainName.StartsWith(definition.StartUrl))
                {
                    destinationDefinition = definition;
                    break;
                }
            }

            return destinationDefinition;
        }
    }
}