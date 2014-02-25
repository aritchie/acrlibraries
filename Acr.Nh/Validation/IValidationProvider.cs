using System;
using System.Collections.Generic;
using NHibernate;


namespace Acr.Nh.Validation {
    
    public interface IValidationProvider {

        // TODO: send dirty properties
        IEnumerable<ValidateResult> Validate(ISession session, object entity, bool update);
    }
}
