using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Nishkriya.Properties;
using Nishkriya.Scraper;

namespace Nishkriya
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private Timer _scrapeTimer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetupScraper();
        }

        private void SetupScraper()
        {
            var interval = Settings.Default.ScrapeInterval*60*1000;
            _scrapeTimer = new Timer(x => new ScraperManager().Run(), null, 60, interval);
        }
    }
}