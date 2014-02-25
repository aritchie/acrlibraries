using System;
using NHibernate.Bytecode;


namespace Acr.Nh {
    
    public class DiObjectsFactory : IObjectsFactory {
        private readonly INhDependencyResolver dependencyResolver;


        public DiObjectsFactory(INhDependencyResolver dependencyResolver) {
            this.dependencyResolver = dependencyResolver;    
        }


        public object CreateInstance(Type type, params object[] ctorArgs) {
            return Activator.CreateInstance(type, ctorArgs);
        }


        public object CreateInstance(Type type, bool nonPublic) {
            return Activator.CreateInstance(type, nonPublic);
        }


        public object CreateInstance(Type type) {
            return this.dependencyResolver.GetService(type) ?? Activator.CreateInstance(type);
        }
    }
}