using CodeCovered.GeoShop.Infrastructure.Entities;

namespace CodeCovered.GeoShop.Entities
{
    public class Person : IntEntity
    {
        public virtual string Name { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Address HomeAddress { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}