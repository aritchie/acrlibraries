using System;
using System.Collections.Generic;
using System.Linq;


namespace Acr.Mail {
    
    internal static class Extensions {

        public static bool IsEmpty(this string @string) {
            return String.IsNullOrWhiteSpace(@string);
        }


        public static void Each<T>(this IEnumerable<T> en, Action<T> action) {
            if (en == null)
                return;

            var it = en.GetEnumerator();
            while (it.MoveNext()) {
                action(it.Current);
            }
        }


        public static bool IsEmpty<T>(this IEnumerable<T> en) {
            return (en == null || !en.Any());
        }

        public static string[] SplitTrim(this string value, char splitter) {
            return value
                .Split(splitter)
                .Select(x => x.Trim()) 
                .Where(x => !x.IsEmpty())
                .ToArray();
        }
    }
}
