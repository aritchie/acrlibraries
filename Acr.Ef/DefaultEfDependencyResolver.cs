using System;
using System.Collections.Generic;


namespace Acr.Ef {
    
    public class DefaultEfDependencyResolver : IEfDependencyResolver {
        private readonly IDictionary<Type, IList<object>> services = new Dictionary<Type, IList<object>>(); 
        

        public void Install(Type serviceType, object service) {
            if (services.ContainsKey(serviceType)) 
                services[serviceType].Add(service);
            else 
                services.Add(serviceType, new List<object> { service });
        }

        #region IEfDependencyResolver Members

        public object GetService(Type serviceType) {
            return null;
        }


        public IEnumerable<object> GetServices(Type serviceType) {
            yield return null;
        }

        #endregion
    }
}
