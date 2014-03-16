using System;
using System.Collections.Generic;


namespace Acr.Ef {
    
    public class ServiceContainerEfDependencyInjector : IEfDependencyResolver {
        private readonly IServiceContainer container;

        public ServiceContainerEfDependencyInjector(IServiceContainer container) {
            this.container = container;
        }


        public object GetService(Type serviceType) {
            return container.Get(serviceType);
        }


        public IEnumerable<object> GetServices(Type serviceType) {
            return container.GetAll(serviceType);
        }
    }
}
