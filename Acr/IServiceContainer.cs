using System;
using System.Collections.Generic;
using System.Reflection;


namespace Acr {

    public interface IServiceContainer {

        void Build();

        object Get(Type sericeType, string name = null);
        T Get<T>(string name = null);
        IEnumerable<T> GetAll<T>();
        IEnumerable<object> GetAll(Type serviceType); 
        void Release(object service);

        void RegisterSingleton<TInterface>(TInterface instance);
        void Register<TInterface, TImpl>(ServiceScope scope = ServiceScope.Singleton) where TImpl : TInterface;
        void RegisterAll<TInterface>(string assembly, ServiceScope scope = ServiceScope.Singleton);
        void RegisterAll<TInterface>(Assembly assembly, ServiceScope scope = ServiceScope.Singleton);
    }
}
