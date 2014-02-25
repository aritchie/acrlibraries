using System;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Threading {
    
    public class TaskAsyncResult : IAsyncResult {
        internal Task Task { get; private set; }


        public TaskAsyncResult(Task task, object asyncState) {
            this.Task = task;
            this.AsyncState = asyncState;
        }

        #region IAsyncResult Members

        public WaitHandle AsyncWaitHandle {
            get { return ((IAsyncResult)Task).AsyncWaitHandle; }
        }


        public bool CompletedSynchronously {
            get { return ((IAsyncResult)Task).CompletedSynchronously; }
        }


        public bool IsCompleted {
            get { return this.Task.IsCompleted; }
        }


        public object AsyncState { get; private set; }

        #endregion
    }
}
