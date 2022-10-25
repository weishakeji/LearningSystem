using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Song.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //资源下载的路由
            routes.MapRoute(
                name: "Download",
                url: "down/{id}",
                defaults: new { controller = "Down", action = "index", id = UrlParameter.Optional }
            );
            //风格模版的路由
            string homepage = WeiSha.Core.Template.Homepage; //默认首页
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Web", action = homepage, id = UrlParameter.Optional }
            );

            ////后台（超管）的路由
            //routes.MapRoute(
            //    name: "Manage",
            //    url: "Manage/{controller}/{action}/{id}",
            //    defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            //);

        }
    }
}
