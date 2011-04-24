using System.Collections.Generic;
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

            new PersistenceSpecification<Branch>(Session)
                .CheckProperty(c => c.Address, address)
                .CheckProperty(c => c.Location, point)
                .CheckReference(c => c.Manager, manager)
                .CheckReference(c => c.Store, store, (b,s) => b.AssignStore(s))
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
        public void TestPersonMap()
        {
            var contact = GetDefaultPerson();

            new PersistenceSpecification<Store>(Session)
                .CheckProperty(c => c.Name, "name")
                .CheckReference(c => c.Contact, contact)
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
}