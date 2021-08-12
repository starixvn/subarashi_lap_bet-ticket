using Fanex.Athena.BetTicket.Core;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Extensions;
using Fanex.Data;
using Fanex.Data.SqlServer;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.WebHost
{
    public class Startup
    {
        private const string IntrospectionSchema = "Introspection";

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            //if(HostingEnvironment.IsDevelopment())
            //{
            //    mvcBuilder.AddRazorRuntimeCompilation(options =>
            //    {
            //        var libraryPath = Path.GetFullPath(Path.Combine(HostingEnvironment.ContentRootPath, "..", "Modules", "Fanex.Athena.BetTicket.Detail"));
            //        options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
            //    });
            //}

            ConfigFanexData();

            services.AddInfrastructure();

            var instrospecCacheDuration = int.TryParse(Configuration["IdentityServer:CacheDuration"], out int cache) ? cache : 0;
            services.AddAuthentication(IntrospectionSchema)
               .AddOAuth2Introspection(IntrospectionSchema, options =>
               {
                   options.Authority = Configuration["IdentityServer:Host"];
                   options.ClientId = Configuration["IdentityServer:CredentialIntrospection:ClientId"];
                   options.ClientSecret = Configuration["IdentityServer:CredentialIntrospection:ClientSecret"];
                   options.SaveToken = true;
                   options.SkipTokensWithDots = true;
                   options.EnableCaching = instrospecCacheDuration > 0;
                   options.CacheKeyPrefix = "OAuth2Introspection:";
                   options.CacheDuration = TimeSpan.FromMinutes(instrospecCacheDuration);
                   options.DiscoveryPolicy.RequireHttps = false;

                   var fromQuery = TokenRetrieval.FromQueryString();
                   var fromHeader = TokenRetrieval.FromAuthorizationHeader();
                   options.TokenRetriever = request => fromQuery.Invoke(request) ?? fromHeader.Invoke(request);
               });

            services.AddHttpClient<IBetListApiClient, BetListApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ConsoleClient:BetList:Host"]);
                client.DefaultRequestHeaders.Add("BETLIST_TOKEN_KEY", Configuration["ConsoleClient:BetList:AuthKey"]);
            }).SetHandlerLifetime(TimeSpan.FromDays(1));

            var proxySetting = Configuration.GetSection("Proxy:BetList").Get<ProxyApiSetting>();
            ConfigProxy(services, proxySetting);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
                {
                    var lang = context.Request.Query.TryGetValue("language", out StringValues language) ?
                        language.First() : "en-US";
                    return Task.FromResult(new ProviderCultureResult(lang));
                }));
            });

            services.AddHealthCheck(Configuration);
            services.AddUnderMaintenance(Configuration);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            if (string.Equals(Configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            if (!HostingEnvironment.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticResource(HostingEnvironment.IsDevelopment());
            app.UseHealthCheck(Configuration);
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseProxy();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void ConfigFanexData()
        {
            var listConnectionName = Configuration.GetSection("ConnectionStrings").GetChildren().Select(_ => _.Key);
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>(listConnectionName
                .Select(connectionName => new KeyValuePair<string, ConnectionConfiguration>(connectionName,
                    new ConnectionConfiguration(connectionName, Configuration.GetConnectionString(connectionName)))
                ));

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: "~/App_Data/SpConfig/FanexSpName.xml")
                .Use(connectionStrings)
                .Run();
        }

        private void ConfigProxy(IServiceCollection services, ProxyApiSetting proxySetting)
        {
            services.AddProxy(options =>
            {
                options.Timeout = TimeSpan.FromSeconds(int.Parse(Configuration["Proxy:Timeout"]));

                options.ValidateRequest = context => context.User.Identity.IsAuthenticated;

                options.ShouldProxyRequest = originalRequest => originalRequest.Path.StartsWithSegments("/proxy");

                options.OnFailedValidation = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };

                options.PrepareRequest = async (originalRequest, message) =>
                {
                    var context = originalRequest.HttpContext;
                    message.Headers.Add(proxySetting.TokenKey, proxySetting.TokenValue);

                    const string Language = "Language";
                    if (originalRequest.Query.TryGetValue(Language, out StringValues language))
                    {
                        message.Headers.Add(Language, language.First());
                    }

                    var accessToken = await context.GetTokenAsync("access_token") ?? context.Items["access_token"] as string;
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        return;
                    }
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                };

                options.PrepareUri = originalRequest =>
                {
                    var path = originalRequest.Path.Value;
                    var realPath = path.Substring($"/proxy".Length);
                    var uri = new UriBuilder(new Uri($"{proxySetting.Url.TrimEnd('/')}{realPath}"))
                    {
                        Query = originalRequest.QueryString.Value
                    }.Uri;

                    return uri;
                };
            });
        }
    }
}