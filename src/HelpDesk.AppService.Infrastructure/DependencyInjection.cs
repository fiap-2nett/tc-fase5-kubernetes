using System;
using HelpDesk.AppService.Application.Core.Abstractions.Authentication;
using HelpDesk.AppService.Application.Core.Abstractions.ExternalService;
using HelpDesk.AppService.Infrastructure.Authentication;
using HelpDesk.AppService.Infrastructure.Authentication.Settings;
using HelpDesk.AppService.Infrastructure.ExternalService;
using HelpDesk.AppService.Infrastructure.ExternalService.Handlers;
using HelpDesk.AppService.Infrastructure.ExternalService.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.AppService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));
            services.Configure<ServiceSettings>(configuration.GetSection(ServiceSettings.SettingsKey));

            services.AddScoped<IUserSessionProvider, UserSessionProvider>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddTransient<MessageHandler>();
            services.AddTransient<SkipServerCertificateValidationHandler>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");

                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            services.AddHttpClient(typeof(IAuthenticationService).FullName)
                .ConfigureHttpClient(config =>
                {
                    config.BaseAddress = new Uri(configuration["ExternalService:Url"]);
                    config.Timeout = TimeSpan.FromSeconds(double.Parse(configuration["ExternalService:RequestTimeoutInSeconds"]));
                })
                .AddHttpMessageHandler<MessageHandler>()
                .AddHttpMessageHandler<SkipServerCertificateValidationHandler>();

            services.AddHttpClient(typeof(IUserService).FullName)
                .ConfigureHttpClient(config =>
                {
                    config.BaseAddress = new Uri(configuration["ExternalService:Url"]);
                    config.Timeout = TimeSpan.FromSeconds(double.Parse(configuration["ExternalService:RequestTimeoutInSeconds"]));
                })
                .AddHttpMessageHandler<MessageHandler>()
                .AddHttpMessageHandler<SkipServerCertificateValidationHandler>();

            services.AddHttpClient(typeof(ITicketService).FullName)
                .ConfigureHttpClient(config =>
                {
                    config.BaseAddress = new Uri(configuration["ExternalService:Url"]);
                    config.Timeout = TimeSpan.FromSeconds(double.Parse(configuration["ExternalService:RequestTimeoutInSeconds"]));
                })
                .AddHttpMessageHandler<MessageHandler>()
                .AddHttpMessageHandler<SkipServerCertificateValidationHandler>();

            services.AddHttpClient(typeof(IStatusService).FullName)
                .ConfigureHttpClient(config =>
                {
                    config.BaseAddress = new Uri(configuration["ExternalService:Url"]);
                    config.Timeout = TimeSpan.FromSeconds(double.Parse(configuration["ExternalService:RequestTimeoutInSeconds"]));
                })
                .AddHttpMessageHandler<MessageHandler>()
                .AddHttpMessageHandler<SkipServerCertificateValidationHandler>();

            services.AddHttpClient(typeof(ICategoryService).FullName)
                .ConfigureHttpClient(config =>
                {
                    config.BaseAddress = new Uri(configuration["ExternalService:Url"]);
                    config.Timeout = TimeSpan.FromSeconds(double.Parse(configuration["ExternalService:RequestTimeoutInSeconds"]));
                })
                .AddHttpMessageHandler<MessageHandler>()
                .AddHttpMessageHandler<SkipServerCertificateValidationHandler>();

            return services;
        }
    }
}
