using System;
using System.Collections.Generic;
using System.Linq;

namespace Deflector
{
    public class BrowserOpenerConfiguration
    {
        public IDictionary<string, BrowserDefinition> Browsers { get; set; } = new Dictionary<string, BrowserDefinition>();

        public BrowserDefinition DefaultBrowser
        {
            get
            {
                if (Default == null && Browsers.Count == 0)
                {
                    throw new InvalidOperationException("There are no browsers defined");
                }

                if (Default == null)
                {
                    return Browsers.First().Value;
                }

                if (Browsers.TryGetValue(Default.Browser, out var browserDefinition))
                {
                    return browserDefinition;
                }

                throw new InvalidOperationException($"The Browser '{Default.Browser}' is not defined");
            }
        }

        public DestinationDefinition Default { get; set; }

        public DestinationDefinition[] Destinations { get; set; } = Array.Empty<DestinationDefinition>();
    }
}