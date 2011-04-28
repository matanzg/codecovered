using System;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class ExpirableProduct : Product
    {
        public virtual DateTime ExpirationDate { get; set; }
    }
}