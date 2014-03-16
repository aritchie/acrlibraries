using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject;
using Ninject.Syntax;
using Ninject.Extensions.Conventions;


namespace Acr.Ninject {
    
    public class NinjectServiceContainer : IServiceContainer {
        public IKernel Kernel { get; private set; }


        public NinjectServiceContainer() : this(new StandardKernel()) { }
        public NinjectServiceContainer(IKernel kernel) {
            this.Kernel = kernel;    
        }


        protected virtual void SetBinding<T>(IBindingWhenInNamedWithOrOnSyntax<T> binding, ServiceScope scope) {
            switch (scope) {
                case ServiceScope.Transient:
                    binding.InTransientScope();
                    break;

                case ServiceScope.Thread:
                    binding.InThreadScope();
                    break;

                case ServiceScope.Singleton:
                    binding.InSingletonScope();
                    break;

                case ServiceScope.Request:
                    this.SetRequestScope(binding);
                    break;
            }
        }


        protected virtual void SetRequestScope<T>(IBindingWhenInNamedWithOrOnSyntax<T> binding) {
            throw new NotImplementedException("Request scope has not been setup on the service container");
        }

        #region IServiceContainer Members

        public void Build() { }


        public object Get(Type sericeType, string name = null) {
            return this.Kernel.Get(sericeType, name);
        }


        public T Get<T>(string name = null) {
            return this.Kernel.Get<T>(name);
        }


        public IEnumerable<T> GetAll<T>() {
            return this.Kernel.GetAll<T>();
        }


        public IEnumerable<object> GetAll(Type serviceType) {
            return this.Kernel.GetAll(serviceType);
        }


        public void Release(object service) {
            this.Kernel.Release(service);
        }


        public void RegisterSingleton<TInterface>(TInterface instance) {
            this.Kernel
                .Bind<TInterface>()
                .ToConstant(instance)
                .InSingletonScope();
        }


        public void Register<TInterface, TImpl>(ServiceScope scope = ServiceScope.Singleton) where TImpl : TInterface {
            var binding = this.Kernel
                .Bind<TInterface>()
                .To<TImpl>();

            this.SetBinding(binding, scope);
        }


        public void RegisterAll<TInterface>(string assembly, ServiceScope scope = ServiceScope.Singleton) {
            this.Kernel.Bind(x => x
                .FromAssembliesMatching(assembly)
                .SelectAllClasses()
                .InheritedFrom<TInterface>()
                .BindToSelf()
                .Configure(y => this.SetBinding(y, scope))
            );
        }


        public void RegisterAll<TInterface>(Assembly assembly, ServiceScope scope = ServiceScope.Singleton) {
            this.Kernel.Bind(x => x
                .From(assembly)
                .SelectAllClasses()
                .InheritedFrom<TInterface>()
                .BindToSelf()
                .Configure(y => this.SetBinding(y, scope))
            );
        }

        #endregion
    }
}
