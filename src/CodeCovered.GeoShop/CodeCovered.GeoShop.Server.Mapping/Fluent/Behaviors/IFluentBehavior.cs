using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent.Behaviors
{
    public interface IFluentBehavior<T>
    {
        void ApplyMap(ClassMap<T> mapBase);
    }
}