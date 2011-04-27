using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeCovered.GeoShop.Infrastructure.Entities;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class InventoryItem : IntEntity
    {
        public virtual Product Product { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual int Amount { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }
}
