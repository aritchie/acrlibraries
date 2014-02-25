using System;


namespace Acr.Nh.Validation {
    
    public class ValidateResult {

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }


        public ValidateResult(string propertyName = null, string errorMessage = null) {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }
    }
}
