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
            var urlWithoutScheme = RemoveProtocol(url);

            var destinationDefinition = FindDestination(urlWithoutScheme);

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


        DestinationDefinition FindDestination(string url)
        {
            DestinationDefinition destinationDefinition = _configuration.Default;

            foreach (var definition in _configuration.Destinations)
            {
                var alternateStartUrl = GetAlternateStartUrl(definition.StartUrl);

                if (url.StartsWith(definition.StartUrl,StringComparison.OrdinalIgnoreCase) 
                    || url.StartsWith(alternateStartUrl, StringComparison.OrdinalIgnoreCase))
                {
                    destinationDefinition = definition;
                    break;
                }
            }

            return destinationDefinition;
        }

        private static string GetAlternateStartUrl(string url)
        {
            const string www = "www.";
            if (url.StartsWith(www, StringComparison.OrdinalIgnoreCase))
            {
                return url.Substring(www.Length);
            }
            return www + url;
        }
    }
}