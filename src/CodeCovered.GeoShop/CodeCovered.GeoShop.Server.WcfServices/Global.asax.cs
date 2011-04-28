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
        }

        private static ISessionFactory ConfigureSessionFactory()
        {
            var configuration = ConfigurationFactory.GetConfiguration(
                ShowSqlSetting.ShowSql,
                ExportSchemaSetting.Export,
                "GeoShopExpress");

            var sf = configuration.BuildSessionFactory();

            InsertSomeData(sf);
            return sf;
        }

        private static void InsertSomeData(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var israel = new Country { Name = "Israel" };
                var telaviv = new City { Name = "TelAviv" };
                israel.AddCity(telaviv);

                session.Save(israel);

                var address = new Address { City = telaviv, Number = 100, PostalCode = 101, Street = "200" };
                var matan = new Person
                              {
                                  Gender = Gender.Male,
                                  Name = "Matan",
                                  HomeAddress = address
                              };

                session.Save(matan);

                var store = new Store { Contact = matan, Name = "Main Office"};
                var branch1 = new Branch
                                  {
                                      Address = address,
                                      Manager = matan,
                                      Name = "branch1",
                                      Location = (Point) Default.GeometryFactory.CreatePoint(new Coordinate(32, 34))
                                  };

                var branch2 = new Branch
                                  {
                                      Address = address,
                                      Manager = matan,
                                      Name = "branch2",
                                      Location = (Point) Default.GeometryFactory.CreatePoint(new Coordinate(32.5, 34.5))
                                  };

                store.AddBranch(branch1);
                store.AddBranch(branch2);

                var milk = new Product {Name = "milk", UnitPrice = 10, Description = "moo"};
                var bread = new Product { Name = "bread", UnitPrice = 8, Description = "poof" };

                session.Save(milk);
                session.Save(bread);

                branch1.AddProductToInventory(milk, 10);
                branch1.AddProductToInventory(bread, 15);
                branch2.AddProductToInventory(milk, 20);

                session.Save(store);
                tx.Commit();
            }
        }
    }
}