namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Represents build-scoped variables
	/// </summary>
	public class VstsBuildVariables : IReadFromProperties
	{
		private const string RepositoryPrefix = "REPOSITORY_";

		/// <summary>
		/// Gets the file system path to the artifact staging directory
		/// </summary>
		public string ArtifactStagingDirectory { get; private set; }

		/// <summary>
		/// Gets the Build ID
		/// </summary>
		public string BuildId { get; private set; }

		/// <summary>
		/// Gets the build number
		/// </summary>
		public string BuildNumber { get; private set; }

		/// <summary>
		/// Gets the URI of the build within VSTS.
		/// </summary>
		public Uri BuildUri { get; private set; }

		/// <summary>
		/// Gets the file system path to the directory containing the binaries for the build
		/// </summary>
		public string BinariesDirectory { get; private set; }

		/// <summary>
		/// Gets the build definition name
		/// </summary>
		public string DefinitionName { get; private set; }

		/// <summary>
		/// Gets the version of the build definition used to execute the build
		/// </summary>
		public string DefinitionVersion { get; private set; }

		/// <summary>
		/// Gets the name of the user that queued the build
		/// </summary>
		public string QueuedBy { get; private set; }

		/// <summary>
		/// Gets the ID of the user that queued the build.
		/// </summary>
		public string QueuedById { get; private set; }

		/// <summary>
		/// Gets the repository variables
		/// </summary>
		public VstsBuildRepositoryVariables Repository { get; private set; }

		/// <summary>
		/// Gets the name of the user for whom the build was requested
		/// </summary>
		public string RequestedFor { get; private set; }

		/// <summary>
		/// Gets the email address of the user for whom the build was requested.
		/// </summary>
		public string RequestedForEmail { get; private set; }

		/// <summary>
		/// Gets the ID of the user for whom the build was requested.
		/// </summary>
		public string RequestedForId { get; private set; }

		/// <summary>
		/// Gets the source branch from which the code is being built.
		/// </summary>
		public string SourceBranch { get; private set; }

		/// <summary>
		/// Gets the name of the source branch from which the code is being built
		/// </summary>
		public string SourceBranchName { get; private set; }

		/// <summary>
		/// Gets the file sytem path to the directory in which sources were downloaded on the build agent.
		/// </summary>
		public string SourcesDirectory { get; private set; }

		/// <summary>
		/// Gets the version of the source code that was checked out for build
		/// </summary>
		public string SourceVersion { get; private set; }

		/// <summary>
		/// Gets the file system path to the build staging directory.
		/// </summary>
		public string StagingDirectory { get; private set; }

		/// <summary>
		/// Gets the name of the shelveset being built if the source control is TFVC.
		/// </summary>
		public string SourceTfvcShelveset { get; private set; }

		/// <inheritdoc />
		public void ReadProperties(IEnumerable<KeyValuePair<string, object>> properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			List<KeyValuePair<string, object>> propertyList = properties.ToList();

			PropertyHelper.SetDirectVariables(this, propertyList);

			this.Repository.ReadProperties(PropertyHelper.SelectPropertiesAndTrimName(propertyList, RepositoryPrefix));
		}
	}
}