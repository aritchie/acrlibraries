using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Acr.Reflection {

    public static class AssemblyUtil {

        public static T GetAttribute<T>(this Assembly assembly) where T : Attribute {
            return (T)assembly.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }       


        public static IQueryable<Type> GetCreateableTypesOfType<T>(this Assembly assembly) {           
            var query = assembly
                .DefinedTypes
                .Where(x =>
                    x.IsClass &&
                    x.IsPublic &&
                    !x.IsAbstract
                )
                .Select(x => x.DeclaringType)
                .AsQueryable();

            return (typeof(T).IsInterface
                ? query.Where(x => x.IsImplementationOf<T>())
                : query.Where(x => x.IsSubclassOf(typeof(T)))
            );
        }


        public static IEnumerable<T> CreateInstances<T>(this Assembly assembly) {
            return assembly
                .GetCreateableTypesOfType<T>()
                .Select(Activator.CreateInstance)
                .Cast<T>();
        }
    }
}