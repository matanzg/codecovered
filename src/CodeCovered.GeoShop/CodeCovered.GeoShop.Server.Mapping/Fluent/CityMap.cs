using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class CityMap : ClassMap<City>
    {
        public CityMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);
            Map(x => x.GeoData);
            Map(x => x.Name);
            References(x => x.Country);
        }
    }
}