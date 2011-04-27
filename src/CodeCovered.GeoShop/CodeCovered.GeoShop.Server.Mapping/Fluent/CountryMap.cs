using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);
            Map(x => x.GeoData);
            Map(x => x.Name).Not.Nullable();

            HasMany(x => x.Cities)
                .Access.CamelCaseField(Prefix.Underscore)
                .AsSet()
                .Inverse()
                .Cascade.All();
        }
    }
}