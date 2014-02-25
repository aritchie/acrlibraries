using System;
using Acr.Collections;
using NHibernate;
using NHibernate.Event;


namespace Acr.Nh.Validation {
    
    public class ValidationEventListener : IPreInsertEventListener, IPreUpdateEventListener, IPreCollectionUpdateEventListener {
        private readonly IValidationProvider validationProvider;


        public ValidationEventListener(IValidationProvider validationProvider) {
            this.validationProvider = validationProvider;    
        }


        public bool OnPreInsert(PreInsertEvent @event) {
            this.Validate(@event.Session, @event.Entity, true);
            return false;
        }


        public bool OnPreUpdate(PreUpdateEvent @event) {
            this.Validate(@event.Session, @event.Entity, true);
            return false;
        }


        public void OnPreUpdateCollection(PreCollectionUpdateEvent @event) {
            if (@event.AffectedOwnerOrNull == null)
                return;

            this.Validate(@event.Session, @event.AffectedOwnerOrNull, true);
        }


        private void Validate(ISession session, object obj, bool update) {
            var results = this.validationProvider.Validate(session, obj, update);
            if (results.IsEmpty())
                return;

            throw new ValidationFailedException(results);
        }
    }
}
