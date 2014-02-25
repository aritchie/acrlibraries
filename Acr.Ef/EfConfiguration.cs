using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace Acr.Ef {
    
    public static class EfConfiguration {
        private static readonly IDictionary<Type, IEfDependencyResolver> dependencyResolvers = new Dictionary<Type, IEfDependencyResolver>(); 


        public static void RegisterDependencyResolver<TDbContext>(IEfDependencyResolver dependencyResolver) where TDbContext : DbContext {
            var t = typeof(TDbContext);

            if (dependencyResolvers.ContainsKey(t)) {
                dependencyResolvers[t] = dependencyResolver;
            }
            else {
                dependencyResolvers.Add(t, dependencyResolver);
            }
        }


        public static IEfDependencyResolver GetDependencyResolver<TDbContext>() where TDbContext : DbContext {
            var t = typeof(TDbContext);
            return (dependencyResolvers.ContainsKey(t) ? dependencyResolvers[t] : null);
        }


        public static IEfDependencyResolver GetDependencyResolver(this DbContext context) {
            return (dependencyResolvers.ContainsKey(context.GetType()) ? dependencyResolvers[context.GetType()] : null);
        }
    }
}
