using System;
using System.Collections.Generic;


namespace Acr.Nh {
    
    public interface INhDependencyResolver {

        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}
