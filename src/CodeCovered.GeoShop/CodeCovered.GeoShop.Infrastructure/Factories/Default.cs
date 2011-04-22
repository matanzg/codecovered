using System.Configuration;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace CodeCovered.GeoShop.Infrastructure.Factories
{
    public static class Default
    {
        public static IGeometryFactory GeometryFactory;

        static Default()
        {
            var pm = Geometry.DefaultFactory.PrecisionModel as PrecisionModel;
            var defaultSrid = ConfigurationManager.AppSettings["defaultSrid"] ?? "4326";
            GeometryFactory = new GeometryFactory(pm, int.Parse(defaultSrid));
        }
    }
}