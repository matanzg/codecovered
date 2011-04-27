using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Gender).Not.Nullable();

            Component(x => x.HomeAddress).ColumnPrefix("Home");
        }
    }
}