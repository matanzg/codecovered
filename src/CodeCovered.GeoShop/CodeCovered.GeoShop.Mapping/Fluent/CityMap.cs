using CodeCovered.GeoShop.Entities;
using FluentNHibernate.Mapping;
using CodeCovered.GeoShop.Mapping.Fluent.Behaviors;

namespace CodeCovered.GeoShop.Mapping.Fluent
{
    public class CityMap : ClassMapWithBehaviors<City>
    {
        public CityMap()
        {
            Map(c => c.Name);
            References(c => c.Country);
        }

        protected override IFluentBehavior<City>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<City>[]
                           {
                               new IntEntityFluentBehavior<City>(),
                               new GeoDataFluentBehavior<City>()
                           };
            }
        }
    }
}