using Owin;
using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ThreadSafeDBService
{
    /// <summary>
    /// Description for Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Description for Startup:Configuration().
        /// </summary>
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            /*
            //static settings
            appBuilder.Use()
                .MaxBandwidth(10000) //bps
                .MaxConcurrentRequests(10)
                .ConnectionTimeout(TimeSpan.FromSeconds(10))
                .MaxQueryStringLength(15) //Unescaped QueryString
                .MaxRequestContentLength(15)
                .MaxUrlLength(20);
           */    

            /*
            //dynamic settings
            appBuilder.Use()
                .MaxBandwidth(() => 10000) //bps
                .MaxConcurrentRequests(() => 10)
                .ConnectionTimeout(() => TimeSpan.FromSeconds(10))
                .MaxQueryStringLength(() => 15)
                .MaxRequestContentLength(() => 15)
                .MaxUrlLength(() => 20)
               
    */

            

            // To Support Cross Origin Request support
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            // To Support JSON
            config.Formatters.JsonFormatter.SupportedMediaTypes
            .Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{StoredProcName}",
                defaults: new { StoredProcName = RouteParameter.Optional }
            );
            
            appBuilder.UseWebApi(config);

        }


    }
}