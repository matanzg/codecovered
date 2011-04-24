using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class StoreMap : ClassMapWithBehaviors<Store>
    {
        public StoreMap()
        {
            Map(s => s.Name).Not.Nullable();
            References(s => s.Contact);

            HasMany(s => s.Branches)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.All()
                .Inverse()
                .AsSet();
        }

        protected override IFluentBehavior<Store>[] Behaviors
        {
            get 
            { 
                return new IFluentBehavior<Store>[]
                                            {
                                                new IntEntityFluentBehavior<Store>()
                                            };
            }
        }
    }
}