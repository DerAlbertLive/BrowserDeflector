using FluentAssertions;
using Xunit;

namespace Deflector.Tests
{
    public class BrowserSelectorTests : IClassFixture<ConfigurationFixture>
    {
        readonly BrowserSelector _selector;

        public BrowserSelectorTests(ConfigurationFixture fixture)
        {
            _selector = new BrowserSelector(fixture.Configuration);
        }

        [Fact]
        public void Http_CompanyUrl_should_resolve_chrome()
        {
            var browser = _selector.SelectBrowser("http://company.visualstudio.com/dingel");

            browser.filename.Should().EndWith("chrome.exe");
        }

        [Fact]
        public void Https_CompanyUrl_should_resolve_chrome()
        {
            var browser = _selector.SelectBrowser("https://company.visualstudio.com");

            browser.filename.Should().EndWith("chrome.exe");
        }

        [Fact]
        public void Http_CompanyUrl_should_resolve_profile1()
        {
            var browser = _selector.SelectBrowser("http://company.visualstudio.com/dingel?23=1");

            browser.arguments.Should().Be("--profile-directory=\"Profile 1\" http://company.visualstudio.com/dingel?23=1");
        }
    }
}