using System;
using NHibernate.Bytecode;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Properties;


namespace Acr.Nh {
    
    public class DiByteCodeProvider : AbstractBytecodeProvider {
        private readonly IObjectsFactory objectsFactory;


        public DiByteCodeProvider(INhDependencyResolver dependencyResolver) {
            this.objectsFactory = new DiObjectsFactory(dependencyResolver);    
        }


        public override IObjectsFactory ObjectsFactory {
            get { return this.objectsFactory; }
        }


        public override IReflectionOptimizer GetReflectionOptimizer(Type clazz, IGetter[] getters, ISetter[] setters) {
            return new ReflectionOptimizer(clazz, getters, setters);
        }
    }
}
