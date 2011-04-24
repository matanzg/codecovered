using System;
using CodeCovered.GeoShop.Infrastructure;
using CodeCovered.GeoShop.Infrastructure.Entities;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace CodeCovered.GeoShop.Entities
{
    public class City : IntEntity, IHaveGeoData
    {
        public virtual IGeometry GeoData { get; set; }
        public virtual Polygon Region
        {
            get { return GeoData as Polygon; }
            set { GeoData = value; }
        }

        public virtual string Name { get; set; }
        public virtual Country Country { get; protected set; }

        public virtual void AssignCountry(Country country)
        {
            Validate.ThatArgumentNotNull(() => country);

            if (Country == country) 
                return;

            Country = country;
            Country.AddCity(this);
        }
    }
}