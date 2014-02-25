using System;
using Acr.Nh.Mapping;
using Acr.Nh.Tests.Models;
using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Acr.Nh.Tests.Maps {

    public class AttachmentMap : IModelMap {
        
        public void Map(ModelMapper mapper) {
            mapper.Class<Attachment>(x => {
                x.Id(y => y.ID, y => {
                    y.Column("AttachmentID");
                    y.Generator(Generators.Native);
                });
                x.Property(y => y.FileName);
                x.Property(y => y.FileSize);
                x.Property(y => y.Data, y => {
                    y.Type(NHibernateUtil.BinaryBlob);
                    y.Lazy(true);
                });
            });
        }
    }
}
