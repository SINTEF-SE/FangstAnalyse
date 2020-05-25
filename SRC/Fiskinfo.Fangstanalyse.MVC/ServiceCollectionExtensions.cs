using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using SintefSecure.Framework.SintefSecure.AspNetCore.Filters;
using SintefSecureBoilerplate.MVC.Constants;
using SintefSecureBoilerplate.MVC.Services.BrowserConfig;
using SintefSecureBoilerplate.MVC.Services.Manifest;
using SintefSecureBoilerplate.MVC.Services.OpenSearch;
using SintefSecureBoilerplate.MVC.Services.Robots;
using SintefSecureBoilerplate.MVC.Services.Sitemap;
using SintefSecureBoilerplate.MVC.Services.SitemapPinger;
using SintefSecureBoilerplate.MVC.Settings;

namespace SintefSecureBoilerplate.MVC
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures caching for the application. Registers the <see cref="IDistributedCache"/> and
        /// <see cref="IMemoryCache"/> types with the services collection or IoC container. The
        /// <see cref="IDistributedCache"/> is intended to be used in cloud hosted scenarios where there is a shared
        /// cache, which is shared between multiple instances of the application. Use the <see cref="IMemoryCache"/>
        /// otherwise.
        /// </summary>
        public static IServiceCollection AddCaching(this IServiceCollection services) =>
            services
                // Adds IMemoryCache which is a simple in-memory cache.
                .AddMemoryCache()
                // Adds IDistributedCache which is a distributed cache shared between multiple servers. This adds a
                // default implementation of IDistributedCache which is not distributed. See below:
                .AddDistributedMemoryCache();

        // Uncomment the following line to use the Redis implementation of IDistributedCache. This will
        // override any previously registered IDistributedCache service.
        // Redis is a very fast cache provider and the recommended distributed cache provider.
        // .AddDistributedRedisCache(
        //     options =>
        //     {
        //     });
        // Uncomment the following line to use the Microsoft SQL Server implementation of IDistributedCache.
        // Note that this would require setting up the session state database.
        // Redis is the preferred cache implementation but you can use SQL Server if you don't have an alternative.
        // .AddSqlServerCache(
        //     x =>
        //     {
        //         x.ConnectionString = "Server=.;Database=ASPNET5SessionState;Trusted_Connection=True;";
        //         x.SchemaName = "dbo";
        //         x.TableName = "Sessions";
        //     });   

        /// <summary>
        /// Configures the anti-forgery tokens for better security. See:
        /// http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        public static IServiceCollection AddAntiforgerySecurely(this IServiceCollection services)
        {
            return services.AddAntiforgery(
                options =>
                {
                    // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f". This adds a little
                    // security through obscurity and also saves sending a few characters over the wire.
                    options.Cookie.Name = "f";

                    // Rename the form input name from "__RequestVerificationToken" to "f" for the same reason above
                    // e.g. <input name="__RequestVerificationToken" type="hidden" value="..." />
                    options.FormFieldName = "f";

                    // Rename the Anti-Forgery HTTP header from RequestVerificationToken to X-XSRF-TOKEN. X-XSRF-TOKEN
                    // is not a standard but a common name given to this HTTP header popularized by Angular.
                    options.HeaderName = "X-XSRF-TOKEN";

                    // If you have enabled SSL/TLS. Uncomment this line to ensure that the Anti-Forgery cookie requires
                    // SSL /TLS to be sent across the wire.
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
        }

        /// Configures the settings by binding the contents of the config.json file to the specified Plain Old CLR
        /// Objects (POCO) and adding <see cref="IOptions{TOptions}"/> objects to the services collection.
        /// <param name="services">The services collection or IoC container.</param>
        /// <param name="configuration">Gets or sets the application configuration, where key value pair settings are
        /// stored.</param>
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                // Adds IOptions<AppSettings> to the services container.
                .Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)))
                // Adds IOptions<SitemapSettings> to the services container.
                .Configure<SitemapSettings>(configuration.GetSection(nameof(SitemapSettings)))
                // Adds IOptions<CacheProfileSettings> to the services container.
                .Configure<CacheProfileSettings>(configuration.GetSection(nameof(CacheProfileSettings)));
        }

        /// <summary>
        /// Configures custom services to add to the ASP.NET MVC 6 Injection of Control (IoC) container.
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IBrowserConfigService, BrowserConfigService>();
            services.AddScoped<IManifestService, ManifestService>();
            services.AddScoped<IOpenSearchService, OpenSearchService>();
            services.AddScoped<IRobotsService, RobotsService>();
            services.AddScoped<ISitemapService, SitemapService>();
            services.AddScoped<ISitemapPingerService, SitemapPingerService>();

            // Add your own custom services here e.g.

            // Singleton - Only one instance is ever created and returned.
            // services.AddSingleton<IExampleService, ExampleService>();

            // Scoped - A new instance is created and returned for each request/response cycle.
            // services.AddScoped<IExampleService, ExampleService>();

            // Transient - A new instance is created and returned each time.
            // services.AddTransient<IExampleService, ExampleService>();

            return services;
        }

        /// <summary>
        /// Configures the settings by binding the contents of the appsettings.json file to the specified Plain Old CLR
        /// Objects (POCO) and adding <see cref="IOptions{TOptions}"/> objects to the services collection.
        /// </summary>
        public static IServiceCollection AddCustomOptions(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                // Adds IOptions<CacheProfileSettings> to the services container.
                .Configure<CacheProfileSettings>(configuration.GetSection(nameof(CacheProfileSettings)));

        /// <summary>
        /// Adds response compression to enable GZIP compression of responses.
        /// </summary>
        public static IServiceCollection AddCustomResponseCompression(
            this IServiceCollection services,
            IConfigurationRoot configuration) =>
            services
                .AddResponseCompression(
                    options =>
                    {
                        // Enable response compression over HTTPS connections.
                        options.EnableForHttps = true;
                        // Add additional MIME types (other than the built in defaults) to enable GZIP compression for.
                        var responseCompressionSettings = configuration.GetSection<ResponseCompressionSettings>(
                            nameof(ResponseCompressionSettings));
                        options.MimeTypes = ResponseCompressionDefaults
                            .MimeTypes
                            .Concat(responseCompressionSettings.MimeTypes);
                    })
                .Configure<GzipCompressionProviderOptions>(
                    options => options.Level = CompressionLevel.Optimal);

        /// <summary>
        /// Add custom routing settings which determines how URL's are generated.
        /// </summary>
        public static IServiceCollection AddCustomRouting(this IServiceCollection services) =>
            services.AddRouting(
                options =>
                {
                    // Improve SEO by stopping duplicate URL's due to case differences or trailing slashes.
                    // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
                    // All generated URL's should append a trailing slash.
                    options.AppendTrailingSlash = true;
                    // All generated URL's should be lower-case.
                    options.LowercaseUrls = true;
                });


        public static IMvcCoreBuilder AddCustomMvcOptions(
            this IMvcCoreBuilder builder,
            IConfigurationRoot configuration,
            IHostingEnvironment hostingEnvironment) =>
            builder.AddMvcOptions(
                options =>
                {
                    // Controls how controller actions cache content from the appsettings.json file.
                    var cacheProfileSettings = configuration.GetSection<CacheProfileSettings>();
                    foreach (var keyValuePair in cacheProfileSettings.CacheProfiles)
                    {
                        options.CacheProfiles.Add(keyValuePair);
                    }

                    if (hostingEnvironment.IsDevelopment())
                    {
                        // Lets you pass a format parameter into the dalQuery string to set the response type:
                        // e.g. ?format=application/json. Good for debugging.
                        options.Filters.Add(new FormatFilterAttribute());
                    }

                    // Check model state for null or invalid models and automatically return a 400 Bad Request.
                    options.Filters.Add(new ValidateModelStateAttribute());

                    // Remove string and stream output formatters. These are not useful for an API serving JSON or XML.
                    options.OutputFormatters.RemoveType<StreamOutputFormatter>();
                    options.OutputFormatters.RemoveType<StringOutputFormatter>();

                    // Returns a 406 Not Acceptable if the MIME type in the Accept HTTP header is not valid.
                    options.ReturnHttpNotAcceptable = true;
                });

        /// <summary>
        /// Adds customized JSON serializer settings.
        /// </summary>
        public static IMvcCoreBuilder AddCustomJsonOptions(this IMvcCoreBuilder builder) =>
            builder.AddJsonOptions(
                options =>
                {
                    // Parse dates as DateTimeOffset values by default. You should prefer using DateTimeOffset over
                    // DateTime everywhere. Not doing so can cause problems with time-zones.
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    // Output enumeration values as strings in JSON.
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

        /// <summary>
        /// Add cross-origin resource sharing (CORS) services and configures named CORS policies. See
        /// https://docs.asp.net/en/latest/security/cors.html
        /// </summary>
        public static IMvcCoreBuilder AddCustomCors(this IMvcCoreBuilder builder) =>
            builder.AddCors(
                options =>
                {
                    // Create named CORS policies here which you can consume using application.UseCors("PolicyName")
                    // or a [EnableCors("PolicyName")] attribute on your controller or action.
                    options.AddPolicy(
                        CorsPolicyName.AllowAny,
                        x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });
    }
}