namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;
	using Newtonsoft.Json;

	/// <summary>
	/// Default implementation of <see cref="IVstsTestAccessor2015"/>
	/// </summary>
	public class VstsTestAccessor2015 : IVstsTestAccessor2015
	{
		/// <inheritdoc />
		public async Task<IDictionary<string, string>> GetTestConfigurationPropertiesAsync(VstsEnvironmentVariables env)
		{
			if (env == null)
			{
				throw new ArgumentNullException("env");
			}

			HttpClient client = new HttpClient
			{
				DefaultRequestHeaders =
				{
					Authorization = new AuthenticationHeaderValue("Bearer", env.System.AccessToken)
				}
			};

			UriBuilder uriBuilder = new UriBuilder(env.System.TeamFoundationCollectionUri);

			uriBuilder.Path += String.Format("/{0}/_apis/test/configurations?includeAllProperties={4}&api-version={1}&$skip={2}&$top={3}", env.System.TeamProject, "3.0-preview.1", 0, 10, true);

			string jsonResponse;

			using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
			{
				jsonResponse = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
			}

			Debug.WriteLine(jsonResponse);

			dynamic result = JsonConvert.DeserializeObject(jsonResponse);

			int count = result.count;

			IDictionary<string, string> properties = new Dictionary<string, string>();

			if (count <= 0)
			{
				return properties;
			}

			dynamic[] configurations = result.value;

			dynamic[] configurationProperties = configurations[0].values;

			foreach (dynamic thing in configurationProperties)
			{
				properties[Convert.ToString(thing.name)] = Convert.ToString(thing.value);
			}

			return properties;
		}
	}
}