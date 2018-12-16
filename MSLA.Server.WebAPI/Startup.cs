using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using System.Web.Routing;

[assembly: OwinStartup(typeof(MSLA.Server.WebAPI.Startup))]
namespace MSLA.Server.WebAPI
{
    public partial class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //Enable Ninjet
            //NinjectWebCommon.Start();

            ConfigureAuth(app);


            app.Use(async (context, next) => {
                var user = context.Authentication.User;
                await next();
            });

            //config.Filters.Add(new CustomAuthorize());

            //config.DependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.CreateKernel());

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);


            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            


        }
    }
}