using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using SintefSecure.Framework.SintefSecure.AspNetCore.Filters;
using SintefSecureBoilerplate.MVC.Constants;
using SintefSecureBoilerplate.MVC.Constants.HomeController;
using SintefSecureBoilerplate.MVC.Services.BrowserConfig;
using SintefSecureBoilerplate.MVC.Services.Manifest;
using SintefSecureBoilerplate.MVC.Services.OpenSearch;
using SintefSecureBoilerplate.MVC.Services.Robots;
using SintefSecureBoilerplate.MVC.Services.Sitemap;
using SintefSecureBoilerplate.MVC.Settings;

namespace SintefSecureBoilerplate.MVC.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly IOptions<AppSettings> _appSettings;
        private readonly IBrowserConfigService _browserConfigService;
        private readonly IManifestService _manifestService;
        private readonly IOpenSearchService _openSearchService;
        private readonly IRobotsService _robotsService;
        private readonly ISitemapService _sitemapService;

        #endregion

        #region Constructors

        public HomeController(
            IBrowserConfigService browserConfigService,
            IManifestService manifestService,
            IOpenSearchService openSearchService,
            IRobotsService robotsService,
            ISitemapService sitemapService,
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _browserConfigService = browserConfigService;
            _manifestService = manifestService;
            _openSearchService = openSearchService;
            _robotsService = robotsService;
            _sitemapService = sitemapService;
        }

        #endregion

        [HttpGet("", Name = HomeControllerRoute.GetIndex)]
        public IActionResult Index()
        {
            return View(HomeControllerAction.Index);
        }

        [HttpGet("about", Name = HomeControllerRoute.GetAbout)]
        public IActionResult About()
        {
            return View(HomeControllerAction.About);
        }

        [HttpGet("contact", Name = HomeControllerRoute.GetContact)]
        public IActionResult Contact()
        {
            return View(HomeControllerAction.Contact);
        }
        
        [Route("search", Name = HomeControllerRoute.GetSearch)]
        public IActionResult Search(string query)
        {
            // You can implement a proper search function here and add a Search.cshtml page.
            // return this.View(HomeControllerAction.Search);

            // Or you could use Google Custom Search (https://cse.google.co.uk/cse) to index your site and display your
            // search results in your own page.

            // For simplicity we are just assuming your site is indexed on Google and redirecting to it.
            return Redirect(string.Format(
                "https://www.google.co.uk/?q=site:{0} {1}",
                Url.AbsoluteRouteUrl(HomeControllerRoute.GetIndex),
                query));
        }

        /// <summary>
        /// Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins
        /// the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and
        /// https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        [NoTrailingSlash]
        [ResponseCache(CacheProfileName = CacheProfileName.BrowserConfigXml)]
        [Route("browserconfig.xml", Name = HomeControllerRoute.GetBrowserConfigXml)]
        public ContentResult BrowserConfigXml()
        {
            return Content(_browserConfigService.GetBrowserConfigXml(), ContentType.Xml, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the manifest JSON for the current site. This allows you to customize the icon and other browser
        /// settings for Chrome/Android and FireFox (FireFox support is coming). See https://w3c.github.io/manifest/
        /// for the official W3C specification. See http://html5doctor.com/web-manifest-specification/ for more
        /// information. See https://developer.chrome.com/multidevice/android/installtohomescreen for Chrome's
        /// implementation.
        /// </summary>
        /// <returns>The manifest JSON for the current site.</returns>
        [NoTrailingSlash]
        [ResponseCache(CacheProfileName = CacheProfileName.ManifestJson)]
        [Route("manifest.json", Name = HomeControllerRoute.GetManifestJson)]
        public ContentResult ManifestJson()
        {
            return Content(_manifestService.GetManifestJson(), ContentType.Json, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the Open Search XML for the current site. You can customize the contents of this XML here. The open
        /// search action is cached for one day, adjust this time to whatever you require. See
        /// http://www.hanselman.com/blog/CommentView.aspx?guid=50cc95b1-c043-451f-9bc2-696dc564766d
        /// http://www.opensearch.org
        /// </summary>
        /// <returns>The Open Search XML for the current site.</returns>
        [NoTrailingSlash]
        [ResponseCache(CacheProfileName = CacheProfileName.OpenSearchXml)]
        [Route("opensearch.xml", Name = HomeControllerRoute.GetOpenSearchXml)]
        public IActionResult OpenSearchXml()
        {
            return Content(_openSearchService.GetOpenSearchXml(), ContentType.Xml, Encoding.UTF8);
        }

        /// <summary>
        /// Tells search engines (or robots) how to index your site.
        /// The reason for dynamically generating this code is to enable generation of the full absolute sitemap URL
        /// and also to give you added flexibility in case you want to disallow search engines from certain paths. The
        /// sitemap is cached for one day, adjust this time to whatever you require. See
        /// </summary>
        /// <returns>The robots text for the current site.</returns>
        [NoTrailingSlash]
        [ResponseCache(CacheProfileName = CacheProfileName.RobotsText)]
        [Route("robots.txt", Name = HomeControllerRoute.GetRobotsText)]
        public IActionResult RobotsText()
        {
            return Content(_robotsService.GetRobotsText(), ContentType.Text, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the sitemap XML for the current site. You can customize the contents of this XML from the
        /// <see cref="SitemapService"/>. The sitemap is cached for one day, adjust this time to whatever you require.
        /// http://www.sitemaps.org/protocol.html
        /// </summary>
        /// <param name="index">The index of the sitemap to retrieve. <c>null</c> if you want to retrieve the root
        /// sitemap file, which may be a sitemap index file.</param>
        /// <returns>The sitemap XML for the current site.</returns>
        [NoTrailingSlash]
        [Route("sitemap.xml", Name = HomeControllerRoute.GetSitemapXml)]
        public async Task<IActionResult> SitemapXml(int? index = null)
        {
            var content = await _sitemapService.GetSitemapXml(index);
            if (content == null)
            {
                return BadRequest("Sitemap index is out of range.");
            }

            return Content(content, ContentType.Xml, Encoding.UTF8);
        }
    }
}