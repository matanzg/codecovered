using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.UnitPrice);

            References(x => x.Category);
        }
    }
}