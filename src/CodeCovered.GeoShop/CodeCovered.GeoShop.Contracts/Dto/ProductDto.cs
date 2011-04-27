using System.Runtime.Serialization;

namespace CodeCovered.GeoShop.Contracts.Dto
{
    [DataContract]
    public class ProductDto
    {
        [DataMember]
        public int code { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string desc { get; set; }

        [DataMember]
        public double price { get; set; }

        [DataMember]
        public int cat_id { get; set; }

        [DataMember]
        public string CategoryDescription { get; set; }
    }
}