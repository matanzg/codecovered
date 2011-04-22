using CodeCovered.GeoShop.Infrastructure.NHibernate.Conventions;
using CodeCovered.GeoShop.Infrastructure.NHibernate.Spatial;
using CodeCovered.GeoShop.Mapping.Fluent;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Spatial.Dialect;
using NHibernate.Tool.hbm2ddl;

namespace CodeCovered.GeoShop.Mapping
{
    public static class ConfigurationFactory
    {
        public static Configuration GetConfiguration(
            ShowSqlSetting showSqlSetting = ShowSqlSetting.DontShowSql,
            ExportSchemaSetting exportSchemaSetting = ExportSchemaSetting.DontExport,
            string connectionStringName = "DbConfiguration")
        {
            return Fluently.Configure()
                .Database(GetDatabaseConfiguration(connectionStringName, showSqlSetting))
                .ExposeConfiguration(cfg => AddConfiguration(cfg, exportSchemaSetting, showSqlSetting))
                .Mappings(AddMappings)
                .BuildConfiguration();
        }

        private static void AddMappings(MappingConfiguration cfg)
        {
            cfg.FluentMappings.AddFromAssemblyOf<CountryMap>()
                .Conventions.Add<PluralClassConvention>()
                .Conventions.Add<UnderscorePropertyConvention>()
                .Conventions.Add<SqlServer2008GeographyTypeConvention>();
                // We're using geography, you can also use geometry 
                //.Add<SqlServer2008GeometryTypeConvention>() 
        }

        private static void AddConfiguration(Configuration cfg, ExportSchemaSetting exportSchemaSetting, ShowSqlSetting showSqlSetting)
        {
            var export = exportSchemaSetting == ExportSchemaSetting.Export;
            var script = showSqlSetting == ShowSqlSetting.ShowSql;
            new SchemaExport(cfg).Create(script && export, export);
        }

        private static IPersistenceConfigurer GetDatabaseConfiguration(string connectionStringName, ShowSqlSetting showSqlSetting)
        {
            var dbConfig = MsSqlConfiguration.MsSql2008;

            // We're using geography, you can also use geometry 
            //dbConfig.Dialect<MsSql2008GeometryDialect>();

            dbConfig.Dialect<MsSql2008GeographyDialect>();
            dbConfig.ConnectionString(s => s.FromConnectionStringWithKey(connectionStringName));

            if (showSqlSetting == ShowSqlSetting.ShowSql)
                dbConfig.ShowSql();

            return dbConfig;
        }
    }

    public enum ShowSqlSetting
    {
        ShowSql,
        DontShowSql
    }

    public enum ExportSchemaSetting
    {
        Export,
        DontExport
    }
}