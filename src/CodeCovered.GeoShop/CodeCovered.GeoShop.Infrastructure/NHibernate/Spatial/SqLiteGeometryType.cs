using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.IO;
using NHibernate;
using NHibernate.Spatial.Type;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate.Spatial
{
    public class SqLiteGeometryType : GeometryTypeBase<string>
    {
        public SqLiteGeometryType()
            : base(NHibernateUtil.String)
        {
        }

        protected override string FromGeometry(object value)
        {
            return value != null ? ((IGeometry)value).AsText() : null;
        }

        protected override IGeometry ToGeometry(object value)
        {
            return value != null ? new WKTReader().Read((string)value) : null;
        }
    }
}