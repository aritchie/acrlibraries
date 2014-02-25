using System;
using NHibernate;
using NHibernate.Event;


namespace Acr.Nh.EventListeners {

    public class AuditEventListener : IPreInsertEventListener,
                                      IPreUpdateEventListener,
                                      IPostInsertEventListener, 
                                      IPostUpdateEventListener, 
                                      IPostDeleteEventListener, 
                                      IPostCollectionUpdateEventListener {

        public bool OnPreUpdate(PreUpdateEvent @event) {
            return false;
        }


        public bool OnPreInsert(PreInsertEvent @event) {
            return false;
        }

        //private string GetPropertValue(object[] state, int index) {
        //    var properyValue = state[index];
        //    if (properyValue == null)
        //        return string.Empty;
        //    else if (properyValue is BaseEntity)
        //        return ((BaseEntity)properyValue).Id.ToString();
        //    else
        //        return properyValue.ToString();
        //}


        public void OnPostUpdate(PostUpdateEvent @event) {
            // TODO: audit ignore
            // TODO: insert into version table
            // TODO: insert change set
            // TODO: audit table naming conventions _AUD
            // TODO: audit context additions (ie. User who did it)
            var dirtyPropertyIndexes = @event.Persister.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session);
            var session = @event.Session.GetSession(EntityMode.Poco);
            var classMap = @event.Session.SessionFactory.GetClassMetadata(@event.Entity.GetType()); // proxies shouldn't be an issue here

            foreach (var index in dirtyPropertyIndexes) {
                //var oldValue = GetPropertyValue(@event.OldState, index);
                //var newValue = GetPropertyValue(@event.State, index);
            }
        }


        public void OnPostInsert(PostInsertEvent @event) {
            throw new NotImplementedException();
        }


        public void OnPostDelete(PostDeleteEvent @event) {
            // TODO: don't store values at time of insert?
        }


        public void OnPostUpdateCollection(PostCollectionUpdateEvent @event) {
            var colMap = @event.Session.SessionFactory.GetCollectionMetadata(@event.Collection.Role);
        }
    }
}

//public void onCollectionUpdate(Object collection, Serializable id) {
//    System.out.println("****onCollectionUpdate");

//    if(collection instanceof PersistentMap) {
//        PersistentMap newValues = (PersistentMap) collection;
//        Object owner = newValues != null ? newValues.getOwner() : null;
//        Set<?> oldValues = newValues != null
//            ? ((Map<?, ?>) newValues.getStoredSnapshot()).keySet()
//            : null;

//        System.out.println("owner: " + (owner != null ? owner.toString() : "(null)"));
//        System.out.println("oldValues: " + (oldValues != null ? oldValues.toString() : "(null)"));
//        System.out.println("newValues: " + (newValues != null ? newValues.toString() : "(null)"));
//    } else if (collection instanceof PersistentSet) {
//        PersistentSet newValues = (PersistentSet) collection;
//        Object owner = newValues != null ? newValues.getOwner() : null;
//        Set<?> oldValues = newValues != null
//            ? ((Map<?, ?>) newValues.getStoredSnapshot()).keySet()
//            : null;

//        System.out.println("owner: " + (owner != null ? owner.toString() : "(null)"));
//        System.out.println("oldValues: " + (oldValues != null ? oldValues.toString() : "(null)"));
//        System.out.println("newValues: " + (newValues != null ? newValues.toString() : "(null)"));
//    }
//}