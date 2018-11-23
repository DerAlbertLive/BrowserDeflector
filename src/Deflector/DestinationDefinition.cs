using System;

namespace BrowserOpener
{
    public class DestinationDefinition
    {
        public DestinationDefinition()
        {
            Parameters = Array.Empty<string>();
        }

        public string StartUrl { get; set; }

        public string Browser { get; set; }

        public string[] Parameters { get; set; }
    }
}