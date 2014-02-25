using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;


namespace Acr.Ef.Validation {
    
    public interface IValidator {

        bool CanValidate(object entity);
        IEnumerable<DbValidationError> Validate(DbContext context, object entity, bool update);
    }
}
