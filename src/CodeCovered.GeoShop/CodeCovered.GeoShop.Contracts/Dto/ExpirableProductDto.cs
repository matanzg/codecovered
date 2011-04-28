using System;
using System.Runtime.Serialization;

namespace CodeCovered.GeoShop.Contracts.Dto
{
    [DataContract]
    public class ExpirableProductDto : ProductDto
    {
        [DataMember]
        public DateTime ExpDate { get; set; }
    }
}