using AutoMapper;

namespace CodeCovered.GeoShop.Infrastructure.AutoMapper
{
    public interface ICreateMap
    {
        void CreateMap(IConfiguration configuration);
    }

    public interface ICreateMapAfter<T> : ICreateMap 
        where T : ICreateMap
    {
    }
}