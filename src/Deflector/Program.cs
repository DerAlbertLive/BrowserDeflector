using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Deflector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ShouldLaunchBrowser(args))
            {
                var configuration = ConfigurationLoader.Load("Configuration.json");
                var selector = new BrowserSelector(configuration);
                
                var browser = selector.SelectBrowser(args[0]);
                LaunchBrowser(browser);
            }
            else if (ShouldUninstall(args))
            {
                ProtocolHandler.Uninstall();
                MessageBox.Show("Browser Deflector removed as a browser,\r\n now you must set an other default browser", "Browser Deflector", MessageBoxButtons.OK);
                LaunchBrowser(new Browser("ms-settings:defaultapps", null));
            }
            else if (ShouldInstall(args))
            {
                ProtocolHandler.Install();
                MessageBox.Show("Browser Deflector is now registered as a browser,\r\n now you must set deflector as Default Browser", "Browser Deflector", MessageBoxButtons.OK);
                LaunchBrowser(new Browser("ms-settings:defaultapps", null));
            }

            else if (args.Length == 0)
            {
                if (ProtocolHandler.IsInstalled())
                {
                    Uninstall();
                }
                else
                {
                    Install();
                }
            }
        }

        static void Uninstall()
        {
            var result =
                MessageBox.Show(
                    "Browser Deflector is registered as a browser.\r\nWould you like to remove it?\r\n\r\nan UAC prompt will appear.",
                    "Browser Deflector", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ProtocolHandler.Uninstall();
            }
        }

        static void Install()
        {
            var result =
                MessageBox.Show(
                    "Browser Deflector has to register themselves as a browser.\r\nWould you like to do that now?\r\n\r\nan UAC prompt will appear.",
                    "Browser Deflector", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ProtocolHandler.Install();
            }
        }

        static bool ShouldUninstall(string[] args)
        {
            return args.Length == 1 && args[0] == ProtocolHandler.UninstallAsBrowser;
        }

        static bool ShouldInstall(string[] args)
        {
            return args.Length == 1 && args[0] == ProtocolHandler.InstallAsBrowser;
        }

        static bool ShouldLaunchBrowser(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }
            var command = args[0];
            return args.Length == 1 && (command != ProtocolHandler.InstallAsBrowser && command != ProtocolHandler.UninstallAsBrowser);
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
