using System;
using System.Linq;
using CodeCovered.GeoShop.Infrastructure.NHibernate.Spatial;
using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using GeoAPI.Geometries;
using NHibernate;
using NHibernate.Spatial.Dialect;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Integration.Tests
{
    [TestFixture]
    public class InsertGeoData : IntegrationBaseTest
    {
        [Test, Ignore]
        public void InsertDataFromShapeFile()
        {
            using (var sessionFactory = GetSessionFactory())
            using (var session = sessionFactory.OpenSession())
            {
                var states = session.QueryOver<State>().List();
                var cities = states.Select(s => new City { GeoData = s.Polygon, Name = s.Name });
                cities.Select(c => Session.Save(c)).ToList();
            }
            
            Session.Flush();
            Transaction.Commit();
        }

        private ISessionFactory GetSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .Dialect<MsSql2008GeographyDialect>()
                .ConnectionString(s => s.FromConnectionStringWithKey("GeoShopSDE")))
                .Mappings(m => m.FluentMappings.Add<StateMap>().Conventions.Add<SqlServer2008GeographyTypeConvention>())
                .BuildSessionFactory();

        }
    }

    public class State
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IGeometry Polygon { get; set; }
    }

    public class StateMap : ClassMap<State>
    {
        public StateMap()
        {
            Table("sde.TL_2010_US_COUNTY10");
            Id(s => s.Id).Column("OBJECTID");
            Map(s => s.Name).Column("NAMELSAD10");
            Map(s => s.Polygon).Column("Shape");
            ReadOnly();
        }
    }
}