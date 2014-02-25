using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Acr.Collections;
using NHibernate;
using NHibernate.Linq;


namespace Acr.Nh.Linq {

    public static class LinqExtensions {

        public static IFutureValue<TResult> ToFutureValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IQueryable<TSource>, TResult>> selector) where TResult : struct {
            var provider = (INhQueryProvider)source.Provider;
            var method = ((MethodCallExpression)selector.Body).Method;
            var expression = Expression.Call(null, method, source.Expression);
            return (IFutureValue<TResult>)provider.ExecuteFuture(expression);
        }


        public static DataPage<T> DataPage<T>(this IQueryable<T> query, Pager pager) where T : class {
            var skip = GetSkipCount(pager.Start, pager.MaxResults, pager.UsePages);
            pager.Sorts.Each(x => query = query.OrderBy(x));

            var count = query.ToFutureValue(x => x.Count());
            var list = query
                .Skip(skip)
                .Take(pager.MaxResults)
                .ToFuture();

            return new DataPage<T> {
                TotalCount = count.Value,
                Data = list.ToArray()
            };
        }


        public static DataPage<T> DataPage<T>(this IQueryOver<T> queryOver, Pager pager) where T : class {
            return queryOver.UnderlyingCriteria.DataPage<T>(pager);
        }


        private static int GetSkipCount(int start, int max, bool usePages) {
            if (!usePages)
                return start;

            start--;
            if (start < 0) 
                start = 0;
                
            return start * max;
        }        
    }
}
