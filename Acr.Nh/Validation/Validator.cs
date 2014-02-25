using System;
using System.Collections.Generic;
using NHibernate;


namespace Acr.Nh.Validation {
    
    public abstract class Validator<T> : IValidator where T : class {

        protected abstract IEnumerable<ValidateResult> ValidateEntity(ISession session, object entity, bool update);
 

        public virtual bool CanValidate(object entity) {
            return (entity.GetType() == typeof(T));
        }


        public virtual IEnumerable<ValidateResult> Validate(ISession session, object entity, bool update) {
            return this.ValidateEntity(session, (T)entity, update);    
        }
    }
}
