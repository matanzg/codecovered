using System.Runtime.Serialization;

namespace CodeCovered.GeoShop.Contracts.Dto
{
    [DataContract]
    public class CategoryDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
