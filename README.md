# VSTS Lab Accessor 2015

This is a helper library for users to access Microsoft VSTS Test Configurations for retrieving Variables
registered through VSTS for use in Test Runs.

## Getting Started

It's important to note that there are (generally) two contexts in which this library will be used:
1. In an Azure DevOps Pipelines Build / Release
2. On a developer's local workstation.

The following example demonstrates how to start using the library in your MSTest code specifically. There's
no reason that this library can't be adapted for use with XUnit, NUnit, or other test frameworks. Following the example is an explanation of how it works. The examples demonstrates how to set up the library for use when running on a Hosted build agent, and fallback to local developer credentials when it's running on a local developer workstation.

```
using VstsLabAccessor2015;

....
[TestClass]
public class MyTestSuite
{
    /// <summary>
    /// Gets the configuration variables retrieved from VSTS
    /// </summary>
    protected IDictionary<string, string> ConfigurationVariables { get; private set; }

    /// <summary>
    /// Gets or sets the context of the currently running test
    /// </summary>
    public TestContext TestContext;

    [TestInitialize]
    public async Task Initialize()
    {
        VstsEnvironmentVariables environmentVariables = VstsEnvironmentVariables.Create(this.TestContext.Properties);

        if (environmentVariables.IsValid)
        {
            var configurationName = Convert.ToString(TestContext.Properties["TestConfiguration"]);

            if (string.IsNullOrEmpty(configurationName))
            {
                throw new ConfigurationErrorsException("Failed to resolve TestConfiguration from test properties");
            }

            this.TestContext.WriteLine("Executing test '{0}' with TestConfiguration '{1}'", TestContext.TestName, configurationName);

            try
            {
                // We test whether or not we're running in a build. If not, we're almost certainly running 
                // locally, so use developer credentials.
                NetworkCredential networkCredential = ResolveCredentials(environmentVariables);

                this.ConfigurationVariables = await new VstsTestAccessor2015(networkCredential, 10).GetTestConfigurationPropertiesAsync(environmentVariables, configurationName);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Failed to retrieve Test Configuration Variables from VSTS", ex);
            }
        }
        else
        {
            throw new ConfigurationErrorsException("Failed to retrieve configuration variables from TestContext. If you're debugging in Visual Studio, ensure you've selected the correct .runsettings file from the Test -> Select Test Settings menu. If you're running in a VSTS vNext build / release, ensure that you've selected the correct .runsettings file in your \"VSTest\" step in the build / release.");
        }
    }

    /// <summary>
    /// Resolves the credentials to be used. If we're running in a build, then we don't need to provide any
    /// credentials: the test VstsTestAccessor2015 object will use the System.AccessToken parameter to use
    /// OAuth authentication to connect using the executor of the test. Otherwise, we're almost certainly
    /// running in a debug environment on a developer's machine, provide explicit credentials derived from
    /// environment variables. The values of these environment variables should be defined in the developer's
    /// User-level environment variables, and should be the Alternate Authentication credentials they've
    /// configured for their user in VSTS.
    /// </summary>
    /// <param name="environmentVariables">
    /// The environment variables resolved from VSTS. Must not be null.
    /// </param>
    /// <returns>
    /// A <see cref="NetworkCredential"/> instance to be used to authenticate for the tests. May be null,
    /// which is a valid value.
    /// </returns>
    private static NetworkCredential ResolveCredentials(VstsEnvironmentVariables environmentVariables)
    {
        if (environmentVariables == null)
        {
            throw new ArgumentNullException(nameof(environmentVariables));
        }

        if (environmentVariables.Tf.Build)
        {
            return null;
        }

        return new NetworkCredential(
            userName: Environment.GetEnvironmentVariable("VSTSALTERNATEAUTH_USERNAME"),
            password: Environment.GetEnvironmentVariable("VSTSALTERNATEAUTH_PASSWORD")
        );
    }

    [TestMethod]
    public void TestThings()
    {
        Assert.IsTrue(true, "Implement test logic here");
    }
}
```

The example above demonstrates how to configure a test suite that does the following:
1. Defines a `ConfigurationVariables` property that holds the Test Variables retrieved from Azure DevOps in the `Initialize()` method.
2. Defines an `Initialize()` method that uses the environment variables defined on Hosted agents in VSTS during a test run to build up a `VstsEnvironmentVariables` object that can be used to easily access the well known environment variables defined by VSTS. If the variables are `.IsValid`, then we use the `System.AccessToken` variable to connect to VSTS and retrieve the Variables defined in the Configuration of the current Test Run.
3. Defines an `ResolveCredentials()` helper method in case we're running on a local developer box and we want to use the developers credentials to access VSTS rather than the currently running Hosted build agent.

## Authentication

Referring to earlier in this documentation where the contexts for using this application are listed, there are two different ways of authenticating with VSTS in order to retrieve Configuration variables for your Test Run:
1. When running on a Hosted Azure Pipelines agent, the current Build / Release phase **MUST** have the "Allow access to System token" setting enabled. That authentication token will then be used to access Azure DevOps (VSTS).
2. When running on a local developer workstation, the library will authenticate using Alternate Authentication credentials (as demonstrated in the helper method).

## How It Works

### Pre-requisites

In order for this library to be useful and work properly, the VSTS Hosted agent environment variables must be made available in an `System.Collections.IDictionary`. At the time this library was originally written, it was meant for use on remote VMs with interactive UI tests, so the local environment of the Hosted agents wasn't available. To remedy that, the example above assumes that the environment variables get placed within the `.runsettings` file provided to MSTest to execute the tests on the remote machines and thereby make them available in the `TestContext.Properties` object. There's also a companion VSTS Extension meant to be used to perform that task available [here](https://marketplace.visualstudio.com/items?itemName=westpeaksconsulting.vsts-test-extensions) called "Inject VSTS Variables into Runsettings".

If you know your tests are going to be running directly on a Hosted VSTS agent rather than a remote VM, you can replace the line `VstsEnvironmentVariables environmentVariables = VstsEnvironmentVariables.Create(this.TestContext.Properties);` with `VstsEnvironmentVariables environmentVariables = VstsEnvironmentVariables.Create(Environment.GetEnvironmentVariables());` and save yourself having to use an extra step in your Build / Release pipeline.