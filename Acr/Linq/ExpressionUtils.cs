using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


namespace Acr.Linq {

    public static class ExpressionUtils {

        public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
            var memberExpression = RemoveUnary(expression.Body);
            return (memberExpression == null ? null : memberExpression.Member);
        }


        private static MemberExpression RemoveUnary(Expression toUnwrap) {
            if (toUnwrap is UnaryExpression)
                return ((UnaryExpression)toUnwrap).Operand as MemberExpression;

            return toUnwrap as MemberExpression;
        }


        public static IEnumerable<TValue> DistinctOn<TValue, TKey>(this IEnumerable<TValue> sequence, Func<TValue, TKey> keySelector) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return DistinctOnImpl(sequence, keySelector);
        }


        private static IEnumerable<TValue> DistinctOnImpl<TValue, TKey>(
            IEnumerable<TValue> sequence, Func<TValue, TKey> keySelector) {
            var keys = new HashSet<TKey>();

            foreach (TValue value in sequence) {
                if (keys.Add(keySelector(value))) {
                    yield return value;
                }
            }
        }
    }
}
