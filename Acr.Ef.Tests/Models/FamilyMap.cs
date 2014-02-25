using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Acr.Ef.Tests.Models {
    
    public class FamilyMap : EntityTypeConfiguration<Family> {

        public FamilyMap() {
            this.ToTable("Families");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("FamilyID");
            
            this.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
     
            this.HasMany(x => x.Members)
                .WithRequired(x => x.Family)
                .HasForeignKey(x => x.FamilyID);
        }
    }
}
