using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace Acr.Ef {
    
    public interface IDbContextModule {

        void OnModelCreating(DbModelBuilder modelBuilder);
        void OnPreSaveChanges(DbContext context);
        void OnPreInsert(DbContext context, DbEntityEntry entry);
        void OnPostInsert(DbContext context, DbEntityEntry entry);
        void OnPreUpdate(DbContext context, DbEntityEntry entry);
        void OnPostUpdate(DbContext context, DbEntityEntry entry);
        void OnPreDelete(DbContext context, DbEntityEntry entry);
        void OnPostDelete(DbContext context, DbEntityEntry entry);
        void OnPostSaveChanges(DbContext context);
    }
}
