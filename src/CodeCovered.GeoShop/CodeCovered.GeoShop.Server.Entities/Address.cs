namespace CodeCovered.GeoShop.Server.Entities
{
    public class Address
    {
        public City City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public int PostalCode { get; set; }

        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.City, City) && Equals(other.Street, Street) && other.Number == Number && other.PostalCode == PostalCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Address)) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (City != null ? City.GetHashCode() : 0);
                result = (result*397) ^ (Street != null ? Street.GetHashCode() : 0);
                result = (result*397) ^ Number;
                result = (result*397) ^ PostalCode;
                return result;
            }
        }
    }
}