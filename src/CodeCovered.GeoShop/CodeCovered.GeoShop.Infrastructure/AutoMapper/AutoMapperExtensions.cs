using System;
using AutoMapper;
using NHibernate;

namespace CodeCovered.GeoShop.Infrastructure.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSrc, TDest> ConstructUsingNHibernate<TSrc, TDest>(
            this IMappingExpression<TSrc, TDest> expression,
            Func<ISession> getSession, Func<TSrc, object> getId)
            where TDest : class, new()
        {
            return expression.ConstructUsing(src =>
                    getSession().Get<TDest>(getId(src)) ?? new TDest());
        }

        public static NHibernateReferenceConfiguration<TSrc, TMember> LoadNHibernateReference<TSrc, TMember>(
            this IMemberConfigurationExpression<TSrc> expression,
            Func<ISession> getSession, Func<TSrc,TMember> getSourceMember)
        {
            return new NHibernateReferenceConfiguration<TSrc, TMember>(expression, getSession, getSourceMember);
        }

        public class NHibernateReferenceConfiguration<TSrc, TMember>
        {
            private readonly IMemberConfigurationExpression<TSrc> _configurationExpression;
            private readonly Func<TSrc, TMember> _getSourceMember;
            private readonly Func<ISession> _getSession;

            internal NHibernateReferenceConfiguration(
                IMemberConfigurationExpression<TSrc> configurationExpression, 
                Func<ISession> getSession, Func<TSrc, TMember> getSourceMember)
            {
                _configurationExpression = configurationExpression;
                _getSession = getSession;
                _getSourceMember = getSourceMember;
            }

            public void As<TResult>() where TResult : class
            {
                _configurationExpression.MapFrom(src =>
                        {
                            var session = _getSession();
                            var id = (object) _getSourceMember(src);
                            return session.Load<TResult>(id);
                        });
            }
        }
    }
}