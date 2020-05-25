using System;
using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using SintefSecure.Framework.SintefSecure.AspNetCore.Filters;
using SintefSecureBoilerplate.DAL.Identity;
using SintefSecureBoilerplate.MVC.Settings;
#if(Authorization)
using Microsoft.EntityFrameworkCore;
#endif
#if(CORS)
using SintefSecureBoilerplate.MVC.Constants;
#endif

namespace SintefSecureBoilerplate.MVC
{
    public class Startup
    {
        /// <summary>
        /// Gets or sets the application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html
        /// </summary>
        private readonly IConfigurationRoot _configuration;

        /// <summary>
        /// The environment the application is running under. This can be Development, Staging or Production by default.
        /// To set the hosting environment on Windows:
        /// 1. On your server, right click 'Computer' or 'My Computer' and click on 'Properties'.
        /// 2. Go to 'Advanced System Settings'.
        /// 3. Click on 'Environment Variables' in the Advanced tab.
        /// 4. Add a new System Variable with the name 'ASPNETCORE_ENVIRONMENT' and value of Production, Staging or
        /// whatever you want. See http://docs.asp.net/en/latest/fundamentals/environments.html
        /// </summary>
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Gets or sets the port to use for HTTPS. Only used in the development environment.
        /// </summary>
        private readonly int? _sslPort;


        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default.</param>
        /// /// <param name="loggerFactory">The type used to configure the applications logging system.
        /// See http://docs.asp.net/en/latest/fundamentals/logging.html</param>
        public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;

            _configuration = new ConfigurationBuilder()
                .SetBasePath(_hostingEnvironment.ContentRootPath)
                // Add configuration from the config.json file.
                .AddJsonFile("config.json")
                // Add configuration from an optional config.development.json, config.staging.json or
                // config.production.json file, depending on the environment. These settings override the ones in the
                // config.json file.
                .AddJsonFile($"config.{_hostingEnvironment.EnvironmentName}.json", optional: true)
                // This reads the configuration keys from the secret store. This allows you to store connection strings
                // and other sensitive settings, so you don't have to check them into your source control provider.
                // Only use this in Development, it is not intended for Production use. See
                // http://docs.asp.net/en/latest/security/app-secrets.html
                // TODO: Fix assembly
                //.AddIf(
//                  _hostingEnvironment.IsDevelopment(),
  //                  x => x.AddUserSecrets<Startup>())
                // Add configuration specific to the Development, Staging or Production environments. This config can
                // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
                // override the ones in all of the above config files.
                // Note: To set environment variables for debugging navigate to:
                // Project Properties -> Debug Tab -> Environment Variables
                // Note: To get environment variables for the machine use the following command in PowerShell:
                // [System.Environment]::GetEnvironmentVariable("[VARIABLE_NAME]", [System.EnvironmentVariableTarget]::Machine)
                // Note: To set environment variables for the machine use the following command in PowerShell:
                // [System.Environment]::SetEnvironmentVariable("[VARIABLE_NAME]", "[VARIABLE_VALUE]", [System.EnvironmentVariableTarget]::Machine)
                // Note: Environment variables use a colon separator e.g. You can override the site title by creating a
                // variable named AppSettings:SiteTitle. See http://docs.asp.net/en/latest/security/app-secrets.html
#if(ApplicationInsights)
                .AddApplicationInsightsSettings(developerMode: !this.hostingEnvironment.IsProduction())
#endif
                .AddEnvironmentVariables()
                .Build();
            
            loggerFactory
                // Log to the console and Visual Studio debug window if in development mode.
                .AddIf(
                    _hostingEnvironment.IsDevelopment(),
                    x => x
                        .AddConsole(_configuration.GetSection("Logging"))
                        .AddDebug());

