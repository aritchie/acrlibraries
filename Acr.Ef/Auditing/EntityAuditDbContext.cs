using System;
using System.Data.Common;
using System.Data.Entity;
using Acr.Ef.Auditing.Mapping;


namespace Acr.Ef.Auditing {
    
    public class EntityAuditDbContext : DbContext {

        internal EntityAuditDbContext(DbConnection connection) : base(connection, false) { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new EntityAuditTypeConfiguration());
            modelBuilder.Configurations.Add(new EntityAuditPropertyTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<EntityAudit> Audits { get; set; }
        public DbSet<EntityAuditProperty> AuditProperties { get; set; }
    }
}
