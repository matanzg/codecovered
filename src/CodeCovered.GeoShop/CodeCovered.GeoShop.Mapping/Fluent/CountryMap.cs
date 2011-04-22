using System;
using System.Linq;
using CodeCovered.GeoShop.Mapping.Fluent.Behaviors;
using CodeCovered.GeoShop.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent
{
    public class CountryMap : ClassMapWithBehaviors<Country>
    {
        public CountryMap()
        {
            Map(c => c.Name).Not.Nullable();

            HasMany(c => c.Cities)
                .Access.CamelCaseField(Prefix.Underscore)
                .AsSet()
                .Inverse()
                .Cascade.All();
        }

        protected override IFluentBehavior<Country>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<Country>[]
                           {
                               new GeoDataFluentBehavior<Country>(),
                               new IntEntityFluentBehavior<Country>()
                           };
            }
        }
    }
}