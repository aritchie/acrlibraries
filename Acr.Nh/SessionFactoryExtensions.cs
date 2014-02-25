using System;
using System.Data;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;


namespace Acr.Nh {
    
    public static class SessionFactoryExtensions {

        public static void UnitOfWork(this ISessionFactory sessionFactory, Action<ISession> unit, IsolationLevel level = IsolationLevel.ReadCommitted) {
            using (var s = sessionFactory.OpenSession()) {
                s.InTransaction(() => unit(s), level);
            }
        }


        public static Task UnitOfWorkAsync(this ISessionFactory sessionFactory, Action<ISession> unit, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            return Task.Factory.StartNew(() => sessionFactory.UnitOfWork(unit, isolationLevel));
        }


        public static Task<ISessionFactory> BuildSessionFactoryAsync(this Configuration config) {
            return Task.Factory.StartNew(() => config.BuildSessionFactory());
        }
    }
}
