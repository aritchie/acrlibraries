using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Autofac;


namespace Acr.Autofac {
    
    public class AutofacServiceContainer : IServiceContainer {
        private IContainer container;
        private ContainerBuilder builder;

        public AutofacServiceContainer() : this(new ContainerBuilder()) {}
        public AutofacServiceContainer(ContainerBuilder builder) {
            this.builder = builder;
        }
        public AutofacServiceContainer(IContainer container) {
            this.container = container;
        }


        private void EnsureCanRegister() {
            if (this.container != null)
                throw new ApplicationException("Container is already built");
        }


        private IContainer C() {
            if (this.container == null)
                throw new ApplicationException("Container has not been built");

            return this.container;
        }

        #region IServiceContainer Members

        public void Build() {
            this.EnsureCanRegister();
            this.container = this.builder.Build();
        }


        public object Get(Type sericeType, string name = null) {
            if (name != null)
                return this.C().ResolveNamed(name, sericeType);

            return this.C().Resolve(sericeType);
        }


        public T Get<T>(string name = null) {
            if (name != null)
                return this.C().ResolveNamed<T>(name);

            return this.C().Resolve<T>();
        }


        public IEnumerable<T> GetAll<T>() {
            return this.C().Resolve<IEnumerable<T>>();
        }


        public IEnumerable<object> GetAll(Type serviceType) {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);

            var c = this.C().Resolve(type) as IEnumerable;
            if (c == null) { 
                var en = c.GetEnumerator();
                while (en.MoveNext())
                    yield return en.Current;
            }
        }


        public void Release(object service) {}


        public void RegisterSingleton<TInterface>(TInterface instance) {
            this.EnsureCanRegister();
            this.builder.Register(_ => instance).SingleInstance();
        }


        public void Register<TInterface, TImpl>(ServiceScope scope = ServiceScope.Singleton) where TImpl : TInterface {
            this.EnsureCanRegister();
            //this.builder.Register<TInterface>().As<TImpl>()
        }


        public void RegisterAll<TInterface>(string assembly, ServiceScope scope = ServiceScope.Singleton) {
            throw new NotImplementedException();
        }


        public void RegisterAll<TInterface>(Assembly assembly, ServiceScope scope = ServiceScope.Singleton) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
