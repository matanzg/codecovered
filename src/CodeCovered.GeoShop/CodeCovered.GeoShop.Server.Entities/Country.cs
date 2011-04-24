using System.Linq;
using System.Collections.Generic;
using CodeCovered.GeoShop.Infrastructure;
using CodeCovered.GeoShop.Infrastructure.Entities;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace CodeCovered.GeoShop.Server.Entities
{
    public class Country : IntEntity, IHaveGeoData
    {
        public Country()
        {
            _cities = new HashSet<City>();
        }

        public virtual IGeometry GeoData { get; set; }
        public virtual Polygon Border
        {
            get { return GeoData as Polygon; }
            set { GeoData = value; }
        }

        public virtual string Name { get; set; }

        protected ICollection<City> _cities;
        public virtual IEnumerable<City> Cities
        {
            get { return _cities.AsEnumerable(); }
        }

        public virtual void AddCity(City city)
        {
            Validate.ThatArgumentNotNull(() => city);

            if (!_cities.Contains(city))
            {
                _cities.Add(city);
                city.AssignCountry(this);
            }
        }
    }
}