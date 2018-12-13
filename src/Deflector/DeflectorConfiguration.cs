using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Deflector
{
    public class DeflectorConfiguration
    {
        public DeflectorConfiguration()
        {
            Browsers = new Dictionary<string, BrowserDefinition>();
            Destinations = Array.Empty<DestinationDefinition>();
        }

        public Dictionary<string, BrowserDefinition> Browsers { get; set; }

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

        public DestinationDefinition[] Destinations { get; set; }
    }
}