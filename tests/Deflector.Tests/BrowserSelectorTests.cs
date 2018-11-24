using Xunit;

namespace Deflector.Tests
{
    public class BrowserSelectorTests : IClassFixture<ConfigurationFixture>
    {
        BrowserSelector _selector;

        public BrowserSelectorTests(ConfigurationFixture fixture)
        {

            _selector = new BrowserSelector(fixture.Configuration);
        }
    }
}