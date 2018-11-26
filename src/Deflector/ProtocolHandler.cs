using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;

namespace Deflector
{
    // based on https://github.com/da2x/EdgeDeflector/blob/master/EdgeDeflector/Program.cs
    public class ProtocolHandler
    {
        ProtocolHandler()
        {
            
        }

        bool IsElevated()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        void ElevatePermissions()
        {
            var rerun = new ProcessStartInfo()
            {
                FileName = System.Reflection.Assembly.GetExecutingAssembly().Location,
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(rerun);
        }

        void RegisterClassRoot(string applicationName)
        {
            var execPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
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

        void RegisterHttpProtocolHandler(string applicationName)
        {
            var appKeyPath = $@"SOFTWARE\Clients\StartMenuInternet\{applicationName}";
            var capabilitiesKeyPath = $@"SOFTWARE\Clients\StartMenuInternet\{applicationName}\Capabilities";

            var execPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using (var appKey = Registry.LocalMachine.OpenOrCreateSubKey(appKeyPath))
            {
                using (var defaultIconKey = appKey.OpenOrCreateSubKey("DefaultIcon"))
                {
                    defaultIconKey.SetValue(string.Empty, execPath + ",0");
                }

                using (var openCommandKey = appKey.OpenOrCreateSubKey(@"shell\open\command"))
                {
                    openCommandKey.SetValue(string.Empty, execPath + " \"%1\"");
                    openCommandKey.Close();
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
                        urlAssociationsKey.Close();
                    }

                    capabilityKey.Close();
                }
            }

            var registeredAppKey = Registry.LocalMachine.OpenOrCreateSubKey(@"SOFTWARE\RegisteredApplications");
            registeredAppKey.SetValue(applicationName, capabilitiesKeyPath);
            registeredAppKey.Close();
        }

        public static void Register()
        {
            var handler = new ProtocolHandler();
            if (!handler.IsElevated())
            {
                handler.ElevatePermissions();
            }
            else
            {
                const string applicationName = "Browser Deflector";

                handler.RegisterClassRoot(applicationName);
                handler.RegisterHttpProtocolHandler(applicationName);
            }
        }
    }
}