namespace PaymentApi.ComponentTests
{
    using AcquiringBankMock.Settings;
    using API;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureAcquiringBankMockSettings(IServiceCollection services)
        {
            services.AddSingleton<AcquiringBankMockSettings, TestAcquiringBankMockSettings>();
        }

        public override void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddSingleton(s => TestFixture.Client);
        }
    }
}
