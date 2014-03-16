using System;


namespace Acr {
    
    public enum ServiceScope {
        Transient,
        Thread,
        Request,
        Singleton
    }
}
