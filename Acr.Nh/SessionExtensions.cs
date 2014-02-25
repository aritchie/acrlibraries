using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate;


namespace Acr.Nh {

    public static class SessionExtensions {


        public static void SaveDynamic(this ISession session, string entityName, object entity) {
            session.Save(entityName, GetObjToSave(entity));
        }


        private static object GetObjToSave(object entity) {
            object objToSave;
            var t = entity.GetType();

            if (!t.Name.StartsWith("<>")) {
                objToSave = entity;
            }
            else {
                var dynEntity = new Dictionary<string, object>();
                var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties) {
                    object propValue = GetObjToSave(property.GetValue(entity, null));
                    dynEntity[property.Name] = propValue;
                }
                objToSave = dynEntity;
            }
            return objToSave;
        }


        public static T GetRequired<T>(this ISession session, object key, string entityName = null, LockMode lockMode = null) where T : class {
            T obj = null;
            if (entityName == null) {
                obj = session.Get<T>(key, lockMode ?? LockMode.None);
            }
            else {
                obj = session.Get(entityName, key) as T;
                if (lockMode != null && lockMode != LockMode.None) {
                    session.Lock(obj, lockMode);
                }
            }

            if (obj == null) {
                throw new ArgumentException(
                    String.Format(
                        "Key query did not return result for type '{0}', Key: '{1}'", 
                        typeof(T).Name,
                        key
                    )
                );
            }
            if (lockMode != null && !lockMode.Equals(LockMode.None)) {
                session.Lock(entityName, obj, lockMode);
            }
            return obj;
        }


        public static void InTransaction(this ISession session, Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            if (session.Transaction != null && session.Transaction.IsActive)
                throw new ArgumentException("A transaction is already running");

            using (var transaction = session.BeginTransaction(isolationLevel)) {
                action.Invoke();
                session.Flush();
                transaction.Commit();
            }
        }


        //public static bool IsUnique(object entity, params string[] properties) {
//            this.Properties.Each(x => {
//                // TODO: what about Map entity mode
//                var pn = x;
//                var value = this.Metadata.GetPropertyValue(model, pn, EntityMode.Poco);

//                if (value != null && !value.GetType().IsSimpleType()) {
//                    var otherMeta = this.GetMetadata(value);
//                    value = otherMeta.GetIdentifier(value, EntityMode.Poco);
//                    pn = String.Format("{0}.{1}", x, otherMeta.IdentifierPropertyName);
//                }
//                criteria.Eq(pn, value);
//            });            
        //}
        //public static bool IsUnique<T>(T obj, IQueryExpression<>)

        //public static Task UnitOfWorkAsync(this ISession session, Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
        //    if (session.Transaction != null && session.Transaction.IsActive)
        //        throw new ArgumentException("A transaction is already running");

        //    return Task.Factory.StartNew(() => session.InTransaction(() => {
        //        // TODO: bind session back to thread
        //        action();
        //    }));
        //}


        //public static Task FlushAsync(this ISession session) {
        //    return Task.Factory.StartNew(session.Flush);
        //}


        //public static Task<T> GetRequiredAsync<T>(this ISession session, object id) where T : class {
        //    return Task<T>.Factory.StartNew(() => session.GetRequired<T>(id));
        //}
    }
}