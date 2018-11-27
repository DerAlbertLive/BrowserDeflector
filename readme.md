# Browser Deflector

Browser Deflector is registered as the default browser in the system, so that
every link (HTTP and HTTPS) is opened by Deflector. Then Deflector decides based on the domain name (can be more sophisticated later on) which browser (profile) should actually be started.

## Use Cases

### Software Development

If you are like me, then you may have different browser profiles defined in Chrome.
For example for different customers, different projects and so on.

But when you receive an e-mails for certain projects which links to the customers Azure DevOps, Jira,
TeamCity etc. your default browser will be openend instead of the correct
browser with the right profile which has the correct cookies and plugins installed.

This also works with links within IDEs which then will open in the correct browser.

### Freelance works with different Customers

Same as Software Development, without IDE. ;-)

### Web Site Compatiblity

If you still have web sites which only run in IE 11 for reasons, then this might
be a tool for you.

## Installation

Copy compiled application output `Deflector.exe`, `Newtonsoft.Json.dll` and the `Configuration.json` into a folder of your choice.

Double click the `.exe`. You will be asked if the application should be registerd as a
browser. "Yes" is the right answer. A User Account Control prompt will appear. After
that you have set the default browser in the Default App Settings. The `.exe` will open
this for you.

If it is installed, you can also double click the app to remove the browser registration.

## Configuration

In the `Configuration.json` you can register destinations. When a URI starts with the destination,
then the configured browser will be started. Take a look at the [TestConfiguration.json](https://github.com/DerAlbertCom/BrowserDeflector/blob/develop/tests/Deflector.Tests/TestConfiguration.json)
for some examples.

TODO: Better Docs here. ;)

## Licenses

The program icon is derived from [https://svgsilh.com/image/773215.html](https://svgsilh.com/image/773215.html)
