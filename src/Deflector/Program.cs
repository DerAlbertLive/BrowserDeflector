using System;
using System.Diagnostics;

namespace Deflector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {

                var configuration = new ConfigurationLoader().LoadConfiguration("Configuration.json");
                var selector = new BrowserSelector(configuration);
                var browser = selector.SelectBrowser(args[0]);
                OpenUri(browser);
            }
            else if (args.Length == 0)
            {
                new RegisterHandler().Register();
            }
        }

           
        static void OpenUri((string filename, string arguments) browser)
        {
            var launcher = new ProcessStartInfo()
            {
                FileName = browser.filename,
                Arguments = browser.arguments,
                UseShellExecute = browser.arguments == null
            };
            Process.Start(launcher);
        }
    }
}
