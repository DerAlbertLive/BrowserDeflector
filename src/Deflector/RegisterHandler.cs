using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;

namespace Deflector
{
    // based on https://github.com/da2x/EdgeDeflector/blob/master/EdgeDeflector/Program.cs
    public class RegisterHandler
    {
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

        void RegisterProtocolHandler()
        {
            var applicationName = "Browser Deflector";
            var deflectorSubKey = $@"SOFTWARE\Clients\StartMenuInternet\{applicationName}";
            var deflectorCapabilities = $@"{deflectorSubKey}\Capabilities";

            var registeredApplications = @"SOFTWARE\RegisteredApplications";
            var shellOpenCommand = $@"shell\open\command";
            var execPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var uriClassSubKey = Registry.ClassesRoot.OpenOrCreateSubKey(applicationName);

            uriClassSubKey.SetValue(string.Empty, $"URL: {applicationName}");

            var iconKey = uriClassSubKey.OpenOrCreateSubKey("DefaultIcon");


            iconKey.SetValue(string.Empty, execPath + ",0");
            iconKey.Close();

            var shellCmdKey = uriClassSubKey.OpenOrCreateSubKey(shellOpenCommand);

            shellCmdKey.SetValue(string.Empty, execPath + " \"%1\"");
            shellCmdKey.Close();

            uriClassSubKey.SetValue("URL Protocol", string.Empty);

            uriClassSubKey.Close();

            var softwareKey = Registry.LocalMachine.OpenOrCreateSubKey(deflectorSubKey);

            var defaultIconKey = softwareKey.OpenOrCreateSubKey("DefaultIcon");
            defaultIconKey.SetValue(string.Empty, execPath + ",0");
            defaultIconKey.Close();

            var openShellCommand = softwareKey.OpenOrCreateSubKey(shellOpenCommand);
            openShellCommand.SetValue(string.Empty, execPath + " \"%1\"");
            openShellCommand.Close();


            RegistryKey capabilityKey = softwareKey.OpenOrCreateSubKey("Capabilities");

            capabilityKey.SetValue("ApplicationDescription", "Open specific web links with configured Browsers");
            capabilityKey.SetValue("ApplicationName", applicationName);
            capabilityKey.SetValue("ApplicationIcon",  execPath + ",0");
            

            var urlAssociationsKey = capabilityKey.OpenOrCreateSubKey("UrlAssociations");

            urlAssociationsKey.SetValue("http", applicationName);
            urlAssociationsKey.SetValue("https", applicationName);
            urlAssociationsKey.Close();

            capabilityKey.Close();
            softwareKey.Close();

            var registeredAppKey = Registry.LocalMachine.OpenOrCreateSubKey(registeredApplications);
            registeredAppKey.SetValue(applicationName, deflectorCapabilities);
            registeredAppKey.Close();
        }

        public void Register()
        {
            if (!IsElevated())
            {
                ElevatePermissions();
            }
            else
            {
                RegisterProtocolHandler();
            }
        }
    }
}