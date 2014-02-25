using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Acr.Ef.Validation;


namespace Acr.Ef {
    
    public class AcrDbContext : DbContext {

        private List<IDbContextModule> modules;
        private IValidationProvider validator;

        #region ctor

        public AcrDbContext() : base() {
            this.Init();
        }


        public AcrDbContext(DbCompiledModel model) : base(model) {
            this.Init();
        }
        
        
        public AcrDbContext(string connectionStringOrName) : base(connectionStringOrName) {
            this.Init();
        }


        public AcrDbContext(string connectionStringOrName, DbCompiledModel model) : base(connectionStringOrName, model) {
            this.Init();
        }


        private void Init() {
            var resolver = this.GetDependencyResolver();
            if (resolver == null)
                return;

            this.validator = resolver.GetService(typeof(IValidationProvider)) as IValidationProvider;

            this.modules = resolver
                .GetServices(typeof(IDbContextModule))
                .Cast<IDbContextModule>()
                .ToList();
        }

        #endregion

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            this.modules.Each(x => x.OnModelCreating(modelBuilder));
            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges() {
            this.modules.Each(x => x.OnPreSaveChanges(this));
            var entries = this.ChangeTracker.Entries().ToList();
            this.OnSavingChanges(entries);
            var result = base.SaveChanges();
            this.OnSavedChanges(entries);
            this.modules.Each(x => x.OnPostSaveChanges(this));
            return result;
        }


        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry) {
            if (!this.Configuration.ValidateOnSaveEnabled)
                return false;

            return (this.validator == null 
                ? base.ShouldValidateEntity(entityEntry) 
                : this.validator.ShouldValidateEntity(this, entityEntry)
            );
        }


        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items) {
            return (this.validator == null
                ? base.ValidateEntity(entityEntry, items)
                : this.validator.ValidateEntity(this, entityEntry, items)
            );
        }

        #endregion

        #region Virtuals

        private List<DbEntityEntry> inserts = new List<DbEntityEntry>();
        private List<DbEntityEntry> updates = new List<DbEntityEntry>();
        private List<DbEntityEntry> deletes = new List<DbEntityEntry>(); 


        protected virtual void OnSavingChanges(IEnumerable<DbEntityEntry> entries) {
            entries.Each(entry => {
                switch (entry.State) {
                    case EntityState.Added:
                        this.inserts.Add(entry);
                        this.modules.ForEach(x => x.OnPreInsert(this, entry));
                        break;

                    case EntityState.Deleted:
                        this.deletes.Add(entry);
                        this.modules.Each(x => x.OnPreDelete(this, entry));
                        break;

                    case EntityState.Modified:
                        this.updates.Add(entry);
                        this.modules.Each(x => x.OnPreUpdate(this, entry));
                        break;
                }
            });
        }


        protected virtual void OnSavedChanges(IEnumerable<DbEntityEntry> entries) {
            if (!this.modules.Any())
                return;

            this.modules.ForEach(x => {
                this.inserts.ForEach(y => x.OnPostInsert(this, y));
                this.updates.ForEach(y => x.OnPostUpdate(this, y));
                this.deletes.ForEach(y => x.OnPostDelete(this, y));
            });         
        }

        #endregion
    }
}
