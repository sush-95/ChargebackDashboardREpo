using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BankDashboard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "app/{controller}/{action}/{id}",
                defaults: new { controller = "LogIn", action = "LogIn", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default1",
                url: "",
                defaults: new { controller = "LogIn", action = "LogIn", id = UrlParameter.Optional }
            );
        }
    }
}
