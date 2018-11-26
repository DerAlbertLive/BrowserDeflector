using System;

namespace Deflector
{
    public class BrowserSelector
    {
        readonly DeflectorConfiguration _configuration;

        public BrowserSelector(DeflectorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string filename, string arguments) SelectBrowser(string url)
        {
            var uri = new Uri(url);
            var destinationDefinition = FindDestination(uri);

            if (_configuration.Browsers.TryGetValue(destinationDefinition.Browser, out var browser))
            {
                var arguments = browser.GetArguments(destinationDefinition.Parameters);
                if (IsFileNameOnly(browser))
                {
                    return ($"{browser.Path}{url}", null);
                }
                return (browser.Path, $"{arguments} {url}");
            }
            return (null, null);
        }

        bool IsFileNameOnly(BrowserDefinition browser)
        {
            return string.Equals(browser.Path, "microsoft-edge:", StringComparison.OrdinalIgnoreCase);
        }

        DestinationDefinition FindDestination(Uri uri)
        {
            DestinationDefinition destinationDefinition = _configuration.Default;

            var domainAndPath = $"{uri.Host}{uri.AbsoluteUri}";
            foreach (var definition in _configuration.Destinations)
            {
                if (domainAndPath.StartsWith(definition.StartUrl))
                {
                    destinationDefinition = definition;
                    break;
                }
            }

            return destinationDefinition;
        }
    }
}