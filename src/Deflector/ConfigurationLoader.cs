using System.IO;
using System.Web.Script.Serialization;

namespace Deflector
{
    public class ConfigurationLoader
    {
        public static DeflectorConfiguration Load(string path)
        {
            if (!File.Exists(path))
            {
                var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var execPath = executingAssembly.Location;
                path = Path.Combine(Path.GetDirectoryName(execPath), path);
            }

            if (File.Exists(path))
            {
                var serializer = new JavaScriptSerializer();

                var allJson = File.ReadAllText(path);
                return serializer.Deserialize<DeflectorConfiguration>(allJson);
            }

            return new DeflectorConfiguration();
        }
    }
}