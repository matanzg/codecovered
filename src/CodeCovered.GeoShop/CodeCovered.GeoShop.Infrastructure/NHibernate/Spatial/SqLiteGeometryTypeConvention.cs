using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using GeoAPI.Geometries;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate.Spatial
{
    public class SqLiteGeometryTypeConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType.Equals(typeof(IGeometry)))
            {
                instance.CustomType<SqLiteGeometryType>();
            }
        }
    }
}