using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class ProductMap : ClassMapWithBehaviors<Product>
    {
        public ProductMap()
        {
            Map(p => p.Name);
            Map(p => p.Description);
            Map(p => p.UnitPrice);

            References(p => p.Category);
        }

        protected override IFluentBehavior<Product>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<Product>[]
                           {
                               new IntEntityFluentBehavior<Product>()
                           };
            }
        }
    }
}