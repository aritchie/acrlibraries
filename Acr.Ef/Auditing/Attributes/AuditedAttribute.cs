using System;


namespace Acr.Ef.Auditing.Attributes {
    
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AuditedAttribute : Attribute {

        public bool IncludeOldValue { get; set; }
        // TODO: serialize action - how to?
    }
}
