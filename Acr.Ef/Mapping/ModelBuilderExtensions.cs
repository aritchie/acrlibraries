using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;


namespace Acr.Ef.Mapping {
    
    public static class ModelBuilderExtensions {

        public static void AppyModelMaps(this DbModelBuilder modelBuilder, params Assembly[] assemblies) {
            assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => 
                    x.IsPublic &&
                    x.IsClass &&
                    !x.IsAbstract &&
                    x.GetInterfaces().Any(y => y == typeof(IDbModelMap))
                )
                .Select(Activator.CreateInstance)
                .Cast<IDbModelMap>()
                .ToList()
                .ForEach(x => x.Map(modelBuilder));
        }
    }
}
