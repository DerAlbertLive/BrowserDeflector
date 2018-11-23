using FluentAssertions;
using Xunit;

namespace BrowserOpener.Tests
{
    public class BrowserDefinitionParameterFormatsTests
    {
        [Fact]
        public void With_chrome_as_browser_default_parameters_format_is_for_profile()
        {
            var browser = new BrowserDefinition()
            {
                Path = "C:\\ding\\dong\\chRomE.exe"
            };

            browser.ParameterFormat.Should().Be("--profile-directory-directory=\"{0}\"");
        }

        [Fact]
        public void With_chrome_as_browser_when_format_is_set_parameters_format_is_for_profile()
        {
            var browser = new BrowserDefinition()
            {
                Path = "C:\\ding\\dong\\chRomE.exe",
                ParameterFormat = "theFormat"
            };

            browser.ParameterFormat.Should().Be("theFormat");
        }

        [Fact]
        public void With_firefox_as_browser_when_format_is_set_parameters_format_is_for_profile()
        {
            var browser = new BrowserDefinition()
            {
                Path = "C:\\ding\\dong\\FireFox.exe",
                ParameterFormat = "aFormat"
            };

            browser.ParameterFormat.Should().Be("aFormat");
        }

        [Fact]
        public void With_firefox_as_browser_default_parameters_format_is_for_profile()
        {
            var browser = new BrowserDefinition()
            {
                Path = "C:\\ding\\dong\\firEfox.exe"
            };

            browser.ParameterFormat.Should().Be("-P \"{0}\"");
        }
    }
}