namespace PaymentApi.API
{
    using System;
    using System.Net.Http;
    using AcquiringBankMock;
    using AcquiringBankMock.Settings;
    using Application.Commands.CreatePayment;
    using CrossCutting;
    using CrossCutting.HttpService;
    using FluentValidation.AspNetCore;
    using Gateway;
    using Gateway.Client;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Middleware;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));

            services.AddControllers().AddApplicationPart(typeof(AcquiringBankMockController).Assembly);
            ConfigureAcquiringBankMockSettings(services);

            services.AddMediatR(typeof(CreatePaymentCommandHandler));

            services.AddSingleton<IHttpService, HttpService>();
            ConfigureHttpClient(services);

            services.AddScoped<IAcquiringBankGateway, AcquiringBankGateway>();
            services.AddScoped<IAcquiringBankClient>(
                s => new AcquiringBankClient(
                        s.GetService<IHttpService>(),
                        Configuration.GetValue<string>("Services:AcquiringBankUrl")));

            services.AddScoped<IEncryptionLibrary>(
                s => new EncryptionLibrary(
                    Configuration.GetValue<string>("EncryptionKey")));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Payment Gateway",
                        Version = "v1",
                        Description = "PaymentGateway created to Process and View payment requests to a mocked AcquiringBank",
                        Contact = new OpenApiContact
                        {
                            Name = "Daniela Zamberlan",
                            Url = new Uri("https://www.linkedin.com/in/daniela-zamberlan/")
                        }
                    });
            });

        }

        /// <summary>
        /// Configures the HttpClient instance to be used by the app.
        /// </summary>
        /// <param name="services">Service collection under configuration.</param>
        public virtual void ConfigureHttpClient(IServiceCollection services)
        {
            var httpClientHandler = new HttpClientHandler
            {
                // This approach was used for accepting self-signed certificates and should be revisited before moving to production. 
                ServerCertificateCustomValidationCallback = (message,cert,chain,errors) => true
            };
            
            var httpClient = new HttpClient(httpClientHandler);

            services.AddSingleton(httpClient);
        }

        /// <summary>
        /// Configures the acquiring bank mock settings.
        /// </summary>
        /// <param name="services">Service collection under configuration.</param>
        public virtual void ConfigureAcquiringBankMockSettings(IServiceCollection services)
        {
            services.AddSingleton<AcquiringBankMockSettings, FakeAcquiringBankMockSettings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}
