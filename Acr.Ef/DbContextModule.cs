using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace Acr.Ef {
    
    public class DbContextModule : IDbContextModule {

        public virtual void OnModelCreating(DbModelBuilder modelBuilder) {}
        public virtual void OnPreSaveChanges(DbContext context) {}
        public virtual void OnPreInsert(DbContext context, DbEntityEntry entry) {}
        public virtual void OnPostInsert(DbContext context, DbEntityEntry entry) {}
        public virtual void OnPreUpdate(DbContext context, DbEntityEntry entry) {}
        public virtual void OnPostUpdate(DbContext context, DbEntityEntry entry) {}
        public virtual void OnPreDelete(DbContext context, DbEntityEntry entry) {}
        public virtual void OnPostDelete(DbContext context, DbEntityEntry entry) {}
        public virtual void OnPostSaveChanges(DbContext context) {}
    }
}
