using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AcklenAvenue.Data.NHibernate;
using AutoMapper;
using AutoMapper.Mappers;
using BootstrapMvcSample;
using BootstrapSupport;
using FluentNHibernate.Cfg.Db;
using MiniAmazon.Data;
using MiniAmazon.Domain;
using MiniAmazon.Web.Infrastructure;
using NHibernate;
using NHibernate.Context;
using Ninject;
using Ninject.Web.Common;

namespace MiniAmazon.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : NinjectHttpApplication
    {
        /*protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            BootstrapMvcSample.ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);
        }*/

        public static ISessionFactory SessionFactory = CreateSessionFactory();

        public MvcApplication()
        {
            BeginRequest += MvcApplication_BeginRequest;
            EndRequest += MvcApplication_EndRequest;
        }

        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            CurrentSessionContext.Unbind(SessionFactory).Dispose();
        }

        private void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            CurrentSessionContext.Bind(SessionFactory.OpenSession());
        }

        public static ISessionFactory CreateSessionFactory()
        {
            MsSqlConfiguration databaseConfiguration = MsSqlConfiguration.MsSql2008.ShowSql().
                ConnectionString(x => x.FromConnectionStringWithKey("MiniAmazon"));
            ISessionFactory sessionFactory = new SessionFactoryBuilder(new MappingScheme(), databaseConfiguration)
                .Build();

            return sessionFactory;
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapBundleConfig.RegisterBundles(BundleTable.Bundles);
            ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfiguration.Configure();
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<IRepository>().To<Repository>();
            kernel.Bind<ISession>().ToMethod(x => SessionFactory.GetCurrentSession());
            kernel.Bind<IMappingEngine>().ToConstant(Mapper.Engine);
            
            

            return kernel;
        }
    }
}