            if (_hostingEnvironment.IsDevelopment())
            {
                var launchConfiguration = new ConfigurationBuilder()
                    .SetBasePath(_hostingEnvironment.ContentRootPath)
                    .AddJsonFile(@"Properties\launchSettings.json")
                    .Build();
                _sslPort = launchConfiguration.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        /// <summary>
        /// Configures the services to add to the ASP.NET MVC 6 Injection of Control (IoC) container. This method gets
        /// called by the ASP.NET runtime. See
        /// http://blogs.msdn.com/b/webdev/archive/2014/06/17/dependency-injection-in-asp-net-vnext.aspx
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services) =>
            services
#if(ApplicationInsights)
                .AddApplicationInsightsTelemetry(this.configuration)
#endif
                .AddAntiforgerySecurely()
                .AddCaching()
                .AddOptions(_configuration)
                .AddRouting(
                    options =>
                    {
                        // Improve SEO by stopping duplicate URL's due to case differences or trailing slashes.
                        // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
                        // All generated URL's should append a trailing slash.
                        options.AppendTrailingSlash = true;
                        // All generated URL's should be lower-case.
                        options.LowercaseUrls = true;
                    })
#if(CORS)
// Add cross-origin resource sharing (CORS) services. See https://docs.asp.net/en/latest/security/cors.html
                .AddCors(
                    options =>
                    {
                        // Create named CORS policies here which you can consume using
                        // application.UseCors("PolicyName") or a [EnableCors("PolicyName")] attribute on your
                        // controller or action.
                        options.AddPolicy(
                            CorsPolicyName.AllowAny,
                            builder => builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
                    })
#endif
                .AddResponseCaching()
                // Add response compression to enable GZIP compression.
                .AddResponseCompression(
                    options =>
                    {
                        options.EnableForHttps = true;
                        // Add additional MIME types (other than the built in defaults) to enable GZIP compression for.
                        var responseCompressionSettings = _configuration.GetSection<ResponseCompressionSettings>(
                            nameof(ResponseCompressionSettings));
                        options.MimeTypes = ResponseCompressionDefaults
                            .MimeTypes
                            .Concat(responseCompressionSettings.MimeTypes);
                    })
                .Configure<GzipCompressionProviderOptions>(
                    options => options.Level = CompressionLevel.Optimal)
                // Add useful interface for accessing the ActionContext outside a controller.
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                // Add useful interface for accessing the HttpContext outside a controller.
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                // Add useful interface for accessing the IUrlHelper outside a controller.
                .AddScoped(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext))
                // Adds a filter which help improve search engine optimization (SEO).
                .AddSingleton<RedirectToCanonicalUrlAttribute>()
                // Add many MVC services to the services container.
                .AddMvc(
                    options =>
                    {
                        // Controls how controller actions cache content from the config.json file.
                        var cacheProfileSettings = _configuration.GetSection<CacheProfileSettings>();
                        foreach (var keyValuePair in cacheProfileSettings.CacheProfiles)
                        {
                            options.CacheProfiles.Add(keyValuePair);
                        }

                        // Adds a filter which help improve search engine optimization (SEO).
                        options.Filters.AddService(typeof(RedirectToCanonicalUrlAttribute));
                    })
                // Configures the JSON output formatter to use camel case property names like 'propertyName' instead of
                // pascal case 'PropertyName' as this is the more common JavaScript/JSON style.
                .AddJsonOptions(
                    x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .Services
                .AddCustomServices()
                // DATABASE CONNECTIONS
#if(Authorization)
        .AddDbContext<IdentityDatabaseContext>(options =>
        {
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            options.UseOpenIddict();
        })
#endif
            
                .BuildServiceProvider();

        /// <summary>
        /// Configures the application and HTTP request pipeline. Configure is called after ConfigureServices is
        /// called by the ASP.NET runtime.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="loggerfactory">The logger factory.</param>
        /// <param name="identityContext">The database context for the identity database</param>
        public void Configure(IApplicationBuilder application, ILoggerFactory loggerfactory,
            IdentityDatabaseContext identityContext)
        {
            //__BEGIN_INITIALIZE_DATABASES_
            if (_hostingEnvironment.IsDevelopment())
            {
                IdentityDatabaseIntializer.InitializeDevelopmentEnvironment(identityContext);
            }
            else
            {
                IdentityDatabaseIntializer.InitializeProductionEnvironment(identityContext);
            }

            //__END_INTIALIZE_DATABASES_

            application
                // Removes the Server HTTP header from the HTTP response for marginally better security and performance.
                .UseNoServerHttpHeader()
                // Require HTTPS to be used across the whole site. Also set a custom port to use for SSL in
                // Development. The port number to use is taken from the launchSettings.json file which Visual
                // Studio uses to start the application.
                .UseRewriter(
                    new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, _sslPort))
#if(CORS)
                .UseCors(CorsPolicyName.AllowAny)
#endif
                .UseResponseCaching()
                .UseResponseCompression()
                .UseStaticFilesWithCacheControl(_configuration)
                .UseCookiePolicy()
                .UseIfElse(
                    _hostingEnvironment.IsDevelopment(),
                    x => x
                        .UseDebugging()
                        .UseDeveloperErrorPages(),
                    x => x.UseErrorPages())
                .UseStrictTransportSecurityHttpHeader()
                .UsePublicKeyPinsHttpHeader()
                .UseContentSecurityPolicyHttpHeader(_sslPort, _hostingEnvironment)
                .UseSecurityHttpHeaders()
                .UseAuthentication()
                .UseMvc();
        }
    }
}