using System.Diagnostics;

namespace Deflector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var configuration = ConfigurationLoader.Load("Configuration.json");
                var selector = new BrowserSelector(configuration);
                
                var browser = selector.SelectBrowser(args[0]);
                LaunchBrowser(browser);
            }
            else if (args.Length == 0)
            {
                ProtocolHandler.Register();
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
    }
}
