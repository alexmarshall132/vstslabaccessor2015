namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	/// <summary>
	/// Default implementation of <see cref="IVstsTestAccessor2015"/>
	/// </summary>
	public class VstsTestAccessor2015 : IVstsTestAccessor2015
	{
		/// <summary>
		/// The credentials to be used to make calls to the VSTS API.
		/// </summary>
		private readonly NetworkCredential credentials;

		/// <summary>
		/// The size of pages of results when querying VSTS
		/// </summary>
		private readonly int pageSize;

		/// <summary>
		/// Initializes a new instance of the <see cref="VstsTestAccessor2015"/> class.
		/// </summary>
		public VstsTestAccessor2015() : this(null, 10)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VstsTestAccessor2015"/> class.
		/// </summary>
		/// <param name="credentials">
		/// The credentials to be used to make calls to the VSTS API. May be null.
		/// </param>
		/// <param name="pageSize">
		/// The page size to be used when querying VSTS for test configurations.
		/// Must be at least 1.
		/// </param>
		public VstsTestAccessor2015(NetworkCredential credentials, int pageSize)
		{
			if (pageSize < 1)
			{
				throw new ArgumentOutOfRangeException("pageSize");
			}

			this.credentials = credentials;
			this.pageSize = pageSize;
		}

		/// <inheritdoc />
		public async Task<IDictionary<string, string>> GetTestConfigurationPropertiesAsync(VstsEnvironmentVariables env, string configurationName)
		{
			if (env == null)
			{
				throw new ArgumentNullException("env");
			}

			if (string.IsNullOrEmpty(configurationName))
			{
				throw new ArgumentException("Must not be null or empty", "configurationName");
			}

			HttpClient client = new HttpClient
			{
				DefaultRequestHeaders =
				{
					Authorization = this.ResolveAuthenticationHeader(env)
				}
			};

			int pageIndex = -1;

			IList<TestConfig> configurationProperties;
			TestConfig configuration;

			do
			{
				configurationProperties = await GetTestConfigurationsPageAsync(env, ++pageIndex, this.pageSize, client);

				if (configurationProperties == null)
				{
					return null;
				}

				configuration = configurationProperties.FirstOrDefault(tc => StringComparer.InvariantCultureIgnoreCase.Equals(tc.Name, configurationName));
			}
			while (configurationProperties.Count >= this.pageSize && configuration == null);

			return configuration == null ? null : (configuration.Values ?? Enumerable.Empty<TestConfigValue>()).ToDictionary(x => x.Name, x => x.Value);
		}

		#region Helper Methods

		private static async Task<IList<TestConfig>> GetTestConfigurationsPageAsync(VstsEnvironmentVariables env, int pageIndex, int pageSize, HttpClient client)
		{
			if (env == null)
			{
				throw new ArgumentNullException("env");
			}

			if (client == null)
			{
				throw new ArgumentNullException("client");
			}

			if (pageIndex < 0)
			{
				throw new ArgumentOutOfRangeException("pageIndex");
			}

			if (pageSize < 1)
			{
				throw new ArgumentOutOfRangeException("pageSize");
			}

			UriBuilder uriBuilder = new UriBuilder(env.System.TeamFoundationCollectionUri);

			uriBuilder.Path += String.Format("/{0}/_apis/test/configurations", env.System.TeamProject);
			uriBuilder.Query = String.Format("includeAllProperties={0}&api-version={1}&$skip={2}&$top={3}", true, "3.0-preview.1", pageIndex * pageSize, pageSize);

			string jsonResponse;

			using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
			{
				jsonResponse = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
			}

			dynamic result = JsonConvert.DeserializeObject(jsonResponse);

			JArray configurations = result.value;

			return configurations.ToObject<List<TestConfig>>();
		}

		private AuthenticationHeaderValue ResolveAuthenticationHeader(VstsEnvironmentVariables env)
		{
			if (env == null)
			{
				throw new ArgumentNullException("env");
			}

			if (this.credentials != null)
			{
				return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", this.credentials.UserName, this.credentials.Password))));
			}

		    string accessToken = env.System.AccessToken;

		    if (String.IsNullOrEmpty(accessToken))
		    {
		        throw new InvalidOperationException("Failed to resolve System.AccessToken as it was blank in the VSTS variables provided to the test");
		    }

		    return new AuthenticationHeaderValue("Bearer", accessToken);
		} 

		#endregion
	}
}