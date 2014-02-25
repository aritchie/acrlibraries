using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;


namespace Acr.Ef.Validation {
    
    public abstract class Validator<T, TDbContext> : IValidator 
            where T : class
            where TDbContext : DbContext {

        protected abstract IEnumerable<DbValidationError> ValidateEntity(TDbContext context, T entity, bool update);


        #region IValidator Members

        public virtual bool CanValidate(object entity) {
            return (entity.GetType() == typeof(T));
        }


        public virtual IEnumerable<DbValidationError> Validate(DbContext context, object entity, bool update) {
            return this.ValidateEntity((TDbContext)context, (T)entity, update);
        }

        #endregion
    }
}
