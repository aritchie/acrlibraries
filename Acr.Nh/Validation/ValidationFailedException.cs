using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Acr.Nh.Validation {
    
    public class ValidationFailedException : Exception {
        public ICollection<ValidateResult> Failures { get; private set; } 


        public ValidationFailedException(IEnumerable<ValidateResult> failures) {
            this.Failures = new ReadOnlyCollection<ValidateResult>(failures.ToList());    
        }
    }
}
