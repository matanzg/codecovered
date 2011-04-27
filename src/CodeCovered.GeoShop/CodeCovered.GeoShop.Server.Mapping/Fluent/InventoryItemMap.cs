using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class InventoryItemMap : ClassMapWithBehaviors<InventoryItem>
    {
        public InventoryItemMap()
        {
            Map(ii => ii.Amount);
            References(ii => ii.Branch);
            References(ii => ii.Product);
        }

        protected override IFluentBehavior<InventoryItem>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<InventoryItem>[]
                           {
                               new IntEntityFluentBehavior<InventoryItem>()
                           };
            }
        }
    }
}