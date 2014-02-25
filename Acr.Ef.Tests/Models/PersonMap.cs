using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Acr.Ef.Tests.Models {
    
    public class PersonMap : EntityTypeConfiguration<Person> {

        public PersonMap() {
            this.ToTable("Persons");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("PersonID");
            
            this.Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();
     
            this.Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();
          
            this.Property(x => x.DateUpdated);
            this.Property(x => x.DateCreated);
            this.Property(x => x.Version);

            this.Property(x => x.FamilyID)
                .IsRequired();

            this.HasRequired(x => x.Family)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.FamilyID);
        }
    }
}
