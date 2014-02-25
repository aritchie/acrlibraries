using System;
using System.Collections.Generic;
using System.Linq;


namespace Acr.Ef {
    
    internal static class UtilExtensions {

        public static bool IsEmpty(this string @string) {
            return String.IsNullOrWhiteSpace(@string);    
        }


        public static bool IsEmpty<T>(this IEnumerable<T> en) {
            return (en == null || !en.Any());
        }


        public static void Each<T>(this IEnumerable<T> en, Action<T> action) {
            if (en.IsEmpty())
                return;

            foreach (var obj in en) {
                action(obj);
            }
        } 


        public static void TryCastAction<T>(this object obj, Action<T> castAction) where T : class {
            var cast = obj as T;
            if (cast != null)
                castAction(cast);
        }
    }
}
