using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using CodeCovered.GeoShop.Infrastructure.Entities;
using CodeCovered.GeoShop.Server.Entities;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace CodeCovered.GeoShop.Server.Mapping.Tests
{
    [TestFixture]
    public class PersistenceSpecificationTests : DomainBaseTest
    {
        protected override bool ShowSql
        {
            get { return true; }
        }
        [Test]
        public void TestBranchMap()
        {
            var manager = GetDefaultPerson();
            var address = GetDefaultAddress();
            var point = GetDefaultPoint();
            var store = GetDefaultStore();

            var product1 = GetDefaultProdcut();
            var product2 = GetDefaultProdcut();

            var inventory = new[] {
                                    new InventoryItem {Amount = 1, Product = product1},
                                    new InventoryItem {Amount = 2, Product = product2},
                                };


            Session.Save(product1);
            Session.Save(product2);

            new PersistenceSpecification<Branch>(Session)
                .CheckProperty(c => c.Name, "matan")
                .CheckProperty(c => c.Address, address)
                .CheckProperty(c => c.Location, point)
                .CheckReference(c => c.Manager, manager)
                .CheckReference(c => c.Store, store, (b, s) => b.AssignStore(s))
                //.CheckList(c => c.Inventory, inventory, new KeyEquilityComparer(), (c, ii) => c.AddProductToInventory(ii.Product, ii.Amount))
                .VerifyTheMappings();
        }

        [Test]
        public void TestCategoryMap()
        {
            new PersistenceSpecification<Category>(Session)
                .CheckProperty(c => c.Description, "food")
                .VerifyTheMappings();
        }

        [Test]
        public void TestCityMap()
        {
            var country = GetDefaultCountry();
            var polygon = GetDefaultPolygon();

            new PersistenceSpecification<City>(Session)
                .CheckProperty(c => c.Name, "name")
                .CheckProperty(c => c.Region, polygon)
                .CheckReference(c => c.Country, country)
                .VerifyTheMappings();
        }

        [Test]
        public void TestCountryMap()
        {
            var polygon = GetDefaultPolygon();
            var cities = new HashSet<City>
                             {
                                 GetDefaultCity(),
                                 GetDefaultCity(),
                             };

            new PersistenceSpecification<Country>(Session)
                .CheckProperty(c => c.Name, "name")
                .CheckProperty(c => c.Border, polygon)
                .CheckList(c => c.Cities, cities, (co, ci) => co.AddCity(ci))
                .VerifyTheMappings();
        }

        [Test]
        public void TestInventoryItemMap()
        {
            var product = GetDefaultProdcut();
            var branch = GetDefaultBranch();

            new PersistenceSpecification<InventoryItem>(Session)
                .CheckReference(c => c.Product, product)
                .CheckReference(c => c.Branch, branch)
                .CheckProperty(c => c.Amount, 10)
                .VerifyTheMappings();
        }

        [Test]
        public void TestPersonMap()
        {
            var contact = GetDefaultPerson();

            new PersistenceSpecification<Store>(Session)
                .CheckProperty(c => c.Name, "name")
                .CheckReference(c => c.Contact, contact)
                .VerifyTheMappings();
        }

        [Test]
        public void TestProductMap()
        {
            var category = GetDefaultCategory();

            new PersistenceSpecification<Product>(Session)
                .CheckProperty(c => c.Description, "desc")
                .CheckProperty(c => c.Name, "matan")
                .CheckProperty(c => c.UnitPrice, 10.10)
                .CheckReference(c => c.Category, category)
                .VerifyTheMappings();
        }

        [Test]
        public void TestStoreMap()
        {
            var contact = GetDefaultPerson();
            var branches = new HashSet<Branch>
                               {
                                   GetDefaultBranch(),
                                   GetDefaultBranch(),
                               };

            new PersistenceSpecification<Store>(Session)
                .CheckProperty(c => c.Name, "name")
                .CheckReference(c => c.Contact, contact)
                .CheckList(c => c.Branches, branches, (c, b) => c.AddBranch(b))
                .VerifyTheMappings();
        }
    }

    public class KeyEquilityComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            return ((IntEntity)x).Equals(((IntEntity)y));
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}