using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace zdjecia
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Włącznie Cors
            config.EnableCors();
            // Konfiguracja i usługi składnika Web API

            // Trasy składnika Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
