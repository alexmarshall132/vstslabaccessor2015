namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;

	public class VstsTfvcBuildRepositoryVariables : IReadFromProperties
	{
		/// <summary>
		/// Gets the file system path to the workspace folder
		/// </summary>
		public string Workspace { get; private set; }

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