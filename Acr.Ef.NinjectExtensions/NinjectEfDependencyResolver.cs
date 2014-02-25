using System;
using System.Collections.Generic;
using Ninject;


namespace Acr.Ef.NinjectExtensions {
    
    public class NinjectEfDependencyResolver : IEfDependencyResolver {
        private readonly IKernel kernel;


        public NinjectEfDependencyResolver(IKernel kernel) {
            this.kernel = kernel; 
        }

        #region IEfDependencyResolver Members

        public object GetService(Type serviceType) {
            return this.kernel.Get(serviceType);
        }


        public IEnumerable<object> GetServices(Type serviceType) {
            return this.kernel.GetAll(serviceType);
        }

        #endregion
    }
}
