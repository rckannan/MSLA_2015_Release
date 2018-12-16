using Microsoft.Owin;
using System.Web.Http;

//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MSLA.Server.WebAPI.NinjectWebCommon), "Start")]
//[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MSLA.Server.WebAPI.NinjectWebCommon), "Stop")]
//[assembly: OwinStartup(typeof(MSLA.Server.WebAPI.Startup))]
namespace MSLA.Server.WebAPI
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Owin;
    using System.Web.Routing;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);

            //IAppBuilder app;

            //Startup.Configuration(app);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        } 

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            //var kernel = new StandardKernel();
            //try
            //{
            //    kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            //    kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            //    RegisterServices(kernel);
            //    return kernel;
            //}
            //catch
            //{
            //    kernel.Dispose();
            //    throw;
            //}
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            // register the dependency resolver passing the kernel container
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var containerConfigurator = new NinjectConfigurator();
            containerConfigurator.Configure(kernel);

            //GlobalConfiguration.Configuration.MessageHandlers.Add(kernel.Get<BasicAuthenticationMessageHandler>());
            //GlobalConfiguration.Configuration.MessageHandlers.Add(kernel.Get<Identity.HTTP.AuthenticationHandler>());
        }
    }



}