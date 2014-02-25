using System;
using NUnit.Framework;


namespace Acr.Ef.Tests {
    
    public abstract class AbstractModuleTestFixture {

        protected virtual void SetupModules(TestEfDependencyResolver resolver) {}


        [TestFixtureSetUp]
        public virtual void OnFixtureSetup() {
            var resolver = (TestEfDependencyResolver)EfConfiguration.GetDependencyResolver<TestDbContext>();
            this.SetupModules(resolver);
        }


        [TestFixtureTearDown]
        public virtual void OnFixtureTearDown() {
            var resolver = (TestEfDependencyResolver)EfConfiguration.GetDependencyResolver<TestDbContext>();
            resolver.Modules.Clear();
        }
    }
}
