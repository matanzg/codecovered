using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using GeoAPI.Geometries;
using NHibernate.Spatial.Type;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate.Spatial
{
    public class SqlServer2008GeometryTypeConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType.Equals(typeof(IGeometry)))
            {
                instance.CustomType<GeometryType>();
                instance.CustomSqlType("Geometry");
            }
        }
    }
}