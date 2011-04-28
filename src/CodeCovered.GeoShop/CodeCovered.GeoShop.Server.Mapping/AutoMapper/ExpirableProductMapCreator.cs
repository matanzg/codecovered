using System;
using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Server.Entities;

namespace CodeCovered.GeoShop.Server.Mapping.AutoMapper
{
    public class ExpirableProductMapCreator : ICreateMapAfter<ProductMapCreator>
    {
        public void CreateMap(IConfiguration configuration)
        {
            configuration.CreateMap<ExpirableProduct, ExpirableProductDto>()
                .ForMember(dest => dest.ExpDate, o => o.MapFrom(src => src.ExpirationDate));
        }
    }
}