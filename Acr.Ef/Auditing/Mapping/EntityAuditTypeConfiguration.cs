using System;
using System.Data.Entity.ModelConfiguration;


namespace Acr.Ef.Auditing.Mapping {
    
    public class EntityAuditTypeConfiguration : EntityTypeConfiguration<EntityAudit> {

        public EntityAuditTypeConfiguration() {
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("AuditID");
            this.Property(x => x.Action).IsRequired();

            this.Property(x => x.AppDomain).HasMaxLength(500).IsRequired();
            this.Property(x => x.IdentityType).HasMaxLength(500).IsRequired();
            this.Property(x => x.UserName).HasMaxLength(250).IsRequired();
            this.Property(x => x.DateCreatedUtc).IsRequired();

            this.Property(x => x.EntityID).HasMaxLength(50).IsRequired();
            this.Property(x => x.EntityType).HasMaxLength(500).IsRequired();
            
            this.HasMany(x => x.Properties)
                .WithRequired(x => x.EntityAudit)
                .HasForeignKey(x => x.EntityAuditID);
        }
    }
}
