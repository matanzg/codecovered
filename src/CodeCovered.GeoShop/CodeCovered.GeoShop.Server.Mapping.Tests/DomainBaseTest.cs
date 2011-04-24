using System.Linq;
using CodeCovered.GeoShop.Infrastructure.Factories;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using CodeCovered.GeoShop.Infrastructure.NHibernate.Spatial;
using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent;
using FluentNHibernate.Cfg;
using GisSharpBlog.NetTopologySuite.Geometries;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Server.Mapping.Tests
{
    [TestFixture]
    public abstract class DomainBaseTest : NHibernateBaseTest
    {
        #region NHibernate Configurations

        protected override void ConfigureMappings(MappingConfiguration cfg)
        {
            cfg.FluentMappings.AddFromAssemblyOf<CountryMap>()
                .Conventions.Add<SqLiteGeometryTypeConvention>();
        }

        protected override bool ExportSchemaBeforeEachTest
        {
            // Since these are unit tests, we have to export the database before each test for sqlite
            get { return true; }
        }

        #endregion

        #region Default Entities

        protected Address GetDefaultAddress()
        {
            return new Address();
        }

        protected Branch GetDefaultBranch()
        {
            return new Branch();
        }

        protected City GetDefaultCity()
        {
            return new City();
        }

        protected Country GetDefaultCountry(string name = "Neverland")
        {
            return new Country
                       {
                           Name = name,
                           Border = GetDefaultPolygon(),
                       };
        }

        protected Person GetDefaultPerson(string name = "Matan", Gender gender = Gender.Male, Address address = null)
        {
            return new Person
                       {
                           Gender = gender,
                           HomeAddress = address,
                           Name = name
                       };
        }

        protected Store GetDefaultStore(string name = "Steam", Person contact = null)
        {
            return new Store
                       {
                           Name = name,
                           Contact = contact
                       };
        }

        protected Point GetDefaultPoint(double x = 10, double y = 10)
        {
            return (Point)Default.GeometryFactory.CreatePoint(new Coordinate(x, y));
        }

        protected Polygon GetDefaultPolygon()
        {
            var point = GetDefaultPoint();
            return GetDefaultPolygon(point);
        }

        protected Polygon GetDefaultPolygon(Point center, double buffer = 0.1)
        {
            var ring = new LinearRing(center.Buffer(buffer).Coordinates.Reverse().ToArray());
            return (Polygon)Default.GeometryFactory.CreatePolygon(ring, null);
        }

        #endregion

    }
}