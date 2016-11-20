namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for objects that access the VSTS test facility
	/// </summary>
	interface IVstsTestAccessor2015
	{
		/// <summary>
		/// Gets the test configuration variables for the specified environment
		/// </summary>
		/// <param name="env">
		///     The current environment. Must not be null.
		/// </param>
		/// <returns>
		/// A <see cref="IDictionary{TKey,TValue}"/> of the properties available for the 
		/// current test configuration. Guaranteed not to be null.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if <paramref name="env"/> is null.
		/// </exception>
		/// <exception cref="Exception">
		/// Thrown if there are any problems retrieving the configuration values for the
		/// current test configuration.
		/// </exception>
		Task<IDictionary<string, string>> GetTestConfigurationPropertiesAsync(VstsEnvironmentVariables env);
	}
}
