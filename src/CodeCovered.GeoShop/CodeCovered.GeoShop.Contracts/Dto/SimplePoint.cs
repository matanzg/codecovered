using System.Runtime.Serialization;

namespace CodeCovered.GeoShop.Contracts.Dto
{
    [DataContract]
    public class SimplePoint
    {
        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }
    }
}