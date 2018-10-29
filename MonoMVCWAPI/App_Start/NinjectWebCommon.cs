using Ninject.Web.Common.WebHost;
using Ninject.Web.WebApi;
using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
//using NHibernate;
using Ninject.Extensions.Conventions;
using System.Web.Http;
using System.Data.Entity;
using MonoMVCWAPI.Models;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MonoMVCWAPI.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MonoMVCWAPI.NinjectWebCommon), "Stop")]
namespace MonoMVCWAPI
{

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            // kernel.Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession()).InRequestScope();
            // kernel.Bind<ISessionFactory>().ToProvider<avatarOPERA_DMS.Areas.DMS.NHibernateSessionFactoryMyProvider>().InSingletonScope();

            kernel.Bind<DbContext>().To<ApplicationDbContext>().InRequestScope();

            RegisterServices(kernel);
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x => x
               .FromThisAssembly() // 1
                                   // .IncludingNonePublicTypes()
            .SelectAllClasses()//.InNamespaceOf<MyService>() // 2
            .BindAllInterfaces() // 3
            .Configure(b => b.InSingletonScope())); // 4
        }

    }
}
