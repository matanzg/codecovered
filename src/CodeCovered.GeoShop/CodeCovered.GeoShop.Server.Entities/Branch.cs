using CodeCovered.GeoShop.Infrastructure;
using CodeCovered.GeoShop.Infrastructure.Entities;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class Branch : IntEntity, IHaveGeoData
    {
        public virtual IGeometry GeoData { get; set; }
        public virtual Point Location
        {
            get { return GeoData as Point; }
            set { GeoData = value; }
        }

        public virtual Person Manager { get; set; }
        public virtual Address Address { get; set; }
        public virtual Store Store { get; protected set; }

        public virtual void AssignStore(Store store)
        {
            Validate.ThatArgumentNotNull(() => store);

            if (Store == store)
                return;

            Store = store;
            Store.AddBranch(this);
        }
    }
}