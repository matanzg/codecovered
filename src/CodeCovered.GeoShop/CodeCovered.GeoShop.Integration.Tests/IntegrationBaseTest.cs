using System.Linq;
using CodeCovered.GeoShop.Infrastructure.Factories;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping;
using FluentNHibernate.Cfg;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate.Cfg;

namespace CodeCovered.GeoShop.Integration.Tests
{
    public abstract class IntegrationBaseTest : NHibernateBaseTest
    {
        #region NHibernate Settings

        protected virtual ExportSchemaSetting Export
        {
            get { return ExportSchemaSetting.DontExport; }
        }

        protected virtual string DbConfiguration
        {
            get { return "GeoShopExpress"; }
        }

        protected override Configuration GetConfiguration()
        {
            var showSql = ShowSql ? ShowSqlSetting.ShowSql : ShowSqlSetting.DontShowSql;

            return ConfigurationFactory.GetConfiguration(showSql, Export, DbConfiguration);
        }

        protected override void ConfigureMappings(MappingConfiguration cfg)
        {
            // gave a full configuration, no need to define more mappings
        }

        protected override bool ExportSchemaBeforeEachTest
        {
            // Since these are integration tests, we don't want to export the database
            // If we want to initiate an export we can change the parameter sent to GetConfiguration
            get { return false; }
        }

        #endregion

        protected Country InsertCountryAround(Point center, double buffer = 0.1, string name = "Neverland")
        {
            var coordinates = center.Buffer(buffer).Coordinates.Reverse().ToArray();
            var polygon = Default.GeometryFactory.CreatePolygon(new LinearRing(coordinates), null);
            var country = new Country { GeoData = polygon, Name = name };
            Session.Save(country);
            Session.Flush();
            Session.Clear();

            return country;
        }

        protected void CleanDatabaseData()
        {
            Session.CreateQuery("delete from Country").ExecuteUpdate();
        }
    }
}