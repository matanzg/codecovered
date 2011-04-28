using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class ExpirableProductMap : SubclassMap<ExpirableProduct>
    {
        public ExpirableProductMap()
        {
            Map(x => x.ExpirationDate);
        }
    }
}