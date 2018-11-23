using FluentAssertions;
using Xunit;

namespace BrowserOpener.Tests
{
    public class ConfigurationDefinitionTests : IClassFixture<ConfigurationFixture>
    {
        readonly BrowserOpenerConfiguration _configuration;

        public ConfigurationDefinitionTests(ConfigurationFixture fixture)
        {
            _configuration = fixture.Configuration;
        }

        [Fact]
        public void Should_have_three_browserDefinitions()
        {
            _configuration.Browsers.Count.Should().Be(3);
        }

        [Theory]
        [InlineData("chrome")]
        [InlineData("edge")]
        [InlineData("firefox")]
        public void Should_contain_browser_definition(string browser)
        {
            _configuration.Browsers.ContainsKey(browser).Should().BeTrue();
        }

        [Fact]
        public void Should_the_pass_from_the_browser_set()
        {
            var browser = _configuration.Browsers["edge"];
            browser.Path.Should().Be("microsoft-edge:");
        }

        [Fact]
        public void Should_the_parameter_format_set()
        {
            var browser = _configuration.Browsers["firefox"];
            browser.ParameterFormat.Should().Be("-P={0}");
        }

        [Fact]
        public void Should_the_parameter_set()
        {
            var browser = _configuration.Browsers["edge"];
            browser.Parameter.Should().Be("option1");
        }

        [Fact]
        public void Should_default_browser_is_set()
        {
            _configuration.Default.Browser.Should().Be("edge");
        }

        [Fact]
        public void Should_default_browser_should_be_discovered()
        {
            _configuration.DefaultBrowser.Path.Should().Be("microsoft-edge:");
        }

        [Fact]
        public void Should_contain_two_destinations()
        {
            _configuration.Destinations.Length.Should().Be(2);
        }

        [Fact]
        public void Should_map_destination_definition_properties()
        {
            var dest = _configuration.Destinations[0];

            dest.Browser.Should().Be("chrome", "browser not bound");
            
            dest.StartUrl.Should().Be("company.visualstudio.com", "startUrl not bound");
            
            dest.Parameters[0].Should().Be("Profile 1", "parameters not bound");
        }
    }
}