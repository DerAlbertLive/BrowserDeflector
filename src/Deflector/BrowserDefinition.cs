using System;

namespace Deflector
{
    public class BrowserDefinition
    {
        public string Path { get; set; }

        public string Parameter { get; set; }

        public string ParameterFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_parameterFormat))
                {
                    return BrowserBasedDefaultFormat(Path);
                }

                return _parameterFormat;
            }
            set => _parameterFormat = value;
        }

        public string GetFullPath(object[] strings)
        {
            return $"{Path} {GetArguments(strings)}".Trim();
        }

        public string GetArguments(object[] strings)
        {
            if (!string.IsNullOrWhiteSpace(Parameter))
            {
                return Parameter;
            }

            if (!string.IsNullOrWhiteSpace(ParameterFormat) && strings.Length > 0)
            {
                return string.Format(ParameterFormat, strings);
            }

            return string.Empty;
        }

        string BrowserBasedDefaultFormat(string path)
        {
            if (path.IndexOf("chrome.exe", StringComparison.OrdinalIgnoreCase) > 0)
            {
                return "--profile-directory=\"{0}\"";
            }

            if (path.IndexOf("firefox.exe", StringComparison.OrdinalIgnoreCase) > 0)
            {
                return "-P \"{0}\"";
            }

            return "{0}";
        }

        string _parameterFormat;
    }
}