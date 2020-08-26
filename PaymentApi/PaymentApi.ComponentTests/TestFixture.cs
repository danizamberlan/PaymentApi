namespace PaymentApi.ComponentTests
{
    using System.Net.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class TestFixture
    {
        public static HttpClient Client;

        public TestFixture()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<TestStartup>();
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddJsonFile(
                        path: "appsettings.json",
                        optional: false,
                        reloadOnChange: true);

                    context.HostingEnvironment.EnvironmentName = "test";
                });

            var host = hostBuilder.Start();

            Client = host.GetTestClient();
        }
    }
}
