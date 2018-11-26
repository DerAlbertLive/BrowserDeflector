using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Deflector
{
    class Program
    {
        const string BrowserDeflector = "Browser Deflector";

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
                var result = MessageBox.Show($"{BrowserDeflector} removed as a browser,\r\nYou must set an other default Web browser.\r\n\r\nIf you click ok, the Default Apps Settings will be opened", BrowserDeflector, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    LaunchBrowser(new Browser("ms-settings:defaultapps", null));
                }
            }
            else if (ShouldInstall(args))
            {
                ProtocolHandler.Install();
                var result =MessageBox.Show($"{BrowserDeflector} is now registered as a browser,\r\nYou must set 'Deflector' as default Web Browser.\r\n\r\nIf you click ok, the Default Apps Settings will be opened", BrowserDeflector, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    LaunchBrowser(new Browser("ms-settings:defaultapps", null));
                }
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
            else
            {
                Environment.Exit(1);
            }
        }

        static void Uninstall()
        {
            var result =
                MessageBox.Show(
                    $"{BrowserDeflector} is registered as a browser.\r\nWould you like to remove it?\r\n\r\nAn UAC prompt will appear.",
                    BrowserDeflector, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProtocolHandler.Uninstall();
            }
        }

        static void Install()
        {
            var result =
                MessageBox.Show(
                    $"{BrowserDeflector} has to register themselves as a browser.\r\nWould you like to do that now?\r\n\r\nAn UAC prompt will appear.",
                    BrowserDeflector, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            var uri = args[0];
            if (uri == ProtocolHandler.InstallAsBrowser || uri == ProtocolHandler.UninstallAsBrowser)
            {
                return false;
            }

            try
            {
                var a = new Uri(uri);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"The Uri: '{uri}' could not be opened", BrowserDeflector, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
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
