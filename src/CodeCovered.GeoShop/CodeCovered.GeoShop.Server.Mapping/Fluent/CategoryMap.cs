using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);
            Version(x => x.Version);
            Map(x => x.Description);
        }
    }
}