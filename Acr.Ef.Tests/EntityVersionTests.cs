using System;
using Acr.Ef.Tests.Models;
using NUnit.Framework;


namespace Acr.Ef.Tests {
    
    [TestFixture]
    public class EntityVersionTests : AbstractModuleTestFixture {

        protected override void SetupModules(TestEfDependencyResolver resolver) {
            resolver.Modules.Add(new EntityVersionModule());
        }



        [Test]
        public void Insert_NoVersion() {
            using (var context = new TestDbContext()) {
                context.Families.Add(new Family {
                    Name = "Insert_NoTimestampProperty",
                    Date = DateTime.Now
                });
                context.SaveChanges();
            }
        }


        [Test]
        public void Insert() {
            var personId = 0;
            using (var context = new TestDbContext()) {
                var person = context.People.Add(new Person {
                    FirstName = "Allan",
                    LastName = "Ritchie",
                    Family = new Family {
                        Name = "Family1",
                        Date = DateTime.Now
                    }
                });
                context.SaveChanges();

                Assert.AreEqual(1, person.Version, "Version was not set");
                personId = person.ID;
            }
            using (var context = new TestDbContext()) {
                // re-read to make sure it saved
                var person = context.People.Find(personId);
                Assert.AreEqual(1, person.Version, "Version was not saved");
            }
        }


        [Test]
        public void Update() {
            var personId = 0;

            using (var context = new TestDbContext()) {
                var person = context.People.Add(new Person {
                    FirstName = "Allan",
                    LastName = "Ritchie",
                    Family = new Family {
                        Name = "Family1",
                        Date = DateTime.Now
                    }
                });
                context.SaveChanges();

                Assert.AreNotEqual(0, person.Version, "Version was not set");
                personId = person.ID;
            }
            using (var context = new TestDbContext()) {
                var person = context.People.Find(personId);
                person.LastName = "Blah";
                context.SaveChanges();

                Assert.AreEqual(2, person.Version, "Version was not updated");
            }          
        }
    }
}
