using System.IO;
using Newtonsoft.Json;

namespace Deflector
{
    public class ConfigurationLoader
    {
        public DeflectorConfiguration LoadConfiguration(string path)
        {
            if (!File.Exists(path))
            {
                var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var execPath = executingAssembly.Location;
                path = Path.Combine(Path.GetDirectoryName(execPath), path);
            }
            using (var reader = File.OpenText(path))
            {
                var serializer =  new JsonSerializer();
                return serializer.Deserialize<DeflectorConfiguration>(new JsonTextReader(reader));
            }
            return new DeflectorConfiguration();
        }
    }
}