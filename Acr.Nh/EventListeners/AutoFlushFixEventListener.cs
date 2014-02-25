using System;
using NHibernate;
using NHibernate.Event;
using NHibernate.Event.Default;


namespace Acr.Nh.EventListeners {

    public class AutoFlushFixEventListener : DefaultAutoFlushEventListener {
    
        public override void OnAutoFlush(AutoFlushEvent @event) {
            try {
                base.OnAutoFlush(@event);
            }
            catch (AssertionFailure) {
            }
        }
    }
}
