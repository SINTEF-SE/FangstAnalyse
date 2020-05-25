using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using SintefSecureBoilerplate.MVC.Constants.HomeController;

namespace SintefSecureBoilerplate.MVC.Services.BrowserConfig
{
    public class BrowserConfigService : IBrowserConfigService
    {
        private readonly IUrlHelper _urlHelper;

        public BrowserConfigService(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins
        /// the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and
        /// https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        public string GetBrowserConfigXml()
        {
            // The URL to the 70x70 small tile image.
            var square70X70LogoUrl = _urlHelper.Content("~/img/icons/mstile-70x70.png");
            // The URL to the 150x150 medium tile image.
            var square150X150LogoUrl = _urlHelper.Content("~/img/icons/mstile-150x150.png");
            // The URL to the 310x310 large tile image.
            var square310X310LogoUrl = _urlHelper.Content("~/img/icons/mstile-310x310.png");
            // The URL to the 310x150 wide tile image.
            var wide310X150LogoUrl = _urlHelper.Content("~/img/icons/mstile-310x150.png");
            // The colour of the tile. This colour only shows if part of your images above are transparent.
            const string tileColour = "#1E1E1E";
            // Update the tile every 1440 minutes. Defines the frequency, in minutes, between poll requests. Must be
            // one of the following values: 30 (1/2 Hour), 60 (1 Hour), 360 (6 Hours), 720 (12 Hours), 1440 (24 Hours).
            const int frequency = 1440;
            // Control notification cycling. Must be one of the following values:
            // 0: (default if there's only one notification) No tiles show notifications.
            // 1: (default if there are multiple notifications) Notifications cycle for all tile sizes.
            // 2: Notifications cycle for medium tiles only.
            // 3: Notifications cycle for wide tiles only.
            // 4: Notifications cycle for large tiles only.
            // 5: Notifications cycle for medium and wide tiles.
            // 6: Notifications cycle for medium and large tiles.
            // 7: Notifications cycle for large and wide tiles.
            const int cycle = 1;

            XDocument document = new XDocument(
                new XElement("browserconfig",
                    new XElement("msapplication",
                        new XElement("tile",
                            new XElement("square70x70logo",
                                new XAttribute("src", square70X70LogoUrl)),
                            new XElement("square150x150logo",
                                new XAttribute("src", square150X150LogoUrl)),
                            new XElement("square310x310logo",
                                new XAttribute("src", square310X310LogoUrl)),
                            new XElement("wide310x150logo",
                                new XAttribute("src", wide310X150LogoUrl)),
                            new XElement("TileColor", tileColour)),
                        new XElement("notification",
                            new XElement("polling-uri",
                            new XElement("frequency", frequency),
                            new XElement("cycle", cycle))))));

            return document.ToString(Encoding.UTF8);
        }
    }
}