using GeoAPI.Geometries;

namespace CodeCovered.GeoShop.Infrastructure.Entities
{
    public interface IHaveGeoData
    {
        IGeometry GeoData { get; set; }
    }
}