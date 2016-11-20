namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;

	public class VstsGitBuildRepositoryVariables
	{
		/// <summary>
		/// The value selected on the build page for checking out git submodules.
		/// </summary>
		public string SubmoduleCheckout { get; private set; }

		/// <inheritdoc />
		public void ReadProperties(IEnumerable<KeyValuePair<string, object>> properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			PropertyHelper.SetDirectVariables(this, properties);
		}
	}
}