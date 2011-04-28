using System;
using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Infrastructure.Factories;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace CodeCovered.GeoShop.Server.Mapping.AutoMapper
{
    public class SimplePointMapCreator : ICreateMap
    {
        public void CreateMap(IConfiguration configuration)
        {
            configuration.CreateMap<Point, SimplePoint>();

            configuration.CreateMap<SimplePoint, Point>()
                .ConstructUsing(src => CreatePoint(src.X, src.Y))
                .ForAllMembers(d => d.Ignore());
        }

        private static Point CreatePoint(double x, double y)
        {
            return Default.GeometryFactory.CreatePoint(new Coordinate(x, y)) as Point;
        }
    }
}