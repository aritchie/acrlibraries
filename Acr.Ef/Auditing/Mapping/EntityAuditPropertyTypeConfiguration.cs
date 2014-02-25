using System;
using System.Data.Entity.ModelConfiguration;


namespace Acr.Ef.Auditing.Mapping {
    
    public class EntityAuditPropertyTypeConfiguration : EntityTypeConfiguration<EntityAuditProperty> {

        public EntityAuditPropertyTypeConfiguration() {
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("AuditID");
            this.Property(x => x.PropertyName).HasMaxLength(255).IsRequired();
            this.Property(x => x.OldValue).HasMaxLength(null).IsRequired();
            this.Property(x => x.NewValue).HasMaxLength(null).IsRequired();
            this.Property(x => x.EntityAuditID).IsRequired();

            this.HasRequired(x => x.EntityAudit)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.EntityAuditID);
        }
    }
}
