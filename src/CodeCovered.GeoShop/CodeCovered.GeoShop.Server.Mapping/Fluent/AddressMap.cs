using CodeCovered.GeoShop.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent
{
    public class AddressMap : ComponentMap<Address>
    {
        public AddressMap()
        {
            References(a => a.City);
            Map(x => x.Number);
            Map(x => x.PostalCode);
            Map(x => x.Street);
        }
    }
}