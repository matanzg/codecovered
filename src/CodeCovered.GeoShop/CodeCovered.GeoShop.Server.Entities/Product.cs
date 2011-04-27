using CodeCovered.GeoShop.Infrastructure.Entities;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class Product : IntEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual double UnitPrice { get; set; }
        public virtual Category Category { get; set; }
    }
}