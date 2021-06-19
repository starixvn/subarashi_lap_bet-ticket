using Fanex.Athena.BetTicket.Infrastructure.ConsoleClient;
using Fanex.Athena.BetTicket.Infrastructure.Repositories;
using Fanex.Athena.BetTicket.Infrastructure.Services;
using Fanex.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.BetTicket.Infrastructure
{
    public static class DefendencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddRepositories()
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IBetListApiClient, BetListApiClient>();
            services.AddScoped<IContextService, ContextService>();
            services.AddSingleton<ILicenseeService, LicenseeService>();

            services.AddSingleton<IMetaModelBindingService, BaseMetaModelBindingService>();

            services.AddHttpContextAccessor();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDbFactory, DbFactory>();

            return services;
        }
    }
}