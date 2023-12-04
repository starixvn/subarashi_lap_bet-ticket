using Fanex.Athena.BetTicket.Core;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Extensions;
using Fanex.Athena.BetTicket.Core.JsonConverter;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Data;
using Fanex.Data.SqlServer;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.WebHost
{
    public class Startup
    {
        private const string IntrospectionSchema = "Introspection";
        private const string JwtSchema = "Jwt";

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = bool.TryParse(Configuration["SkipCheckClientIP"], out bool skipCheckClientIP);

            services.AddControllersWithViews(options =>
            {
                var policyBuilder = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser();
                if (!skipCheckClientIP)
                {
                    policyBuilder.RequireAssertion(context => context.Resource is not AuthorizationFilterContext mvcContext
                                    || context.User.FindFirst("ClientIP")?.Value is not string clientIP
                                    || string.Equals(mvcContext.HttpContext.Connection.RemoteIpAddress.ToString(), clientIP, StringComparison.InvariantCultureIgnoreCase));
                }


                options.Filters.Add(new AuthorizeFilter(policyBuilder.Build()));
            }).AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                    jsonOptions.JsonSerializerOptions.Converters.Add(new StringToDateTimeConverter());
                });

            services.AddAuthorization(options =>
            {
                options.ViewDetailPolicy();
            });

            ConfigFanexData();
            ConfigAuthentication(services);
            ConfigProxy(services, skipCheckClientIP);
            ConfigRequestLocalization(services);
            services.AddCore();

            services.AddHttpClient<IBetListApiClient, BetListApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ConsoleClient:BetList:Host"]);
                client.DefaultRequestHeaders.Add("BETLIST_TOKEN_KEY", Configuration["ConsoleClient:BetList:AuthKey"]);
            }).SetHandlerLifetime(TimeSpan.FromDays(1));


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetPreflightMaxAge(TimeSpan.FromDays(1));
                    });
            });

            services.AddHealthCheck(Configuration);
            services.AddUnderMaintenance(Configuration);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddHttpsRedirection(options =>
            {
                const int httpsPort = 443;
                options.HttpsPort = httpsPort;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UsePing();
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

            app.UseHealthCheck(Configuration);
            app.UseStaticResource(HostingEnvironment.IsDevelopment());
            app.UseUnderMaintenance();
            app.UseRouting();
            app.UseCors();
            app.UseRequestLocalization();
            app.UseAuthentication();
            app.UseProxy();
            app.UseAuthorization();
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

        private void ConfigAuthentication(IServiceCollection services)
        {
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
                   string tokenRetriever(HttpRequest request) => fromQuery.Invoke(request) ?? fromHeader.Invoke(request);

                   options.TokenRetriever = tokenRetriever;

                   options.ForwardDefaultSelector = context =>
                   {
                       var token = tokenRetriever(context.Request);
                       if (!string.IsNullOrEmpty(token) && token.Contains("."))
                       {
                           if (!context.Request.Headers.ContainsKey("Authorization"))
                           {
                               context.Request.Headers.Add("Authorization", "Bearer " + token);
                           }
                           return JwtSchema;
                       }
                       return IntrospectionSchema;
                   };
               })
               .AddJwtBearer(JwtSchema, x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWTStatement:Secret"])),
                        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWTStatement:Secret"])),
                        ValidIssuer = Configuration["JWTStatement:Issuer"],
                        ValidAudience = Configuration["JWTStatement:Audience"],
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RoleClaimType = ClaimsIdentity.DefaultNameClaimType,
                        RequireExpirationTime = true,
                        ValidateLifetime = true
                    };
                });
        }

        private static void ConfigRequestLocalization(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                const string DefaultCultureName = "en-US";
                var csCulture = new CultureInfo("zh-CN");
                var enCulture = new CultureInfo("en-US")
                {
                    DateTimeFormat = new DateTimeFormatInfo
                    {
                        ShortDatePattern = "M/d/yyyy",
                        ShortTimePattern = "h:mm tt",
                        LongDatePattern = "MM/dd/yyyy",
                        LongTimePattern = "h:mm:ss tt"
                    }
                };

                options.DefaultRequestCulture = new RequestCulture(DefaultCultureName);
                options.SupportedCultures = new List<CultureInfo>
                {
                    enCulture
                };
                options.SupportedUICultures = new List<CultureInfo>
                {
                    enCulture,
                    csCulture,
                    new CultureInfo("zh-TW"),
                    new CultureInfo("zh-Hans"),
                    new CultureInfo("vi-VN"),
                    new CultureInfo("id-ID"),
                    new CultureInfo("th-TH"),
                };
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider { UIQueryStringKey = "language" }
                };
            });
        }

        private void ConfigProxy(IServiceCollection services, bool skipCheckClientIP)
        {
            var proxySetting = Configuration.GetSection("Proxy:BetList").Get<ProxyApiSetting>();

            services.AddProxy(options =>
            {
                const string ProxySegment = "/proxy";

                options.Timeout = TimeSpan.FromSeconds(int.Parse(Configuration["Proxy:Timeout"]));

                options.ValidateRequest = context => context.User.Identity.IsAuthenticated
                && (skipCheckClientIP || context.User.FindFirst("ClientIP")?.Value is not string clientIP
                    || string.Equals(context.Connection.RemoteIpAddress.ToString(), clientIP, StringComparison.InvariantCultureIgnoreCase));

                options.ShouldProxyRequest = originalRequest => originalRequest.Path.StartsWithSegments(ProxySegment);

                options.OnFailedValidation = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };

                options.PrepareRequest = async (originalRequest, message) =>
                {
                    var contextService = originalRequest.HttpContext
                    .RequestServices.GetRequiredService<IContextService>();

                    message.Headers.Add(proxySetting.TokenKey, proxySetting.TokenValue);
                    message.Headers.Add("Language", contextService.Language);
                    message.Headers.Add("X-SiteDBServer", contextService.SiteDBServer);

                    var accessToken = await contextService.AccessToken;
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                };

                options.PrepareUri = originalRequest =>
                {
                    var path = originalRequest.Path.Value;
                    var realPath = path[ProxySegment.Length..];
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