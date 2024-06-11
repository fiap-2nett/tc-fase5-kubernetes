using HelpDesk.AppService.Application.Core.Abstractions.Services;
using HelpDesk.AppService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.AppService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
