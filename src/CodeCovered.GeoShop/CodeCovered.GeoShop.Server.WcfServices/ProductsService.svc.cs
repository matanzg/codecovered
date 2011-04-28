using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Contracts.Interface;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using CodeCovered.GeoShop.Server.BL;
using CodeCovered.GeoShop.Server.Entities;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate;
using NHibernate.Linq;

namespace CodeCovered.GeoShop.Server.WcfServices
{
    public class ProductsService : IProductsService
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly ISessionFactory _sessionFactory;

        public ProductsService()
            : this(
                Global.Container.Resolve<ISessionFactory>(),
                Global.Container.Resolve<IMappingEngine>())
        {

        }

        public ProductsService(ISessionFactory sessionFactory, IMappingEngine mappingEngine)
        {
            _sessionFactory = sessionFactory;
            _mappingEngine = mappingEngine;
        }

        public IEnumerable<BranchDto> QueryBranchesByCenterPoint(SimplePoint center, double buffer)
        {
            return InAUnitOfWork(session =>
            {
                var geoLocator = new GeoLocator(session);
                var geoPoint = _mappingEngine.Map<SimplePoint, Point>(center);
                var results = geoLocator.Locate<Branch>(geoPoint, buffer);
                return results.Select(r => _mappingEngine.Map<Branch, BranchDto>(r)).ToList();
            });
        }

        public ProductDto GetProductDetails(int productId)
        {
            return InAUnitOfWork(session =>
            {
                var product = session.Get<Product>(productId);
                return _mappingEngine.Map<Product, ProductDto>(product);
            });
        }

        public ProductDto SaveOrUpdateProduct(ProductDto productDto)
        {
            return InAUnitOfWork(session =>
            {
                var product = _mappingEngine.Map<ProductDto, Product>(productDto);
                session.SaveOrUpdate(product);
                return _mappingEngine.Map<Product, ProductDto>(product);
            });
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            return InAUnitOfWork(session => session.Query<Category>().Select(category =>
                _mappingEngine.Map<Category, CategoryDto>(category))
                .ToList());
        }

        public void CreateCategory(string description)
        {
            InAUnitOfWork(session => session.Save(new Category { Description = description }));
        }

        private T InAUnitOfWork<T>(Func<ISession, T> func)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    NHibernateSession.InitCurrentSession(session);

                    var result = func(session);
                    tx.Commit();
                    return result;
                }
                catch
                {
                    if (tx != null && tx.IsActive)
                        tx.Rollback();

                    throw;
                }
                finally
                {
                    NHibernateSession.InitCurrentSession(null);
                }
            }
        }

        private void InAUnitOfWork(Action<ISession> action)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    NHibernateSession.InitCurrentSession(session);

                    action(session);
                    tx.Commit();
                }
                catch
                {
                    if (tx != null && tx.IsActive)
                        tx.Rollback();

                    throw;
                }
                finally
                {
                    NHibernateSession.InitCurrentSession(null);
                }
            }
        }
    }
}
