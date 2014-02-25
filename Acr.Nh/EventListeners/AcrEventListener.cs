using System;
using System.Linq;
using Acr.Collections;
using NHibernate.Event;


namespace Acr.Nh.EventListeners {
    
    public class AcrEventListener :
            IPreLoadEventListener,
            IPostLoadEventListener,
            IFlushEntityEventListener,
            IPreInsertEventListener, 
            IPostInsertEventListener, 
            IPreUpdateEventListener, 
            IPostUpdateEventListener,
            IPreDeleteEventListener,
            IPostDeleteEventListener {

        private readonly INhDependencyResolver dependencyResolver;


        public AcrEventListener(INhDependencyResolver dependencyResolver) {
            this.dependencyResolver = dependencyResolver;
        }

        #region Events

        public void OnPreLoad(PreLoadEvent @event) {
            this.Process<IPreLoadEventListener>(x => x.OnPreLoad(@event));
        }


        public void OnPostLoad(PostLoadEvent @event) {
            this.Process<IPostLoadEventListener>(x => x.OnPostLoad(@event));
        }


        public void OnFlushEntity(FlushEntityEvent @event) {
            this.Process<IFlushEntityEventListener>(x => x.OnFlushEntity(@event));
        }


        public bool OnPreInsert(PreInsertEvent @event) {
            this.Process<IPreInsertEventListener>(x => x.OnPreInsert(@event));
            return false;
        }


        public void OnPostInsert(PostInsertEvent @event) {
            this.Process<IPostInsertEventListener>(x => x.OnPostInsert(@event));
        }


        public bool OnPreUpdate(PreUpdateEvent @event) {
            this.Process<IPreUpdateEventListener>(x => x.OnPreUpdate(@event));
            return false;
        }


        public void OnPostUpdate(PostUpdateEvent @event) {
            this.Process<IPostUpdateEventListener>(x => x.OnPostUpdate(@event));
        }


        public bool OnPreDelete(PreDeleteEvent @event) {
            this.Process<IPreDeleteEventListener>(x => x.OnPreDelete(@event));
            return false;
        }


        public void OnPostDelete(PostDeleteEvent @event) {
            this.Process<IPostDeleteEventListener>(x => x.OnPostDelete(@event));
        }

        #endregion

        #region Internals

        private void Process<T>(Action<T> action) {
            this.dependencyResolver
                .GetServices(typeof(T))
                .Cast<T>()
                .Each(action);
        }

        #endregion
    }
}
