using System;

namespace Deflector
{
    public class DestinationDefinition
    {
        public DestinationDefinition()
        {
            Parameters = Array.Empty<object>();
        }

        public string StartUrl { get; set; }

        public string Browser { get; set; }

        public object[] Parameters { get; set; }
    }
}