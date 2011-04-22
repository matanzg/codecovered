using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate
{
    public abstract class NHibernateBaseTest
    {
        protected ISessionFactory SessionFactory;
        protected Configuration Configuration;
        protected ISession Session;
        protected ITransaction Transaction;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            Configuration = GetConfiguration();
            SessionFactory = Configuration.BuildSessionFactory();
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            SessionFactory.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            Session = SessionFactory.OpenSession();
            Transaction = Session.BeginTransaction();

            if (ExportSchemaBeforeEachTest)
            {
                var se = new SchemaExport(Configuration);
                se.Execute(ShowSql, true, false, Session.Connection, SchemaExportTextWriter);
            }
        }

        [TearDown]
        public void Teardown()
        {
            if (Transaction != null && Transaction.IsActive)
                Transaction.Rollback();

            Session.Dispose();
        }

        protected virtual Configuration GetConfiguration()
        {
            return Fluently.Configure()
                .Database(ConfigureDatabase())
                .Mappings(ConfigureMappings)
                .ExposeConfiguration(AddCustomConfigurations)
                .BuildConfiguration();
        }

        protected virtual IPersistenceConfigurer ConfigureDatabase()
        {
            var db = SQLiteConfiguration.Standard.InMemory();

            if (ShowSql)
                db.ShowSql();

            return db;
        }

        protected virtual void AddCustomConfigurations(Configuration cfg)
        {
        }

        protected abstract void ConfigureMappings(MappingConfiguration cfg);

        protected abstract bool ExportSchemaBeforeEachTest { get; }

        protected virtual TextWriter SchemaExportTextWriter
        {
            get { return null; }
        }

        protected virtual bool ShowSql
        {
            get { return false; }
        }
    }
}