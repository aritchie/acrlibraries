using System;
using System.Collections.Generic;


namespace Acr.Ef {
    
    public interface IEfDependencyResolver {

        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}
