using NHibernate;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate
{
    public static class NHibernateSession
    {
        public static ISession Current { get; private set; }

        public static void InitCurrentSession(ISession session)
        {
            Current = session;
        }
    }
}