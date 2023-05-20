using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DaiPhucVinh
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Login",
            //    url: "login",
            //    defaults: new { controller = "User", action = "Login" }
            //);
            //routes.MapRoute(
            //    name: "Logout",
            //    url: "logout",
            //    defaults: new { controller = "User", action = "Logout" }
            //);           
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DaiPhucVinh.Controllers" }
                );
        }
    }
}
