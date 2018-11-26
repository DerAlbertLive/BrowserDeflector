namespace Deflector
{
    public class Browser
    {
        public Browser(string filename, string arguments)
        {
            Filename = filename;
            Arguments = arguments;
        }

        public string Filename { get; }
        public string Arguments { get; }
    }
}