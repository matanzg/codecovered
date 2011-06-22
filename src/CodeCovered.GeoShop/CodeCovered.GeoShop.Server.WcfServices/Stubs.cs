using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCovered.GeoShop.Infrastructure.Factories;
using CodeCovered.GeoShop.Server.Entities;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate;

namespace CodeCovered.GeoShop.Server.WcfServices
{
    public class Stubs
    {
        public static void InsertSomeData(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var israel = new Country { Name = "Israel" };
                var telaviv = new City { Name = "TelAviv" };
                israel.AddCity(telaviv);

                session.Save(israel);

                var address = new Address { City = telaviv, Number = 100, PostalCode = 101, Street = "200" };
                var matan = new Person
                {
                    Gender = Gender.Male,
                    Name = "Matan",
                    HomeAddress = address
                };

                session.Save(matan);

                var store = new Store { Contact = matan, Name = "Main Office" };
                var branch1 = new Branch
                {
                    Address = address,
                    Manager = matan,
                    Name = "branch1",
                    Location = (Point)Default.GeometryFactory.CreatePoint(new Coordinate(32, 34))
                };

                var branch2 = new Branch
                {
                    Address = address,
                    Manager = matan,
                    Name = "branch2",
                    Location = (Point)Default.GeometryFactory.CreatePoint(new Coordinate(32.5, 34.5))
                };

                store.AddBranch(branch1);
                store.AddBranch(branch2);

                var milk = new Product { Name = "milk", UnitPrice = 10, Description = "moo" };
                var bread = new Product { Name = "bread", UnitPrice = 8, Description = "poof" };

                session.Save(milk);
                session.Save(bread);

                branch1.AddProductToInventory(milk, 10);
                branch1.AddProductToInventory(bread, 15);
                branch2.AddProductToInventory(milk, 20);

                session.Save(store);
                tx.Commit();
            }
        }
    }
}