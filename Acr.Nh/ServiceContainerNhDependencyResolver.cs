using System;
using System.Collections.Generic;


namespace Acr.Nh {
    
    public class ServiceContainerNhDependencyResolver : INhDependencyResolver {
        private readonly IServiceContainer container;


        public ServiceContainerNhDependencyResolver(IServiceContainer container) {
            this.container = container;    
        }


        public object GetService(Type serviceType) {
            return this.container.Get(serviceType);
        }


        public IEnumerable<object> GetServices(Type serviceType) {
            return this.container.GetAll(serviceType);
        }
    }
}
