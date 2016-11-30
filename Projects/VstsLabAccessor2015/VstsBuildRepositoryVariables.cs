namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class VstsBuildRepositoryVariables : IReadFromProperties
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VstsBuildRepositoryVariables"/> class.
		/// </summary>
		public VstsBuildRepositoryVariables()
		{
			this.Tfvc = new VstsTfvcBuildRepositoryVariables();
			this.Git = new VstsGitBuildRepositoryVariables();
		}

		/// <summary>
		/// Gets the Clean value selected on the repository tab
		/// </summary>
		public string Clean { get; private set; }

		/// <summary>
		/// The local path on the build agent where build files are downloaded.
		/// </summary>
		public string LocalPath { get; private set; }

		/// <summary>
		/// Gets the name of the repository
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the type of provider of the repository.
		/// </summary>
		public string Provider { get; private set; }

		/// <summary>
		/// Gets the team foundation version control properties
		/// </summary>
		public VstsTfvcBuildRepositoryVariables Tfvc { get; private set; }

		/// <summary>
		/// Gets the Git version control properties
		/// </summary>
		public VstsGitBuildRepositoryVariables Git { get; private set; }

		/// <summary>
		/// Gets the URL for the repository
		/// </summary>
		public Uri Uri { get; private set; }

		/// <inheritdoc />
		public void ReadProperties(IEnumerable<KeyValuePair<string, object>> properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			List<KeyValuePair<string, object>> propertyList = properties.ToList();

			PropertyHelper.SetDirectVariables(this, propertyList);

			this.Tfvc.ReadProperties(PropertyHelper.SelectPropertiesAndTrimName(propertyList, "TFVC_"));
			this.Git.ReadProperties(PropertyHelper.SelectPropertiesAndTrimName(propertyList, "GIT_"));
		}
	}
}