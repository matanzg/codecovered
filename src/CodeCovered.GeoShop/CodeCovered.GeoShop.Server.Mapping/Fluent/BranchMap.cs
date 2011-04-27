using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class BranchMap : ClassMap<Branch>
    {
        public BranchMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);

            Map(x => x.Name);
            Map(x => x.GeoData);

            References(x => x.Store);
            References(x => x.Manager);

            Component(x => x.Address).ColumnPrefix("Address");

            HasMany(b => b.Inventory)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.All()
                .Inverse()
                .AsSet();
        }
    }
}