namespace TestOkur.Report.Integration.Tests.Common
{
	using Microsoft.AspNetCore.TestHost;
	using TestOkur.TestHelper;

	public class TestServerFactory : TestServerFactory<TestStartup>
	{
		static TestServerFactory()
		{
			TestServer = new TestServerFactory()
				.Create();
		}

		public static TestServer TestServer { get; }
	}
}
