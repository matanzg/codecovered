using System;
using AutoMapper;
using CodeCovered.GeoShop.Contracts.Dto;
using CodeCovered.GeoShop.Contracts.Interface;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using CodeCovered.GeoShop.Server.Entities;
using GisSharpBlog.NetTopologySuite.Geometries;
using NHibernate;

namespace CodeCovered.GeoShop.Server.WcfServices
{
    public class ProductsService : IProductsService
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly ISessionFactory _sessionFactory;

        public ProductsService() : this(
                Global.Container.Resolve<ISessionFactory>(),
                Global.Container.Resolve<IMappingEngine>())
        {

        }

        public ProductsService(ISessionFactory sessionFactory, IMappingEngine mappingEngine)
        {
            _sessionFactory = sessionFactory;
            _mappingEngine = mappingEngine;
        }

        public SimplePoint GetProductLocation(int productId)
        {
            return InAUnitOfWork(session =>
            {
                var product = session.Get<Product>(productId);
                var productLocation = new Point(1, 1);//product.Branch.Location);
                return _mappingEngine.Map<Point, SimplePoint>(productLocation);
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

        public void UpdateProduct(ProductDto productDto)
        {
            InAUnitOfWork(session =>
            {
                var product = _mappingEngine.Map<ProductDto, Product>(productDto);
                return _mappingEngine.Map<Product, ProductDto>(product);
            });
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
    }
}
