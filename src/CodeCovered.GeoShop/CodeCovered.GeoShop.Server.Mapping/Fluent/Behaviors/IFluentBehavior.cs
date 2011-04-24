using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors
{
    public interface IFluentBehavior<T>
    {
        void ApplyMap(ClassMap<T> mapBase);
    }
}