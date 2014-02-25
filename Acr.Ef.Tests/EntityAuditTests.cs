using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Acr.Ef.Auditing;
using Acr.Ef.Tests.Models;
using NUnit.Framework;


namespace Acr.Ef.Tests {
    
    [TestFixture]
    public class EntityAuditTests : AbstractModuleTestFixture {
        private int familyId;


        protected override void SetupModules(TestEfDependencyResolver resolver) {
            resolver.Modules.Add(new EntityAuditModule());
            using (var context = new TestDbContext()) {
                var family = context.Families.Add(new Family {
                    Name = "Test",
                    Date = DateTime.Now
                });
                context.SaveChanges();
                this.familyId = family.ID;
            }
        }


        [Test]
        public void InsertAuditTests() {
            using (var context = new TestDbContext()) {
                var fid = this.familyId.ToString();

                var audit = context
                    .Set<EntityAudit>()
                    .FirstOrDefault(x =>
                        x.EntityID == fid &&
                        x.Action == EntityAuditAction.Insert &&
                        x.EntityType.Contains("Family")
                    );

                Assert.NotNull(audit, "No insert audit found");
            }
        }


        [Test]
        public void UpdateTests() {
            using (var context = new TestDbContext()) {
                var family = context.Families.Find(this.familyId);
                family.Name = "Test2";
                family.Date = DateTime.Now.AddDays(1);
                context.SaveChanges();

                var fid = this.familyId.ToString();
                var audit = context
                    .Set<EntityAudit>()
                    .FirstOrDefault(x =>
                        x.EntityID == fid && 
                        x.EntityType.Contains("Family") &&
                        x.Action == EntityAuditAction.Update
                    );

                Assert.NotNull(audit, "No update audit");

                var exists = audit.Properties.Any(x => 
                    x.PropertyName == "Name" && 
                    x.OldValue == "Test" && 
                    x.NewValue == "Test2"
                );
                Assert.True(exists, "Audit property for Name not found");
            }
        }


        [Test]
        public void BinarySave() {
            using (var context = new TestDbContext()) {
                var file = context.MyFiles.Add(new MyFile {
                    Name = "Acr.Ef.Tests.dll",
                    Content = File.ReadAllBytes("Acr.Ef.Tests.dll")
                });
                context.SaveChanges();
            }
            // TODO: assert
        }


        [Test]
        public void ManyToOne() {
            int personId;

            using (var context = new TestDbContext()) {
                var p = context.People.Add(new Person {
                    FirstName = "Allan",
                    LastName = "Ritchie",
                    Family = context.Families.Find(this.familyId)
                });
                context.SaveChanges();
                personId = p.ID;
            }
            using (var context = new TestDbContext()) {
                var p = context.People.Find(personId);
                p.Family = new Family {
                    Name = "New Family",
                    Date = DateTime.UtcNow  
                };
                context.SaveChanges();
            }
            using (var context = new TestDbContext()) {
                var pid = personId.ToString();
                var audit = context
                    .Set<EntityAudit>()
                    .FirstOrDefault(x => 
                        x.EntityID == pid &&
                        x.Action == EntityAuditAction.Update &&
                        x.EntityType.Contains("Person")
                    );

                Assert.NotNull(audit, "Audit not found");
                var exists = audit.Properties.Any(
                    x => x.PropertyName == "Family" || 
                    x.PropertyName == "FamilyId"
                );
                Assert.True(exists, "Family audit property not found");
            }
        }


        [Test]
        public void OneToManyInsert() {
            using (var context = new TestDbContext()) {
                context.Families.Add(new Family {
                    Name = "one-to-many test",
                    Date = DateTime.UtcNow,
                    Members = new Collection<Person>() {
                        new Person { FirstName = "OneTo", LastName = "Many" }
                    }
                });
                context.SaveChanges(); 
            }
            // TODO: verify two separate insert audits
        }


        [Test]
        [Ignore("Not done")]
        public void OneToManyRemove() {
            
        }


        [Test]
        [Ignore("Not done")]
        public void Nulls() {
            
        }


        [Test]
        [Ignore("Not done")]
        public void ManyToManyAdd() {
            
        }


        [Test]
        [Ignore("Not done")]
        public void ManyToManyRemove() {
            
        }


        [Test]
        public void Delete() {
            string strFamilyId;
            int intFamilyId;

            using (var context = new TestDbContext()) {
                var family = context.Families.Add(new Family {
                    Name = "Delete Me!",
                    Date = DateTime.UtcNow
                });
                context.Families.Add(family);
                context.SaveChanges();
                intFamilyId = family.ID;
                strFamilyId = intFamilyId.ToString();
            }

            using (var context = new TestDbContext()) {
                var family = context.Families.Find(intFamilyId);
                context.Families.Remove(family);
                context.SaveChanges();

                var audit = context
                    .Set<EntityAudit>()
                    .FirstOrDefault(x =>
                        x.EntityID == strFamilyId &&
                        x.Action == EntityAuditAction.Delete
                    );
                Assert.NotNull(audit, "Delete audit was not found");
            }
        }
    }
}
