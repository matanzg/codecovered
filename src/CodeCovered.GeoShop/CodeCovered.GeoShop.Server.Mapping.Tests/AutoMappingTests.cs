using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Infrastructure.AutoMapper;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.AutoMapper;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Server.Mapping.Tests
{
    [TestFixture]
    public class AutoMappingTests : DomainBaseTest
    {
        [SetUp]
        public void Setup()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => ApplyMap.On(cfg).AddFromAssemblyOf<ProductMapCreator>().Apply());
            NHibernateSession.InitCurrentSession(Session);
        }

        [Test]
        public void AssertAllConfigurations()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void product_exists_in_db_use_existing_product()
        {
            var p = new Product
                        {
                            Description = "desc",
                            Name = "name",
                            UnitPrice = 10,
                        };

            Session.Save(p);
            Session.Flush();
            Session.Clear();

            var dto = Mapper.Map<Product, ProductDto>(p);
            dto.name = "newName";
            var entity = Mapper.Map<ProductDto, Product>(dto);
            Session.Flush();
            Session.Clear();

            var result = Session.Get<Product>(p.Id);
            Assert.That(dto.CategoryDescription, Is.EqualTo("desc"));
            Assert.That(result.Name, Is.EqualTo("newName"));
            Assert.That(result.Description, Is.EqualTo("desc"));
        }

        [Test]
        public void product_does_not_exist_in_db_create_new_product()
        {
            var dto = new ProductDto
                          {
                              desc = "desc",
                              name = "newName",
                              cat_id = 1,
                              price = 100
                          };

            var entity = Mapper.Map<ProductDto, Product>(dto);
            Session.Save(entity);
            Session.Flush();
            Session.Clear();

            var result = Session.Get<Product>(entity.Id);
            Assert.That(result.Name, Is.EqualTo("newName"));
            Assert.That(result.Description, Is.EqualTo("desc"));
        }
    }
}