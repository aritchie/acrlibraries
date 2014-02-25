using System;
using NHibernate;
using NHibernate.Event;
using NHibernate.Event.Default;


namespace Acr.Nh.EventListeners {

    [Serializable]
    public class FlushFixEventListener : DefaultFlushEventListener {

        public override void OnFlush(FlushEvent @event) {
            try {
                base.OnFlush(@event);
            }
            catch (AssertionFailure) {
                // throw away
            }
        }
    }
}
