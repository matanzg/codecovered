using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent.Behaviors
{
    public abstract class ClassMapWithBehaviors<T> : ClassMap<T>
    {
        protected abstract IFluentBehavior<T>[] Behaviors { get; }

        protected ClassMapWithBehaviors()
        {
            ApplyBehaviors();
        }

        private void ApplyBehaviors()
        {
            foreach (var fluentBehavior in Behaviors)
            {
                fluentBehavior.ApplyMap(this);
            }
        }
    }
}