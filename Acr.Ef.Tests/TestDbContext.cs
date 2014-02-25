using System;
using System.Data.Entity;
using Acr.Ef.Tests.Models;


namespace Acr.Ef.Tests {
    
    public class TestDbContext : AcrDbContext {

        public DbSet<Family> Families { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<MyFile> MyFiles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new FamilyMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new MyFileMap());
            base.OnModelCreating(modelBuilder);
        }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        //    modelBuilder.HasDefaultSchema("[=SPH=]");

        //    var fb = modelBuilder.Entity<Family>();
        //    fb.HasKey(x => x.ID);
        //    fb.Property(x => x.ID).HasColumnName("FamilyID");

        //    var pb = modelBuilder.Entity<Person>();
        //    pb.HasKey(x => x.ID);


        //    base.OnModelCreating(modelBuilder);
        //}

        public TestDbContext() : base("name=UnitTests") {
        }
    }
}
