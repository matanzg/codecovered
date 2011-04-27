using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class BranchMap : ClassMapWithBehaviors<Branch>
    {
        public BranchMap()
        {
            Component(b => b.Address).ColumnPrefix("Address");

            Map(b => b.Name);

            References(b => b.Store);
            References(b => b.Manager);

            HasMany(b => b.Inventory)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.All()
                .Inverse()
                .AsSet();
        }

        protected override IFluentBehavior<Branch>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<Branch>[]
                                            {
                                                new GeoDataFluentBehavior<Branch>(),
                                                new IntEntityFluentBehavior<Branch>()
                                            };
            }
        }
    }
}