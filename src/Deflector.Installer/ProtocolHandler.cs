using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;

namespace Deflector.Installer
{
    // based on https://github.com/da2x/EdgeDeflector/blob/master/EdgeDeflector/Program.cs

    internal class ProtocolHandler
    {
        public const string InstallAsBrowser = "--install-browser";
        public const string UninstallAsBrowser = "--uninstall-browser";

        ProtocolHandler()
        {
            
        }

        bool IsElevated()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        void ElevatePermissions(string arguments)
        {
            var rerun = new ProcessStartInfo()
            {
                FileName = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Arguments = arguments,
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(rerun);
        }

        void RemoveClassRoot(string applicationName)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(applicationName, false);
        }

        bool IsRegistered(string applicationName)
        {
            return Registry.ClassesRoot.OpenSubKey(applicationName, false) != null;
        }

        void RegisterClassRoot(string applicationName)
        {
            var execPath = GetDeflectorPath();
            using (var classRegistryKey = Registry.ClassesRoot.OpenOrCreateSubKey(applicationName))
            {
                classRegistryKey.SetValue(string.Empty, $"URL: {applicationName}");

                using (var defaultIconKey = classRegistryKey.OpenOrCreateSubKey("DefaultIcon"))
                {
                    defaultIconKey.SetValue(string.Empty, execPath + ",0");
                }

                using (var openCommandKey = classRegistryKey.OpenOrCreateSubKey(@"shell\open\command"))
                {
                    openCommandKey.SetValue(string.Empty, execPath + " \"%1\"");
                }
                classRegistryKey.SetValue("URL Protocol", string.Empty);
            }
        }

        private static string GetDeflectorPath()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            return Path.Combine(directory, "Deflector.exe");
        }

        void RemoveHttpProtocolHandler(string applicationName)
        {
            var appKeyPath = $@"SOFTWARE\Clients\StartMenuInternet\{applicationName}";
            Registry.LocalMachine.DeleteSubKeyTree(appKeyPath, false);
        }

        void RegisterHttpProtocolHandler(string applicationName)
        {
            var appKeyPath = $@"SOFTWARE\Clients\StartMenuInternet\{applicationName}";
            var capabilitiesKeyPath = $@"SOFTWARE\Clients\StartMenuInternet\{applicationName}\Capabilities";

            var execPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using (var appKey = Registry.LocalMachine.OpenOrCreateSubKey(appKeyPath))
            {
                appKey.SetValue(string.Empty, applicationName);
                using (var defaultIconKey = appKey.OpenOrCreateSubKey("DefaultIcon"))
                {
                    defaultIconKey.SetValue(string.Empty, execPath + ",0");
                }

                using (var openCommandKey = appKey.OpenOrCreateSubKey(@"shell\open\command"))
                {
                    openCommandKey.SetValue(string.Empty, execPath + " \"%1\"");
                }

                using (var capabilityKey = appKey.OpenOrCreateSubKey("Capabilities"))
                {
                    capabilityKey.SetValue("ApplicationDescription", "Open specific web links within configured Browsers");
                    capabilityKey.SetValue("ApplicationName", applicationName);
                    capabilityKey.SetValue("ApplicationIcon",  execPath + ",0");

                    using (var urlAssociationsKey = capabilityKey.OpenOrCreateSubKey("UrlAssociations"))
                    {
                        urlAssociationsKey.SetValue("http", applicationName);
                        urlAssociationsKey.SetValue("https", applicationName);
                    }
                }
            }

            var registeredAppKey = Registry.LocalMachine.OpenOrCreateSubKey(@"SOFTWARE\RegisteredApplications");
            registeredAppKey.SetValue(applicationName, capabilitiesKeyPath);
        }

        const string ApplicationName = "Browser Deflector";

        public static bool IsInstalled()
        {
            var handler = new ProtocolHandler();
            return handler.IsRegistered(ApplicationName);
        }

        public static void Uninstall()
        {
            var handler = new ProtocolHandler();
            if (!handler.IsElevated())
            {
                handler.ElevatePermissions(UninstallAsBrowser);
            }
            else
            {
                handler.RemoveClassRoot(ApplicationName);
                handler.RemoveHttpProtocolHandler(ApplicationName);
            }
        }

        public static void Install()
        {
            var handler = new ProtocolHandler();
            if (!handler.IsElevated())
            {
                handler.ElevatePermissions(InstallAsBrowser);
            }
            else
            {

                handler.RegisterClassRoot(ApplicationName);
                handler.RegisterHttpProtocolHandler(ApplicationName);
            }
        }
    }
}