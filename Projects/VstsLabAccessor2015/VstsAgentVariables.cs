namespace VstsLabAccessor2015
{
	/// <summary>
	/// Represents VSTS variables in the Agent scope.
	/// </summary>
	public class VstsAgentVariables
	{
		/// <summary>
		/// Gets the build directory
		/// </summary>
		public string BuildDirectory { get; private set; }

		/// <summary>
		/// Gets the home directory
		/// </summary>
		public string HomeDirectory { get; private set; }

		/// <summary>
		/// Gets the ID of the agent
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Gets the name of the machine on which the agent is running
		/// </summary>
		public string MachineName { get; private set; }

		/// <summary>
		/// The name of the agent
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The path to the working folder on the agent
		/// </summary>
		public string WorkFolder { get; private set; }

		/// <summary>
		/// Gets the name of the job that's currently executing on the agent.
		/// </summary>
		public string JobName { get; private set; }

		/// <summary>
		/// Gets the file system path to the directory to which artifacts are downloaded
		/// during deployment of a release.
		/// </summary>
		public string ReleaseDirectory { get; private set; }

		/// <summary>
		/// Gets the working directory for the agent.
		/// </summary>
		public string RootDirectory { get; private set; }

		/// <summary>
		/// Gets the current working directory for the agent.
		/// </summary>
		public string WorkingDirectory { get; private set; }
	}
}