using System;
using System.Collections.Generic;
using System.Reflection;


namespace Acr {
    
    public static class Ioc {
        private static IServiceContainer container;

        public static IServiceContainer Container {
            get {
                if (container == null)
                    throw new Exception("Serivce container is not set");

                return container;
            }
            set {
                // register the service container on the actual container
                container.RegisterSingleton(value);
                container = value;
            }
        }


        public static object Get(Type sericeType, string name = null) {
            return container.Get(sericeType, name);    
        }


        public static T Get<T>(string name = null) {
            return container.Get<T>(name);
        }


        public static IEnumerable<T> GetAll<T>() {
            return container.GetAll<T>();
        }


        public static IEnumerable<object> GetAll(Type serviceType) {
            return container.GetAll(serviceType);
        }


        public static void Release(object obj) {
            container.Release(obj);
        }


        public static void RegisterSingleton<TInterface>(TInterface instance) {
            container.RegisterSingleton(instance);
        }


        public static void Register<TInterface, TImpl>(ServiceScope scope = ServiceScope.Singleton) where TImpl : TInterface {
            container.Register<TInterface, TImpl>(scope);
        }


        public static void RegisterAll<TInterface>(string assembly, ServiceScope scope = ServiceScope.Singleton) {
            container.RegisterAll<TInterface>(assembly, scope);
        }


        public static void RegisterAll<TInterface>(Assembly assembly, ServiceScope scope = ServiceScope.Singleton) {
            container.RegisterAll<TInterface>(assembly, scope);
        }
    }
}
