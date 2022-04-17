using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Repositories;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.BetTicket.Core
{
    public static class DefendencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
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