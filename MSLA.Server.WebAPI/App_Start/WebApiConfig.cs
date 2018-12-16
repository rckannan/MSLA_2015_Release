using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Converters;

namespace MSLA.Server.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.DependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.CreateKernel());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
             name: "BOApi",
             routeTemplate: "api/{controller}/{DocType}/{Doc_ID}"//,
                                                                 //defaults: new { DocMaster = RouteParameter.Optional, id = RouteParameter.Optional }
         );

        //    config.Routes.MapHttpRoute(
        //    name: "BOApiSess",
        //    routeTemplate: "api/{controller}/{DocType}/{Doc_ID}/{Session_ID}"//,
        //                                                        //defaults: new { DocMaster = RouteParameter.Optional, id = RouteParameter.Optional }
        //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Add(jsonFormatter);

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add( new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd\\THH:mm:ss.fffK" });
        }
    }
}
