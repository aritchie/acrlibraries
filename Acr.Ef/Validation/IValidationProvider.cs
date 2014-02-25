using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;


namespace Acr.Ef.Validation {
    
    public interface IValidationProvider {

        bool ShouldValidateEntity(DbContext context, DbEntityEntry entityEntry);
        DbEntityValidationResult ValidateEntity(DbContext context, DbEntityEntry entityEntry, IDictionary<object, object> items);
    }
}
