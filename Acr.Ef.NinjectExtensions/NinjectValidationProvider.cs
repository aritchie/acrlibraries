using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Acr.Collections;
using Acr.Ef.Validation;
using Ninject;
using Ninject.Extensions.Conventions;


namespace Acr.Ef.NinjectExtensions {
    
    public class NinjectValidationProvider : IValidationProvider {
        private readonly IKernel kernel;


        public static void Register(IKernel kernel, params string[] validatorAssemblyNames) {
            kernel
                .Bind<IValidationProvider>()
                .ToMethod(_ => new NinjectValidationProvider(kernel))
                .InSingletonScope();

            if (validatorAssemblyNames.IsEmpty())
                return;

            kernel.Bind(x => x
                .FromAssembliesMatching(validatorAssemblyNames)
                .SelectAllClasses()
                .InheritedFrom<IValidator>()
                .BindToSelf()
                .Configure(y => y.InSingletonScope())
            );
        }


        public NinjectValidationProvider(IKernel kernel) {
            this.kernel = kernel;
        }

        #region IValidationProvider Members

        public bool ShouldValidateEntity(DbContext context, DbEntityEntry entityEntry) {
            return this.kernel
                .GetAll<IValidator>()
                .Any(x => x.CanValidate(entityEntry.Entity));
        }


        public DbEntityValidationResult ValidateEntity(DbContext context, DbEntityEntry entityEntry, IDictionary<object, object> items) {
            var validator = this.kernel
                .GetAll<IValidator>()
                .FirstOrDefault(x => x.CanValidate(entityEntry.Entity));

            if (validator == null)
                return null;

            var update = (entityEntry.State == EntityState.Modified);
            var errors = validator.Validate(context, entityEntry.Entity, update);

            return new DbEntityValidationResult(entityEntry, errors);
        }

        #endregion
    }
}
