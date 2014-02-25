//using System;
//using NHibernate.Mapping.ByCode;


//namespace Acr.AppFramework.Mapping {
    
//    public static class MappingExtensions {

//        public static void Formula<T>(this IClassMapper<T> classMapper, Func<T, U> property, string formula) where T : class {
//            classMapper.Property(property, x => {
//                x.Formula(formula);
//                x.Lazy(true);
//            });
//        }  
//    }
//}
