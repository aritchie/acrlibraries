using System;
using System.Data.Entity;


namespace Acr.Ef.Mapping {
    
    public interface IDbModelMap {

        void Map(DbModelBuilder builder);
    }
}
