using HelpDesk.ApiService.Application.Core.Abstractions.Services;
using HelpDesk.ApiService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.ApiService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITicketStatusService, TicketStatusService>();

            return services;
        }
    }
}
