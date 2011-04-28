using System.Runtime.Serialization;

namespace CodeCovered.GeoShop.Contracts.Dto
{
    [DataContract]
    public class BranchDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ManagerName { get; set; }

        [DataMember]
        public int ManagerId { get; set; }

        [DataMember]
        public int StoreId { get; set; }

        [DataMember]
        public SimplePoint Location { get; set; }
    }
}