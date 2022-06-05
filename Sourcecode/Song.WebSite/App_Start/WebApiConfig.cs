using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Song.WebSite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{classname}/{act}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            // 下面这句可省略
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new System.Net.Http.Formatting.QueryStringMapping("datatype", "json", "application/json"));
        }
    }
}
