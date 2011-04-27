using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class InventoryItemMap : ClassMap<InventoryItem>
    {
        public InventoryItemMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);
            Map(x => x.Amount);
            References(x => x.Branch);
            References(x => x.Product);
        }
    }
}