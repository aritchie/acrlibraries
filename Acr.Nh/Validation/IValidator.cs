using System;
using System.Collections.Generic;
using NHibernate;


namespace Acr.Nh.Validation {
    
    public interface IValidator {

        bool CanValidate(object entity);
        IEnumerable<ValidateResult> Validate(ISession session, object entity, bool update);
    }
}
