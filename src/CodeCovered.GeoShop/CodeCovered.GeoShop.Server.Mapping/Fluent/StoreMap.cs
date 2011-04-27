using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);

            Map(x => x.Name).Not.Nullable();
            References(x => x.Contact);

            HasMany(x => x.Branches)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.All()
                .Inverse()
                .AsSet();
        }
    }
}