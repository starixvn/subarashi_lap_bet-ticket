using Fanex.Athena.BetTicket.Core.HealthCheck;
using Fanex.Athena.BetTicket.Core.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Core.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
               .AddCheck<WhoAmIHealthCheck>("WhoAmI")
               .AddCheck<MemoryHealthCheck>("Memory")
               .AddUrlGroup(new Uri($"{configuration["IdentityServer:Host"]}/{configuration["IdentityServer:HealthPath"]}"), "Identity Server")
               .AddUrlGroup(new Uri($"{configuration["ConsoleClient:BetList:Host"]}{configuration["ConsoleClient:BetList:HealthPath"]}"), "BetList API");
        }

        public static void UseHealthCheck(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ping"))
                {
                    return;
                }
                await next();
            });

            Func<HttpContext, HealthReport, Task> customResponseWriter = async (context, report) =>
            {
                var result = JsonConvert.SerializeObject(
                    new
                    {
                        status = report.Status.ToString(),
                        monitors = report.Entries.Select(e => new { name = e.Key, status = Enum.GetName(typeof(HealthStatus), e.Value.Status), values = e.Value.Data })
                    }, Formatting.Indented);

                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            };

            app.UseHealthChecks("/WhoAmI", options: new HealthCheckOptions
            {
                AllowCachingResponses = false,
                Predicate = _ => _.Name.Equals("WhoAmI"),
                ResponseWriter = customResponseWriter
            }).UseHealthChecks("/health", options: new HealthCheckOptions
            {
                AllowCachingResponses = false,
                Predicate = _ => !_.Name.Equals("WhoAmI"),
                ResponseWriter = customResponseWriter
            });
        }

        public static void AddUnderMaintenance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new UMClientConfiguration().Initialize(configuration));
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddSession();
        }

        public static void UseUnderMaintenance(this IApplicationBuilder app)
        {
            app.UseSession();
            app.UseMiddleware<UnderMaintenanceHandler>();
        }

        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            //mvc
            app.UseWhen(context => !context.Request.IsAjaxRequest() && !context.Request.IsStaticRequest(), builder => builder.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}"));

            //api
            app.UseWhen(context => context.Request.IsAjaxRequest(), builder => builder.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
                var errorObject = new
                {
                    statusCode = response.StatusCode,
                    msg = "An error occurred while processing your request",
                    isSuccess = false
                };
                await response.WriteAsync(JsonConvert.SerializeObject(errorObject));
            }));

            //handle error
            app.UseMiddleware<ErrorHandler>();
        }

        public static void UseStaticResource(this IApplicationBuilder app, bool isDev)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int OneYear = 60 * 60 * 24 * 365;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = isDev ?
                    "no-cache" : "public,max-age=" + OneYear;
                    ctx.Context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                }
            });
        }
    }
}