namespace VstsLabAccessor2015
{
	using System;

	public class VstsSystemVariables
	{
		/// <summary>
		/// Gets the OAuth token to the VSTS Rest API used by the build system and
		/// made accessible to build steps for their use, if configured in the build.
		/// </summary>
		public string AccessToken { get; private set; }

		/// <summary>
		/// Gets the ID of the TFVC project collection
		/// </summary>
		public Guid? CollectionId { get; private set; }

		/// <summary>
		/// Gets the file system path to the default working directory of the agent.
		/// </summary>
		public string DefaultWorkingDirectory { get; private set; }

		/// <summary>
		/// Gets the ID of the build definition used to execute the build.
		/// </summary>
		public string DefinitionId { get; private set; }

		/// <summary>
		/// Gets the URI of the team foundation server
		/// </summary>
		public Uri TeamFoundationServerUri { get; private set; }

		/// <summary>
		/// Gets the URI to the Team Foundation Collection in use
		/// </summary>
		public Uri TeamFoundationCollectionUri { get; private set; }

		/// <summary>
		/// Gets the name of the Team Project that contains this build.
		/// </summary>
		public string TeamProject { get; private set; }

		/// <summary>
		/// Gets the ID of the team project to which this build belongs.
		/// </summary>
		public string TeamProjectId { get; private set; }

		/// <summary>
		/// Gets the file system path to the directory in which release artifacts are stored.
		/// </summary>
		public string ArtifactsDirectory { get; private set; }

		/// <summary>
		/// Gets a value indicating whether or not debug mode is enabled.
		/// </summary>
		public bool Debug { get; private set; }

		/// <summary>
		/// Gets the file system path to the working folder for the build.
		/// </summary>
		public string WorkFolder { get; private set; }
	}
}