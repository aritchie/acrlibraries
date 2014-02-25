using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Threading {
    
    public class Timer : IDisposable {

        public string TimerId { get; private set; }
        public bool IsActive { get; private set; }
        public bool PreventOverlap { get; private set; }
        public bool IsRunningAction { get; private set; }

        private CancellationTokenSource cancelSource;
        private readonly TimeSpan time;
        private readonly Action action;


        public Timer(TimeSpan time, Action action) {
            this.time = time;
            this.action = action;
            this.PreventOverlap = true;
            this.TimerId = Guid.NewGuid().ToString();
        }


        public void Start() {
            if (this.cancelSource != null)
                return;

            this.cancelSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => {
                while (!this.cancelSource.IsCancellationRequested) { 
                    Task
                        .Delay(this.time, this.cancelSource.Token)
                        .ContinueWith(_ => this.OnElapsed(), this.cancelSource.Token);
                }
            }, this.cancelSource.Token);
        }


        public void Stop() {
            if (this.cancelSource == null)
                return;

            this.cancelSource.Cancel();
            this.cancelSource.Dispose();
            this.cancelSource = null;
        }


        private void OnElapsed() {
            if (this.PreventOverlap && this.IsRunningAction)
                return;

            try { 
                this.action();
            }
            catch (Exception ex) {
                Debug.WriteLine("Error in timer: {0}.  Exception: {1}", this.TimerId, ex);
            }
        }

        #region IDisposable Members

        public void Dispose() {
            this.Stop();
        }

        #endregion
    }
}
