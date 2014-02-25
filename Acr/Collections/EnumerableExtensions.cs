using System;
using System.Collections.Generic;
using System.Linq;


namespace Acr.Collections {
    
    public static class EnumerableExtensions {

        public static int IndexWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
            int index = 0;
            var en = source.GetEnumerator();
            
            while (en.MoveNext()) {
                if (predicate(en.Current)) 
                    return index;
                index++;
            }
            return -1;                
        }


        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
            }
            else {
                dictionary.Add(key, value);
            }
        }


        public static ICollection<T> RemoveWhere<T>(this ICollection<T> list, Func<T, bool> predicate) {
            var removeList = list.Where(predicate).ToList();
            foreach (var item in removeList) {
                list.Remove(item);
            }
            return list;
        }


        public static ICollection<T> EnsureSetAdd<T>(this ICollection<T> set, T item) {
            if (set == null) {
                set = new HashSet<T>();
            }
            set.Add(item);
            return set;
        }


        public static void Each<T>(this IEnumerator<T> en, Action<T> action) {
            if (en != null) {
                while (en.MoveNext()) {
                    action(en.Current);
                }
            }
        }


        public static void Each<T>(this IEnumerable<T> en, Action<T> action) {
            if (en != null) {
                en.GetEnumerator().Each(action);
            }
        }


        public static void Each<T>(this IEnumerator<T> en, Action<T, int> action) {
            if (en != null) {
                int i = 0;
                while (en.MoveNext()) {
                    action(en.Current, i);
                    i++;
                }
            }
        }


        public static void Each<T>(this IEnumerable<T> en, Action<T, int> action) {
            if (en != null) {
                en.GetEnumerator().Each(action);
            }
        }


        public static bool IsEmpty<T>(this IEnumerable<T> en) {
            return (en == null || !en.Any());
        }
    }
}
