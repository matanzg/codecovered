using System;
using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Server.Entities;

namespace CodeCovered.GeoShop.Server.Mapping.AutoMapper
{
    public class CategoryMapCreator : ICreateMap
    {
        public void CreateMap(IConfiguration configuration)
        {
            configuration.CreateMap<Category, CategoryDto>();
        }
    }
}