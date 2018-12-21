using System;
using System.Diagnostics;


namespace Deflector
{
    class Program
    {
        const string BrowserDeflector = "Browser Deflector";

        static void Main(string[] args)
        {
    

            if (ShouldLaunchBrowser(args))
            {
                var configuration = ConfigurationLoader.Load("Configuration.json");
                var selector = new BrowserSelector(configuration);
                
                var browser = selector.SelectBrowser(args[0]);
                LaunchBrowser(browser);
            }
            else
            {
                Environment.Exit(1);
            }
        }

        static void LaunchBrowser(Browser browser)
        {
            var launcher = new ProcessStartInfo()
            {
                FileName = browser.Filename,
                Arguments = browser.Arguments,
                UseShellExecute = browser.Arguments == null // without arguments we have a protocol handler only uri, these must be started with ShellExecute
            };
            Process.Start(launcher);
        }

        static bool ShouldLaunchBrowser(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }
            var uri = args[0];

            try
            {
                var a = new Uri(uri);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine($"The Uri: '{uri}' could not be opened", BrowserDeflector);
                return false;
            }
        }
    }
}
