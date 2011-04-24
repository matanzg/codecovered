using System;
using System.Linq;
using CodeCovered.GeoShop.Infrastructure.Factories;
using CodeCovered.GeoShop.Server.Entities;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate.Linq;
using NHibernate.Spatial.Criterion;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Integration.Tests
{
    [TestFixture]
    public class SqlServer2008GeoTests : IntegrationBaseTest
    {
        [SetUp]
        public new void Setup()
        {
            CleanDatabaseData();
        }

        [Test]
        public void make_sure_the_database_is_querying_geography_correctly()
        {
            // arrange
            var center1 = Default.GeometryFactory.CreatePoint(new Coordinate(35, 32)).As<Point>();
            var country1 = InsertCountryAround(center1, 1);
            var center2 = Default.GeometryFactory.CreatePoint(new Coordinate(36, 32)).As<Point>();
            var country2 = InsertCountryAround(center2);
            var center3 = Default.GeometryFactory.CreatePoint(new Coordinate(36, 33)).As<Point>();
            InsertCountryAround(center3);

            // act
            var propName = ReflectionHelper.GetProperty<Country, IGeometry>(s => s.GeoData).Name;
            var result = Session.CreateCriteria<Country>()
                .Add(SpatialRestrictions.Intersects(propName, center2))
                .List<Country>();

            // assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(e => e.Id == country1.Id));
            Assert.That(result.Any(e => e.Id == country2.Id));
        }

        protected override bool ShowSql
        {
            get { return true; }
        }
    }
}