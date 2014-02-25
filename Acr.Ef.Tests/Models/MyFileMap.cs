using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Acr.Ef.Tests.Models {
    
    public class MyFileMap : EntityTypeConfiguration<MyFile> {

        public MyFileMap() {
            this.HasKey(x => x.ID);
            this.Property(x => x.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("MyFileID");

            this.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(x => x.Content)
                .HasMaxLength(null)
                .IsRequired();
        }
    }
}
