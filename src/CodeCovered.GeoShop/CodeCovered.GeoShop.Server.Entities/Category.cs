using CodeCovered.GeoShop.Infrastructure.Entities;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class Category : IntEntity
    {
        public virtual string Description { get; set; }
    }
}