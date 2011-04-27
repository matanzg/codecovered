using AutoMapper;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Server.Mapping.AutoMapper;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Server.Mapping.Tests
{
    [TestFixture]
    public class AutoMappingTests
    {
        [SetUp]
        public void Setup()
        {
            Mapper.Reset();
        }

        [Test]
        public void AssertAllConfigurations()
        {
            Mapper.Initialize(cfg => ApplyMap.On(cfg).AddFromAssemblyOf<ProductMapCreator>().Apply());
            Mapper.AssertConfigurationIsValid();
        }
    }
}