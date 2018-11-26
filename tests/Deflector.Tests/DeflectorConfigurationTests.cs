using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Deflector.Tests
{
    public class DeflectorConfigurationTests
    {
        readonly DeflectorConfiguration _configuration;

        public DeflectorConfigurationTests()
        {
            _configuration = new DeflectorConfiguration();
        }


        [Fact]
        public void With_no_browsers_and_no_default_browser_should_throw_exception()
        {
            Action action = () =>
            {
                var b = _configuration.DefaultBrowser;
            };

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void With_browsers_and_no_default_browser_should_the_first_browser_return()
        {

            _configuration.Browsers.Add("ding", new BrowserDefinition());

            var browser = _configuration.DefaultBrowser;

            browser.Should().BeSameAs(_configuration.Browsers.First().Value);
        }

        [Fact]
        public void With_default_browser_the_browser_should_be_return()
        {
            var dingBrowser = new BrowserDefinition();
            var dongBrowser = new BrowserDefinition();

            _configuration.Browsers.Add("ding", dingBrowser);
            _configuration.Browsers.Add("dong", dongBrowser);

            _configuration.Default = new DestinationDefinition()
            {
                Browser = "dong"
            };

            var browser = _configuration.DefaultBrowser;

            browser.Should().BeSameAs(dongBrowser);
        }

        [Fact]
        public void With_unknown_default_an_exception_should_be_thrown()
        {
            var dingBrowser = new BrowserDefinition();
            var dongBrowser = new BrowserDefinition();

            _configuration.Browsers.Add("ding", dingBrowser);
            _configuration.Browsers.Add("dong", dongBrowser);

            _configuration.Default = new DestinationDefinition()
            {
                Browser = "dang"
            };

            Action a = () =>
            {
                var configurationDefaultBrowser = _configuration.DefaultBrowser;
            };

            a.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void With_default_and_no_defined_browsers()
        {

            _configuration.Default = new DestinationDefinition()
            {
                Browser = "dang"
            };

            Action a = () =>
            {
                var configurationDefaultBrowser = _configuration.DefaultBrowser;
            };

            a.Should().Throw<InvalidOperationException>();
        }

    }
}