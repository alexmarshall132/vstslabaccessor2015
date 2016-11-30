namespace VstsLabAccessor2015
{
	public class TestConfig
	{
		/// <summary>
		/// Gets or sets the name of the test configuration
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the test configuration values.
		/// </summary>
		public TestConfigValue[] Values { get; set; }
	}
}