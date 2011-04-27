using System;
using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using CodeCovered.GeoShop.Server.Entities;

namespace CodeCovered.GeoShop.Server.Mapping.AutoMapper
{
    public class ProductMapCreator : ICreateMap
    {
        public void CreateMap(IConfiguration configuration)
        {
            configuration.CreateMap<Product, ProductDto>()
                .ForMember(d => d.code, o => o.MapFrom(src => src.Id))
                .ForMember(d => d.desc, o => o.MapFrom(src => src.Description))
                .ForMember(d => d.cat_id, o => o.MapFrom(src => src.Category.Id))
                .ForMember(d => d.price, o => o.MapFrom(src => src.UnitPrice));

            configuration.CreateMap<ProductDto, Product>()
                .ConstructUsingNHibernate(() => NHibernateSession.Current, dto => dto.code)
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Version, o => o.Ignore())
                .ForMember(d => d.Category, o => o.LoadNHibernateReference(
                    () => NHibernateSession.Current,
                    src => src.cat_id).As<Category>())
                .ForMember(d => d.Description, o => o.MapFrom(src => src.desc))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(src => src.price));
        }
    }
}