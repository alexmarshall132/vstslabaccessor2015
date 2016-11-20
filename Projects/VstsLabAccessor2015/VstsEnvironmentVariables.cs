using System;
namespace VstsLabAccessor2015
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Represents the set of VSTS environment variables.
	/// </summary>
    public class VstsEnvironmentVariables
	{
		private const string AgentPrefix = "AGENT_";
		private const string BuildPrefix = "BUILD_";
		private const string ReleasePrefix = "RELEASE_";
		private const string TeamFoundationPrefix = "TF_";
		private const string SystemPrefix = "SYSTEM_";
		private const string CommonPrefix = "COMMON_";

		private static readonly string[] WellKnownPrefixes =
		{
			AgentPrefix,
			BuildPrefix,
			ReleasePrefix,
			TeamFoundationPrefix,
			SystemPrefix,
			CommonPrefix
		};

		/// <summary>
		/// Gets the agent variables
		/// </summary>
		public VstsAgentVariables Agent { get; private set; }

		/// <summary>
		/// Gets the build variables
		/// </summary>
		public VstsBuildVariables Build { get; private set; }

		/// <summary>
		/// Gets the release variables
		/// </summary>
		public VstsReleaseVariables Release { get; private set; }

		/// <summary>
		/// Gets the system variables
		/// </summary>
		public VstsSystemVariables System { get; private set; }

		/// <summary>
		/// Gets the Team Foundation variables
		/// </summary>
		public VstsTeamFoundationVariables Tf { get; private set; }

		/// <summary>
		/// Gets the common variables
		/// </summary>
		public VstsCommonVariables Common { get; private set; }

		/// <summary>
		/// Gets a value indicating whether or not this instance is valid.
		/// </summary>
		public bool IsValid { get; private set; }

		/// <summary>
		/// Creates a new <see cref="VstsEnvironmentVariables"/> instance from the given <paramref name="testProperties"/>
		/// </summary>
		/// <param name="testProperties">
		/// The test properties from which we want to extract the TFS Test Run settings. Must not be null.
		/// This should typically be <see cref="TestContext.Properties"/>
		/// </param>
		/// <returns>
		/// A new <see cref="VstsEnvironmentVariables"/> instance generated from the given <paramref name="testProperties"/>.
		/// Guaranteed not to be null.
		/// </returns>
		public static VstsEnvironmentVariables Create(IDictionary testProperties)
		{
			if (testProperties == null)
			{
				throw new ArgumentNullException("testProperties");
			}

			Dictionary<string, object> properties = testProperties.Keys.OfType<string>()
				.Where(k => WellKnownPrefixes.Any(k.StartsWith))
				.ToDictionary(k => k, k => testProperties[k]);

			VstsEnvironmentVariables runProperties = new VstsEnvironmentVariables
			{
				Agent = new VstsAgentVariables(),
				Build = new VstsBuildVariables(),
				Release = new VstsReleaseVariables(),
				Tf = new VstsTeamFoundationVariables(),
				Common = new VstsCommonVariables(),
				System = new VstsSystemVariables(),
				IsValid = false
			};

			if (properties.Count == 0)
			{
				return runProperties;
			}

			PropertyHelper.SetVariables(runProperties.Agent, PropertyHelper.SelectPropertiesAndTrimName(properties, AgentPrefix));
			PropertyHelper.SetVariables(runProperties.Build, PropertyHelper.SelectPropertiesAndTrimName(properties, BuildPrefix));
			PropertyHelper.SetVariables(runProperties.Release, PropertyHelper.SelectPropertiesAndTrimName(properties, ReleasePrefix));
			PropertyHelper.SetVariables(runProperties.Tf, PropertyHelper.SelectPropertiesAndTrimName(properties, TeamFoundationPrefix));
			PropertyHelper.SetVariables(runProperties.Common, PropertyHelper.SelectPropertiesAndTrimName(properties, CommonPrefix));
			PropertyHelper.SetVariables(runProperties.System, PropertyHelper.SelectPropertiesAndTrimName(properties, SystemPrefix));

			runProperties.IsValid = true;

			return runProperties;
		}
	}
}
