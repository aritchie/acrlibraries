using System;
using Acr.Nh.Mapping;
using Acr.Nh.Tests.Models;
using NHibernate.Mapping.ByCode;


namespace Acr.Nh.Tests.Maps {
    
    public class TodoMap : IModelMap {

        public void Map(ModelMapper mapper) {
            mapper.Class<Todo>(x => {
                x.Property(y => y.Details, y => y.Length(500));
                x.Property(y => y.DateCreated);
                x.Property(y => y.DateCompleted);
                x.ManyToOne(y => y.User);
            });
        }
    }
}
