using System;
using System.Collections.Concurrent;
using System.Threading;


namespace Acr {
    
    public class ObjectPool<T> : IDisposable where T : class {

        private readonly ConcurrentQueue<T> instances;
        private readonly Func<T> instanceCreator;
        private int instancesInUse;

        #region Properties
        
        public int MaximumSize { get; private set; }
        public int InUse { get { return this.instancesInUse; } }

        public int Available {
            get {
                return (this.MaximumSize == 0
                    ? -1
                    : this.MaximumSize - this.instancesInUse
                );
            }
        }
        
        #endregion

        #region ctor
        
        public ObjectPool(int maxSize = 10) {
            this.MaximumSize = maxSize;
            this.instances = new ConcurrentQueue<T>();
        } 

        ~ObjectPool() {
            Dispose(false);
        }

        #endregion

        #region Methods

        public T Acquire() {
            T instance = null;
            if (!this.instances.TryDequeue(out instance)) {
                if (this.Available == 0) {
                    // TODO: queue exhausted? wait?
                }
                else { 
                    instance = (this.instanceCreator == null
                        ? Activator.CreateInstance<T>()
                        : this.instanceCreator()
                    );
                }
            }
            Interlocked.Increment(ref this.instancesInUse);
            return instance;
        }


        public void Release(T obj) {
            this.instances.Enqueue(obj);
            Interlocked.Decrement(ref this.instancesInUse);
        }


        public void AcquireUnit(Action<T> action) {            
            T obj = null;
            try {
                obj = this.Acquire();
                action(obj);
            }
            finally {
                if (obj != null) {
                    this.Release(obj);
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing) {
            // clear instances
            if (disposing) {
                Array.ForEach(
                    this.instances.ToArray(), 
                    x => {
                        var d = x as IDisposable;
                        if (d != null) {
                            d.Dispose();
                        }
                    }
                );
            }
        }

        #endregion
    }
}
