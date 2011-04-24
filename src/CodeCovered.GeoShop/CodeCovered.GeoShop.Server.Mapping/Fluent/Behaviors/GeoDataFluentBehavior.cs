using CodeCovered.GeoShop.Infrastructure.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors
{
    public class GeoDataFluentBehavior<T> : IFluentBehavior<T>
        where T : IHaveGeoData
    {
        public void ApplyMap(ClassMap<T> mapBase)
        {
            mapBase.Map(e => e.GeoData);
        }
    }
}