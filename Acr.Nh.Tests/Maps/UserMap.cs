using System;
using Acr.Nh.Mapping;
using Acr.Nh.Tests.Models;
using NHibernate.Mapping.ByCode;


namespace Acr.Nh.Tests.Maps {
    
    public class UserMap : IModelMap {

        public void Map(ModelMapper mapper) {
            mapper.Class<User>(x => {
                x.Property(y => y.UserName);
                x.Property(y => y.Password);
                x.Property(y => y.FirstName);
                x.Property(y => y.LastName);
                x.Set(
                    y => y.Todos,
                    y => y.Key(z => z.Column("UserID")),
                    y => y.OneToMany()
                );
            });
        }
    }
}
