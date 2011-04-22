using CodeCovered.GeoShop.Infrastructure.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent.Behaviors
{
    public class IntEntityFluentBehavior<T> : IFluentBehavior<T>
        where T : IntEntity
    {
        public void ApplyMap(ClassMap<T> mapBase)
        {
            mapBase.Id(p => p.Id).GeneratedBy.Native();
            mapBase.Version(p => p.Version);
        }
    }
}