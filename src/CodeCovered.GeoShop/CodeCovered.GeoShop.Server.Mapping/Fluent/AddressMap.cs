using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class AddressMap : ComponentMap<Address>
    {
        public AddressMap()
        {
            References(x => x.City);
            Map(x => x.Number);
            Map(x => x.PostalCode);
            Map(x => x.Street);
        }
    }
}