using System.Web.Http;
using WebApiContrib.MessageHandlers;

namespace Lvgaga
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new RequireHttpsHandler());
        }
    }
}
