namespace VstsLabAccessor2015
{
	using System;

	/// <summary>
	/// Represents VSTS release variables
	/// </summary>
	public class VstsReleaseVariables
	{
		/// <summary>
		/// Gets the name of the release definition running the tests
		/// </summary>
		public string DefinitionName { get; private set; }

		/// <summary>
		/// Gets the URI of the environment within the release to which the system is being deployed and tests are currently being run.
		/// </summary>
		public string EnvironmentUri { get; private set; }

		/// <summary>
		/// Gets the name of the environment within the release to which the system is being deployed and tests are currently being run.
		/// </summary>
		public string EnvironmentName { get; private set; }

		/// <summary>
		/// Gets the text description of the release that's being executed.
		/// </summary>
		public string ReleaseDescription { get; private set; }

		/// <summary>
		/// Gets the ID of the release that's currently executing.
		/// </summary>
		public string ReleaseId { get; private set; }

		/// <summary>
		/// Gets the name of the release that's currently executing.
		/// </summary>
		public string ReleaseName { get; private set; }

		/// <summary>
		/// Gets the URI of the release that's currently executing.
		/// </summary>
		public Uri ReleaseUri { get; private set; }

		/// <summary>
		/// Gets the name of the user for whom the release was requested.
		/// </summary>
		public string RequestedFor { get; private set; }

		/// <summary>
		/// Gets the ID of the user for whom the release was requested.
		/// </summary>
		public string RequestedForId { get; private set; }
	}
}