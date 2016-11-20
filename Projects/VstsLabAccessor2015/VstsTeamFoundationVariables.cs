namespace VstsLabAccessor2015
{
	/// <summary>
	/// Represents Team Foundation variables during a VSTS build.
	/// </summary>
	public class VstsTeamFoundationVariables
	{
		/// <summary>
		/// Gets or sets a value indicating whether or not the build is being executed
		/// by a VSTS build step
		/// </summary>
		public bool Build { get; private set; }
	}
}