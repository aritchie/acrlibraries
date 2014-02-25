using System;
using Acr.Ef.Tests.Models;
using NUnit.Framework;


namespace Acr.Ef.Tests {

    [TestFixture]
    public class EntityTimestampTests : AbstractModuleTestFixture {

        protected override void SetupModules(TestEfDependencyResolver resolver) {
            resolver.Modules.Add(new EntityTimestampModule());
        }


        [Test]
        public void Insert_NoTimestampProperty() {
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

                Assert.AreNotEqual(DateTimeOffset.MinValue, person.DateCreated, "DateCreated was not set");
                personId = person.ID;
            }
            using (var context = new TestDbContext()) {
                // re-read to make sure it saved
                var person = context.People.Find(personId);
                Assert.AreNotEqual(DateTimeOffset.MinValue, person.DateCreated, "DateCreated was not saved");
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

                Assert.AreNotEqual(DateTimeOffset.MinValue, person.DateCreated, "DateCreated was not set");
                personId = person.ID;
            }
            using (var context = new TestDbContext()) {
                var person = context.People.Find(personId);
                person.LastName = "Blah";
                context.SaveChanges();

                Assert.AreNotEqual(DateTimeOffset.MinValue, person.DateUpdated, "DateCreated was not saved");
            }
        }
    }
}
