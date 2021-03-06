﻿using System.Web.Mvc;
using System.Web.Routing;

namespace SchoolsNearMe.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new
                {
                    allaspx = @".*\.aspx(/.*)?"
                });
            routes.IgnoreRoute("{*robotstxt}", new
                {
                    robotstxt = @"(.*/)?robots.txt(/.*)?"
                });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                    {
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional
                    }
                );
        }
    }
}