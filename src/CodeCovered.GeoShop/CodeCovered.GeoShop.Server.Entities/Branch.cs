using System.Collections.Generic;
using System.Linq;
using CodeCovered.GeoShop.Infrastructure;
using CodeCovered.GeoShop.Infrastructure.Entities;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class Branch : IntEntity, IHaveGeoData
    {
        public Branch()
        {
            _inventory = new HashSet<InventoryItem>();
        }

        public virtual string Name { get; set; }
        public virtual Person Manager { get; set; }
        public virtual Address Address { get; set; }
        public virtual Store Store { get; protected set; }

        private ICollection<InventoryItem> _inventory;
        public virtual IEnumerable<InventoryItem> Inventory
        {
            get { return _inventory.AsEnumerable(); }
        }

        public virtual IGeometry GeoData { get; set; }
        public virtual Point Location
        {
            get { return GeoData as Point; }
            set { GeoData = value; }
        }

        public virtual void AssignStore(Store store)
        {
            Validate.ThatArgumentNotNull(() => store);

            if (Store == store)
                return;

            Store = store;
            Store.AddBranch(this);
        }

        public virtual void AddProductToInventory(Product product, int amount)
        {
            Validate.ThatArgumentNotNull(() => product);

            var item = _inventory.Where(ii => ii.Product.Id == product.Id).FirstOrDefault();

            if (item == null)
                _inventory.Add(new InventoryItem { Amount = amount, Product = product, Branch = this });
            else
                item.Amount += amount;
        }
    }
}