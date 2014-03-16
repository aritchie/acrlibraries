using System;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Event;
using NHibernate.Persister.Entity;


namespace Acr.Nh.EventListeners {
    
    public static class EventListenerExtensions {


        public static void UpdateState(this PreInsertEvent @event, string propertyName, object propertyValue) {
            int index = @event.Persister.GetPropertyIndex(@event.Entity, propertyName);
            if (index >= 0) {
                @event.Persister.SetPropertyValue(@event.Entity, index, propertyValue, EntityMode.Poco);
                @event.State[index] = propertyValue;
            }
        }


        public static void UpdateState(this PreUpdateEvent @event, string propertyName, object propertyValue) {
            var index = @event.Persister.GetPropertyIndex(@event.Entity, propertyName);
            if (index >= 0) {
                @event.Persister.SetPropertyValue(@event.Entity, index, propertyValue, EntityMode.Poco);
                @event.State[index] = propertyValue;
            }
        }


        /// <summary>
        /// Can only be used by PreUpdate & PreInsert
        /// </summary>
        /// <param name="event"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void UpdateState(this AbstractPreDatabaseOperationEvent @event, string propertyName, object propertyValue) {
            var update = @event as PreUpdateEvent;
           
            if (update != null) {
                update.UpdateState(propertyName, propertyValue);
            }
            else {
                var insert = @event as PreInsertEvent;
                Verify.IsNotNull(insert, "This extenion method can only be used in PreUpdate & PreInsert use-cases");

                insert.UpdateState(propertyName, propertyValue);
            }
        }


        //public static void UpdateState<T, TProperty>(this AbstractPreDatabaseOperationEvent @event, Expression<Func<T, TProperty>> property, TProperty value) {
        //    @event.UpdateState(property.GetMember().Name, value);
        //}


        //public static void UpdateState<T, TProperty>(this PreInsertEvent @event, Expression<Func<T, TProperty>> property, TProperty value) {
        //    @event.UpdateState(property.GetMember().Name, value);
        //}


        //public static void UpdateState<T, TProperty>(this PreUpdateEvent @event, Expression<Func<T, TProperty>> property, TProperty value) {
        //    @event.UpdateState(property.GetMember().Name, value);
        //}


        //public static bool IsDirtyProperty<T, TProperty>(this PreUpdateEvent @event, Expression<Func<T, TProperty>> property, TProperty value) {
        //    return @event.IsDirtyProperty(property.GetMember().Name);
        //}


        public static bool IsDirtyProperty(this PreUpdateEvent @event, string propertyName) {
            var index = Array.IndexOf(@event.Persister.PropertyNames, propertyName, 0);
            return (@event.OldState[index] != @event.State[index]);
        }


        public static void SetPropertyValue(this IEntityPersister persister, object obj, string propertyName, object value) {
            var index = persister.GetPropertyIndex(obj, propertyName);
            if (index > -1) 
                persister.SetPropertyValue(obj, index, value, EntityMode.Poco);
        }


        public static int GetPropertyIndex(this IEntityPersister persister, object obj, string propertyName) {
            for (var i = 0; i < persister.PropertyNames.Length; i++)
                if (persister.PropertyNames[i] == propertyName)
                    return i;
             
            return -1;
        }

    }
}
