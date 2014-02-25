using System;
using System.Collections.Generic;
using Acr.Ef.Validation;


namespace Acr.Ef.Tests {
    
    public class TestEfDependencyResolver : IEfDependencyResolver {

        public List<IDbContextModule> Modules { get; set; }
        public IValidationProvider ValidationProvider { get; set; }

        #region IEfDependencyResolver Members

        public object GetService(Type serviceType) {
            if (serviceType == typeof(IValidationProvider))
                return this.ValidationProvider;

            return null;
        }


        public IEnumerable<object> GetServices(Type serviceType) {
            if (serviceType == typeof(IDbContextModule))
                return this.Modules;

            return null;
        }

        #endregion
    }
}
