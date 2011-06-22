using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using AutoMapper;
using AutoMapper.Mappers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Infrastructure.Factories;
using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping;
using CodeCovered.GeoShop.Server.Mapping.AutoMapper;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate;
using ISession = NHibernate.ISession;

namespace CodeCovered.GeoShop.Server.WcfServices
{
    public class Global : System.Web.HttpApplication
    {
        public static IWindsorContainer Container { get; set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigureContainer();
        }

        private static void ConfigureContainer()
        {
            Container = new WindsorContainer();

            Container.Register(Component.For<IMappingEngine>()
                        .UsingFactoryMethod(ConfigureMappingEngine)
                        .LifeStyle.Singleton)
                    .Register(Component.For<ISessionFactory>()
                        .UsingFactoryMethod(ConfigureSessionFactory)
                        .LifeStyle.Singleton);
        }

        private static IMappingEngine ConfigureMappingEngine()
        {
            var config = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers());
            ApplyMap.On(config).AddFromAssemblyOf<ProductMapCreator>().Apply();
            config.AssertConfigurationIsValid();

            return new MappingEngine(config);

            // Another way, Useful if we want to reuse the 
            // static Mapper class' engine or we already have it
            // spread around in our code

            //Mapper.Initialize(cfg =>
            //    ApplyMap.On(cfg).AddFromAssemblyOf<ProductMapCreator>().Apply());

            //return Mapper.Engine;
        }

        private static ISessionFactory ConfigureSessionFactory()
        {
            var configuration = ConfigurationFactory.GetConfiguration(
                ShowSqlSetting.ShowSql,
                ExportSchemaSetting.Export,
                "GeoShopExpress");

            var sf = configuration.BuildSessionFactory();

            Stubs.InsertSomeData(sf);
            return sf;
        }
    }
}