using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;


namespace Acr.Ef {
    
    public static class DbContextExtensions {

        public static DbPropertyEntry TryGetProperty(this DbEntityEntry entry, string propertyName) {
            try { 
                return entry.Property(propertyName);
            }
            catch {
                return null;
            }
        }


        public static DataPage<T> DataPage<T>(this IQueryable<T> query, Pager pager) where T : class {
            var skip = GetSkipCount(pager.Start, pager.MaxResults, pager.UsePages);
            pager.Sorts.Each(x => query = query.OrderBy(x));
            
            var count = query.Count();
            var data = query
                .Skip(skip)
                .Take(pager.MaxResults)
                .ToArray();

            return new DataPage<T> {
                TotalCount = count,
                Data = data
            };
        }


        private static int GetSkipCount(int start, int max, bool usePages) {
            if (!usePages)
                return start;

            start--;
            if (start < 0) 
                start = 0;
                
            return start * max;
        }


        public static bool HasChanges(this DbContext context) {
            return context
                .ChangeTracker
                .Entries()
                .Any(x => x.State != EntityState.Unchanged);
        }


        public static bool HasChanges(this DbContext context, object entity) {
            return (context.Entry(entity).State != EntityState.Unchanged);
        }


        public static void RegisterCatalogSchemaInterceptor(this DbContext context, Func<string> schemaAction) {
            DbInterception.Add(new SchemaDbCommandInterceptor(schemaAction));
        }


        public static bool IsPropertyDirty<T, TProperty>(this DbContext context, T obj, Expression<Func<T, TProperty>> expression) where T : class {
            return context.Entry(obj).Property(expression).IsModified;
        }


        public static bool IsPropertyDirty(this DbContext context, object obj, string property) {
            return context.Entry(obj).Property(property).IsModified;    
        }


        public static bool IsLoaded<T, TProperty>(this DbContext context, T obj, Expression<Func<T, TProperty>> expression) 
                where T : class 
                where TProperty : class {
            return context.Entry(obj).Reference(expression).IsLoaded;
        }


        public static bool IsLoaded(this DbContext context, object obj, string property) {
            return context.Entry(obj).Reference(property).IsLoaded;    
        }


        public static TProperty GetPreviousValue<TEntity, TProperty>(this DbContext context, TEntity obj, Expression<Func<TEntity, TProperty>> expression) where TEntity : class {
            return context.Entry(obj).Property(expression).OriginalValue;
        }


        public static object GetPreviousValue(this DbContext context, object obj, string property) {
            return context.Entry(obj).Property(property).OriginalValue;
        }


        public static T GetPreviousValue<T>(this DbContext context, object obj, string property) {
            return (T)context.GetPreviousValue(obj, property);
        }


        public static T Lazy<T>(this T context, bool enable) where T : DbContext {
            context.Configuration.LazyLoadingEnabled = enable;
            context.Configuration.ProxyCreationEnabled = enable;
            return context;
        }
    }
}
