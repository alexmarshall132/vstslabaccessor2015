
namespace VstsLabAccessor2015.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	/// <summary>
	/// Test suite for the <see cref="VstsTestAccessor2015"/> class.
	/// </summary>
	[TestClass]
	public class VstsTestAccessor2015Tests
	{
		/// <summary>
		/// The subject under test
		/// </summary>
		private VstsTestAccessor2015 sut;

		#region Test Lifecycle Methods

		[TestInitialize]
		public void InitializeTest()
		{
			string userName = Environment.GetEnvironmentVariable("VSTSTESTACCESSOR2015_USERNAME");
			string password = Environment.GetEnvironmentVariable("VSTSTESTACCESSOR2015_PASSWORD");

			this.sut = new VstsTestAccessor2015(new NetworkCredential(userName, password), 10);
		}

		[TestCleanup]
		public void CleanupTest()
		{
			this.sut = null;
		} 

		#endregion

		/// <summary>
		/// Verifies that the subject under test can correctly query and parse the VSTS REST Test Management API
		/// </summary>
		/// <returns>
		/// A <see cref="Task"/> for executing this test asynchronously.
		/// </returns>
		[TestMethod]
		public async Task VerifyQueryIsCorrectlyParsed()
		{
			var values = new Dictionary<string, string>
			{
				{ "SYSTEM_TEAMFOUNDATIONCOLLECTIONURI", Environment.GetEnvironmentVariable("VSTSTESTACCESSOR2015_TEAMPROJECTCOLLECTIONURI") },
				{ "SYSTEM_TEAMPROJECT", Environment.GetEnvironmentVariable("VSTSTESTACCESSOR2015_TEAMPROJECT") }
			};

			VstsEnvironmentVariables variables = VstsEnvironmentVariables.Create(values);

			IDictionary<string, string> configValues = await this.sut.GetTestConfigurationPropertiesAsync(variables, "Windows 8");

			Assert.AreNotEqual(configValues, "The configuration values dictionary must not be null");
			Assert.AreEqual(1, configValues.Count, "Unexpected number of configuration values");
			Assert.AreEqual("Operating System", configValues.Single().Key, "Unexpected value key");
			Assert.AreEqual("Windows 8", configValues.Single().Value, "Unexpected configuration value");
		}
	}
}
