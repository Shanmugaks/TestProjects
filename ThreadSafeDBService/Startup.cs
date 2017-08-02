using Owin;
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
                defaults: new { NumberToConvert = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);

        }


    }
}