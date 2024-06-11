using System.IO;
using System.Text.Json;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using HelpDesk.ApiService.Api.Extensions;
using HelpDesk.ApiService.Application;
using HelpDesk.ApiService.Infrastructure;
using HelpDesk.ApiService.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.ApiService.Api.IntegrationTests.SeedWork
{
    public sealed class TestStartup
    {
        public readonly IConfiguration _configuration;

        public TestStartup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApplication()
                .AddInfrastructure(_configuration)
                .AddPersistence(_configuration);

            services
                .AddAuthentication(options => options.DefaultScheme = TestServerDefaults.AuthenticationScheme)
                .AddTestServer();

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IncludeFields = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .AddApplicationPart(AssemblyReference.Assembly);

            services.AddHttpContextAccessor();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.EnsureDatabaseCreated();
            app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
