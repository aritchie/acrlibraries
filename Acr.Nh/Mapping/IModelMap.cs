using System;
using NHibernate.Mapping.ByCode;


namespace Acr.Nh.Mapping {

    public interface IModelMap {

        void Map(ModelMapper mapper);
    }
}
