using System;
using System.Collections.Generic;
using System.Linq;
using CodeCovered.GeoShop.Infrastructure.Entities;
using CodeCovered.GeoShop.Infrastructure.Factories;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Spatial.Criterion;

namespace CodeCovered.GeoShop.Server.BL
{
    public class GeoLocator
    {
        private readonly ISession _session;

        public GeoLocator(ISession session)
        {
            _session = session;
        }

        public IEnumerable<T> Locate<T>(Point center, double buffer)
            where T : class, IHaveGeoData
        {
            var propName = ReflectionHelper.GetProperty<T, IGeometry>(x => x.GeoData).Name;
            var coordinates = center.Buffer(buffer).Coordinates.Reverse().ToArray();
            var polygon = Default.GeometryFactory.CreatePolygon(new LinearRing(coordinates), null);

            var results = _session.CreateCriteria<T>()
                .Add(SpatialRestrictions.Intersects(propName, polygon))
                .List<T>();

            return results;
        }
    }
}