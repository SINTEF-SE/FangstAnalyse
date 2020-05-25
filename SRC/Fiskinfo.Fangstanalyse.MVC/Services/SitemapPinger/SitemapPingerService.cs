using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using SintefSecureBoilerplate.MVC.Constants.HomeController;
using SintefSecureBoilerplate.MVC.Settings;
#if(ApplicationInsights)
using Microsoft.ApplicationInsights;
#endif

namespace SintefSecureBoilerplate.MVC.Services.SitemapPinger
{
    public class SitemapPingerService : ISitemapPingerService
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<SitemapPingerService> _logger;
        private readonly IOptions<SitemapSettings> _sitemapSettings;
#if(ApplicationInsights)
        private readonly TelemetryClient telemetryClient;
#endif
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapPingerService"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default.</param>
        /// <param name="logger">The <see cref="SitemapPingerService"/> logger.</param>
        /// <param name="sitemapSettings">The sitemap settings.</param>
        /// <param name="urlHelper">The URL helper.</param>
        public SitemapPingerService(
            IHostingEnvironment hostingEnvironment,
            ILogger<SitemapPingerService> logger,
            IOptions<SitemapSettings> sitemapSettings,
#if(ApplicationInsights)
            TelemetryClient telemetryClient,
#endif
            IUrlHelper urlHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _sitemapSettings = sitemapSettings;
            _urlHelper = urlHelper;
#if(ApplicationInsights)
            this.telemetryClient = telemetryClient;
#endif
            _httpClient = new HttpClient();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send (or 'ping') the URL of this sites sitemap.xml file to search engines like Google, Bing and Yahoo,
        /// This method should be called each time the sitemap changes. Google says that 'We recommend that you
        /// resubmit a Sitemap no more than once per hour.' The way we 'ping' our sitemap to search engines is
        /// actually an open standard See
        /// http://www.sitemaps.org/protocol.html#submit_ping
        /// You can read the sitemap ping documentation for the top search engines below:
        /// Google - http://googlewebmastercentral.blogspot.co.uk/2014/10/best-practices-for-xml-sitemaps-rssatom.html
        /// Bing - http://www.bing.com/webmaster/help/how-to-submit-sitemaps-82a15bd4.
        /// Yahoo - https://developer.yahoo.com/search/siteexplorer/V1/ping.html
        /// </summary>
        public async Task PingSearchEngines()
        {
#if(ApplicationInsights)
            this.telemetryClient.TrackEvent("PingSitemapToSearchEngines");
#endif
            if (_hostingEnvironment.IsProduction())
            {
                foreach (var sitemapPingLocation in _sitemapSettings.Value.SitemapPingLocations)
                {
                    var sitemapUrl = _urlHelper.AbsoluteRouteUrl(HomeControllerRoute.GetSitemapXml).TrimEnd('/');
                    var url = sitemapPingLocation + WebUtility.UrlEncode(sitemapUrl);
                    var response = await _httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpRequestException exception = new HttpRequestException(string.Format(
                            CultureInfo.InvariantCulture,
                            "Pinging search engine {0}. Response status code does not indicate success: {1} ({2}).",
                            url,
                            (int) response.StatusCode,
                            response.ReasonPhrase));
#if(ApplicationInsights)
                        this.telemetryClient.TrackException(exception, new Dictionary<string, string>() { { "Url", url } });
#endif
                        _logger.LogError("Error while pinging site-map to search engines.", exception);
                    }
                }
            }
        }

        #endregion
    }
}