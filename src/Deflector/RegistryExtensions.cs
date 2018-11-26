using Microsoft.Win32;

namespace Deflector
{
    public static class RegistryExtensions
    {
        public static RegistryKey OpenOrCreateSubKey(this RegistryKey key, string subKey)
        {
            var shellCmdKey = key.OpenSubKey(subKey, true) ?? key.CreateSubKey(subKey);

            return shellCmdKey;
        }
    }
}