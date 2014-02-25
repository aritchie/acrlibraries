using System;
using System.Collections.Generic;
using System.Data.Entity;
using Acr.Ef.Auditing;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using NUnit.Framework;


namespace Acr.Ef.Tests {
    
    [SetUpFixture]
    public class Setup {

        [SetUp]
        public void OnStartUp() {
            EntityFrameworkProfiler.Initialize();

            var resolver = new TestEfDependencyResolver {
                Modules = new List<IDbContextModule> {
                    new EntityAuditModule()
                }
            };
            EfConfiguration.RegisterDependencyResolver<TestDbContext>(resolver);
            Database.SetInitializer(new DropCreateDatabaseAlways<TestDbContext>());
            resolver.Modules.Clear();
            
        }
    }
}
