using System;
using CodeCovered.GeoShop.Entities;
using CodeCovered.GeoShop.Mapping.Fluent.Behaviors;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent
{
    public class BranchMap : ClassMapWithBehaviors<Branch>
    {
        public BranchMap()
        {
            Component(b => b.Address).ColumnPrefix("Address");
            
            References(b => b.Store);
            References(b => b.Manager);
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