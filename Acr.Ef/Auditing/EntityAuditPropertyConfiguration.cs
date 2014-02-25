using System;


namespace Acr.Ef.Auditing {
    
    public class EntityAuditPropertyConfiguration {

        public string Name { get; set; }
        public bool IsAudited { get; set; }
        public bool IncludeOldValue { get; set; }

        public Func<object, string> Serialize { get; set; }
    }
}
