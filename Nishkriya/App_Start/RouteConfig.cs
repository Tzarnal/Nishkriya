using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nishkriya
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index Route",
                url: "{controller}",
                defaults: new { controller = "Home", action = "Index",}
                );

            routes.MapRoute(
                name: "View Route",
                url: "{controller}/{id}",
                defaults: new { controller = "Home", action = "Details", id = UrlParameter.Optional }
                );

            //Default Route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}