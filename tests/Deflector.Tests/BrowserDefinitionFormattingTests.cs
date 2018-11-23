using System;
using FluentAssertions;
using Xunit;

namespace BrowserOpener.Tests
{
    public class BrowserDefinitionFormattingTests
    {
        BrowserDefinition _browser;

        public BrowserDefinitionFormattingTests()
        {
            _browser = new BrowserDefinition()
            {
                Path = "c:\\browser.exe",
                ParameterFormat = "-p {0}"
            };
        }

        [Fact]
        public void WithEmpty_Parameters_only_path_is_return()
        {
            var path = _browser.GetFullPath(new string[0]);
            path.Should().Be("c:\\browser.exe");
        }

        [Fact]
        public void With_parameter_paramter_should_the_parameter_be_included()
        {
            var path = _browser.GetFullPath(new[] {"P1"});

            path.Should().Be("c:\\browser.exe -p P1");
        }

        [Fact]
        public void With_given_parameter_should_the_parameter_format_be_ignored()
        {
            _browser.Parameter = "TheParameters";

            var path = _browser.GetFullPath(new[] {"P1"});

            path.Should().Be("c:\\browser.exe TheParameters");
        }

    }
}