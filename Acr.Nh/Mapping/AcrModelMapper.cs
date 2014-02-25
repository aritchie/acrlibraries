using System;
using System.Reflection;
using Acr.Nh.Types;
using Acr.Reflection;
using NHibernate.Mapping.ByCode;


namespace Acr.Nh.Mapping {
    
    public class AcrModelMapper : ModelMapper {

        public string HiloTableName { get; set; }
        public string HiloColumnName { get; set; }
        public string HiloEntityColumnName { get; set; }

        #region ctor
        
        public AcrModelMapper() {
            this.HiloTableName = "PrimaryKey";
            this.HiloColumnName = "NextHigh";
            this.HiloEntityColumnName = "entity_name";

            this.BeforeMapClass += this.OnBeforeMapClass;
            this.BeforeMapSet += this.OnBeforeMapSet;
            this.BeforeMapProperty += this.OnBeforeMapProperty;
            this.BeforeMapManyToOne += this.OnBeforeMapManyToOne;
        }

        #endregion

        #region Events

        private void OnBeforeMapClass(IModelInspector mi, Type type, IClassAttributesMapper map) {
            if (mi.IsRootEntity(type)) {
                this.MapTable(type, map);
                this.MapIdentifier(type, map);
            }
        }


        private void OnBeforeMapSet(IModelInspector mi, PropertyPath member, ISetPropertiesMapper map) {
            map.Cascade(Cascade.All | Cascade.DeleteOrphans);
            map.BatchSize(50);
            map.Lazy(CollectionLazy.Lazy);
            map.Inverse(true);
        }


        private void OnBeforeMapProperty(IModelInspector mi, PropertyPath member, IPropertyMapper map) {
            var type = member.LocalMember.GetPropertyOrFieldType();

            if (type == typeof(string)) {
                if (member.LocalMember.Name == "FileName") {
                    map.Length(255);
                }
                else {
                    map.Type<DefaultStringType>();
                    map.Length(50);
                }
            }
            else if (type == typeof(byte[])) {
                map.Length(Int32.MaxValue / 2);
                map.Column(x => x.SqlType("varbinary(max)"));
            }

            if (member.LocalMember.Name == "DateCreated") {
                map.Update(false);
            }
        }


        private void OnBeforeMapManyToOne(IModelInspector mi, PropertyPath member, IManyToOneMapper propertyCustomizer) {
            var pi = member.LocalMember as PropertyInfo;

            if (pi != null && mi.IsRootEntity(pi.PropertyType)) {
                string name = pi.PropertyType.Name + "ID";

                // CreatedBy == MemberID_CreatedBy
                if (pi.PropertyType.Name != pi.Name) {
                    name += "_" + pi.Name;
                }
                propertyCustomizer.Column(k => k.Name(name));
            }
        }

        #endregion

        #region Internals

        private void MapTable(Type type, IClassAttributesMapper map) {
            string table = type.Name;

            if (type.Name.EndsWith("y")) {
                table = type.Name.TrimEnd('y') + "ies";
            }
            else if (!type.Name.EndsWith("s")) {
                table = type.Name + "s";
            }
            map.Table(table);
        }


        private void MapIdentifier(Type type, IClassAttributesMapper map) {
            if (type.HasProperty("ID")) {
                string id = type.Name + "ID";
                var p = type.GetProperty("ID");

                if (p.PropertyType == typeof(Guid)) {
                    map.Id(p, x => {
                        x.Column(id);
                        x.Generator(Generators.Guid);
                    });
                }
                else if (p.PropertyType == typeof(int) || p.PropertyType == typeof(long)) {
                    map.Id(p, x => {
                        x.Column(id);
                        x.Generator(
                            Generators.HighLow,
                            y => y.Params(new {
                                table = this.HiloTableName,
                                column = this.HiloColumnName,
                                max_lo = 100,
                                where = String.Format("{0} = '{1}'", this.HiloEntityColumnName, type.Name.ToLowerInvariant())
                            })
                        );
                    });
                }
                else if (p.PropertyType == typeof(string)) {
                    map.Id(p, x => {
                        x.Column(id);
                        x.Generator(Generators.Assigned);
                    });
                }
            }
        }

        #endregion
    }
}
