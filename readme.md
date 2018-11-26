# Browser Deflector

Browser Deflector will be registerd as the Default Browser in the System. So that
every link (http and https) will be open by Deflector, then Deflector decides on
die Domainname (or later be more sophisticated) which browser to start for real.

## Use Cases

### Software Development

If you are like me, then you may have different Browser Profiles defined in Chrome,
for different customeres, different projects or whatever.

But when you get mail for that projects which links to the customers Azure DevOps, Jira,
Teamcity or what else then your default browser will be openend instead of the correct
browser with the right profile which has the correct cookies and plugins installed.

This also works with Links within IDEs which then will opens in the correct browser

### Freelance works with different Customers

Same a Software Development, without IDE ;-)

### WebSite Compatiblity

If you have still use Websites which only runs in IE 11 for reasons, then this might
be a tool for you.


## Installation

Copy compiled Application Output `Deflector.exe`, `Newtonsoft.Json.dll` and the `Configuration.json` 
in a folder of your choice.

Double click the .exe, you will be asked if the application should be registerd as a
browser. Yes is the right answer. An User Account Control prompt will appear. After
the you have set the default Browser in the Default App Settings. The .exe will open
this for you.

If it is installed, you can also double click the app to remove the Browser Registration.

## Configuration

In the `Configuration.json` you can register destination, when an Uri start with the destination
then the configured browser will be started. Take a look at the [TestConfiguration.json](https://github.com/DerAlbertCom/BrowserDeflector/blob/develop/tests/Deflector.Tests/TestConfiguration.json)
for some examples.

TODO: Better Docs here ;)

## Licenses

The Programm Icon is derived from https://svgsilh.com/image/773215.html
