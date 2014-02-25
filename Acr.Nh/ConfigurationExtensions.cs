using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Acr.Collections;
using Acr.Nh.EventListeners;
using Acr.Nh.Mapping;
using Acr.Reflection;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;
using Env = NHibernate.Cfg.Environment;
using ModelMapper = NHibernate.Mapping.ByCode.ModelMapper;


namespace Acr.Nh {

    public enum NhDatabaseDriver {
        MsSql2005,
        MsSql2008,
        MsSql2012,
        Oracle10,
        Sqlite,
        MySql,
        PostgreSql
    }


    public static class ConfigurationExtensions {

        public static Task<ISessionFactory> BuildSessionFactoryAsync(this Configuration config) {
            return Task<ISessionFactory>.Factory.StartNew(config.BuildSessionFactory); 
        }


        public static Configuration RegisterDependencyResolver(this Configuration cfg, INhDependencyResolver dependencyResolver) {
            Env.BytecodeProvider = new DiByteCodeProvider(dependencyResolver);
            cfg.AddEventListener(new AcrEventListener(dependencyResolver));
            return cfg;
        }

        /// <summary>
        /// This will set the default schema on the session factory as well as set the global interceptor
        /// </summary>
        /// <returns></returns>
        public static Configuration RegisterDbSchemaInterceptor(this Configuration config, Action<CatalogSchemaLocator> dbLocatorVisitor, bool interceptDatabase, bool interceptSchema) {
            var interceptor = new CatalogSchemaInterceptor(dbLocatorVisitor, interceptDatabase, interceptSchema);
            config.Interceptor = interceptor;
            config.Mappings(x => {
                if (interceptDatabase)
                    x.DefaultCatalog = interceptor.CatalogPlaceHolder;

                if (interceptSchema)
                    x.DefaultSchema = interceptor.SchemaPlaceHolder;
            });
            return config;
        }


        public static Configuration AddAssemblyByCodeMap<T>(this Configuration config, Assembly assembly) where T : ModelMapper, new() {
            return config.AddAssemblyByCodeMap(new T(), assembly);
        }


        public static Configuration AddAssemblyByCodeMap(this Configuration config, Assembly assembly) {
            return config.AddAssemblyByCodeMap(new ModelMapper(), assembly);
        }


        public static Configuration AddAssemblyByCodeMap(this Configuration config, ModelMapper mapper, Assembly assembly) {
            assembly
                .CreateInstances<IModelMap>()
                .Each(x => x.Map(mapper));

            config.AddDeserializedMapping(mapper.CompileMappingForAllExplicitlyAddedEntities(), "CodeBy.hbm.xml");

            return config;
        }


        /// <summary>
        /// Should really only be used for unit tests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCodeMap"></typeparam>
        /// <param name="config"></param>
        /// <param name="codeMap"></param>
        /// <returns></returns>
        public static Configuration AddCodeMap<T, TCodeMap>(this Configuration config, TCodeMap codeMap) 
                where T : ModelMapper, new()
                where TCodeMap : IModelMap, new() {

            var mapper = new T();
            var code = new TCodeMap();
            code.Map(mapper);
            config.AddDeserializedMapping(mapper.CompileMappingForAllExplicitlyAddedEntities(), "Code.hbm.xml");
            return config;
        }


        public static void CreateSchema(this Configuration config) {
            var schema = new SchemaExport(config);
            schema.Create(false, true);
        }


        public static IEnumerable<Exception> UpdateSchema(this Configuration config) {
            var schema = new SchemaUpdate(config);
            schema.Execute(false, true);
            return schema.Exceptions;
        }


        public static Configuration PreventDatabaseConnectionDuringFactoryBuild(this Configuration config) {
            return config.SetProperty(Env.Hbm2ddlKeyWords, "none");
        }


        public static void FullDriver(this IDbIntegrationConfigurationProperties config, string databaseType) {
            config.FullDriver((NhDatabaseDriver)Enum.Parse(typeof(NhDatabaseDriver), databaseType, true));
        }


        public static void FullDriver(this IDbIntegrationConfigurationProperties config, NhDatabaseDriver databaseType) {
            switch (databaseType) {
                case NhDatabaseDriver.MsSql2005:
                    config.Dialect<MsSql2005Dialect>();
                    config.Driver<SqlClientDriver>();
                    config.Batcher<SqlClientBatchingBatcherFactory>();
                    break;

                case NhDatabaseDriver.MsSql2008:
                    config.Dialect<MsSql2008Dialect>();
                    config.Driver<SqlClientDriver>();
                    config.Batcher<SqlClientBatchingBatcherFactory>();
                    break;

                case NhDatabaseDriver.MsSql2012:
                    config.Dialect<MsSql2012Dialect>();
                    config.Driver<SqlClientDriver>();
                    config.Batcher<SqlClientBatchingBatcherFactory>();
                    break;

                case NhDatabaseDriver.Oracle10:
                    config.Dialect<Oracle10gDialect>();
                    config.Driver<OracleClientDriver>();
                    config.Batcher<OracleDataClientBatchingBatcherFactory>();
                    break;

                case NhDatabaseDriver.MySql:
                    config.Dialect<MySQL5Dialect>();
                    config.Driver<MySqlDataDriver>();
                    break;

                case NhDatabaseDriver.PostgreSql:
                    config.Dialect<PostgreSQL82Dialect>();
                    config.Driver<NpgsqlDriver>();
                    break;

                case NhDatabaseDriver.Sqlite:
                    config.Dialect<SQLiteDialect>();
                    config.Driver<SQLite20Driver>();
                    break;
                    
                default :
                    throw new ArgumentException(databaseType + " is not a valid auto-configuration database");
            }
        }

        
        public static Configuration FixFlush(this Configuration config) {
            config.EventListeners.AutoFlushEventListeners = new [] {
                new AutoFlushFixEventListener()
            };
            config.EventListeners.FlushEventListeners = new [] {
                new FlushFixEventListener()
            };
            return config;
        }


        public static Configuration AddEventListener(this Configuration config, ListenerType listenerType, string typeName) {
            var type = Type.GetType(typeName);
            var o = Activator.CreateInstance(type);
            return config.AddEventListener(listenerType, o);
        }


        public static Configuration AddEventListener(this Configuration config, string typeName) {
            var type = Type.GetType(typeName);
            var o = Activator.CreateInstance(type);
            return config.AddEventListener(o);
        }


        public static Configuration AddEventListener(this Configuration config, object o) {
            var type = o.GetType();

            if (type.IsImplementationOf<IAutoFlushEventListener>()) {
                config.AddEventListener(ListenerType.Autoflush, o);
            }

            if (type.IsImplementationOf<IDeleteEventListener>()) {
                config.AddEventListener(ListenerType.Delete, o);
            }

            if (type.IsImplementationOf<IDirtyCheckEventListener>()) {
                config.AddEventListener(ListenerType.DirtyCheck, o);
            }

            if (type.IsImplementationOf<IEvictEventListener>()) {
                config.AddEventListener(ListenerType.Evict, o);
            }

            if (type.IsImplementationOf<IFlushEntityEventListener>()) {
                config.AddEventListener(ListenerType.FlushEntity, o);
            }

            if (type.IsImplementationOf<ILoadEventListener>()) {
                config.AddEventListener(ListenerType.Load, o);
            }

            if (type.IsImplementationOf<ILockEventListener>()) {
                config.AddEventListener(ListenerType.Lock, o);
            }

            if (type.IsImplementationOf<ISaveOrUpdateEventListener>()) {
                config.AddEventListener(ListenerType.SaveUpdate, o);
            }

            if (type.IsImplementationOf<IPreUpdateEventListener>()) {
                config.AddEventListener(ListenerType.PreUpdate, o);
            }

            if (type.IsImplementationOf<IPreLoadEventListener>()) {
                config.AddEventListener(ListenerType.PreLoad, o);
            }

            if (type.IsImplementationOf<IPreDeleteEventListener>()) {
                config.AddEventListener(ListenerType.PreDelete, o);
            }

            if (type.IsImplementationOf<IPreInsertEventListener>()) {
                config.AddEventListener(ListenerType.PreInsert, o);
            }

            if (type.IsImplementationOf<IPostLoadEventListener>()) {
                config.AddEventListener(ListenerType.PostLoad, o);
            }
                
            if (type.IsImplementationOf<IPostInsertEventListener>()) {
                config.AddEventListener(ListenerType.PostInsert, o);
            }

            if (type.IsImplementationOf<IPostUpdateEventListener>()) {
                config.AddEventListener(ListenerType.PostUpdate, o);
            }

            if (type.IsImplementationOf<IPostDeleteEventListener>()) {
                config.AddEventListener(ListenerType.PostDelete, o);
            }
            return config;
        }


        public static Configuration AddEventListener(this Configuration config, ListenerType listenerType, object o) {
            //config.AppendListeners(listenerType, new [] { o });
            switch (listenerType) {
                case ListenerType.Autoflush              : config.AppendListeners(ListenerType.Autoflush, new [] { (IAutoFlushEventListener)o });  break;
                case ListenerType.Merge                  : config.AppendListeners(ListenerType.Merge, new [] { (IMergeEventListener)o }); break;
                case ListenerType.Create                 : config.AppendListeners(ListenerType.Create, new [] { (IPersistEventListener)o }); break;
                case ListenerType.CreateOnFlush          : config.AppendListeners(ListenerType.CreateOnFlush, new [] { (IPersistEventListener)o }); break;
                case ListenerType.Delete                 : config.AppendListeners(ListenerType.Delete, new [] { (IDeleteEventListener)o }); break; 
                case ListenerType.DirtyCheck             : config.AppendListeners(ListenerType.DirtyCheck, new [] { (IDirtyCheckEventListener)o }); break; 
                case ListenerType.Evict                  : config.AppendListeners(ListenerType.Evict, new [] { (IEvictEventListener)o }); break; 
                case ListenerType.Flush                  : config.AppendListeners(ListenerType.Flush, new [] { (IFlushEventListener)o }); break; 
                case ListenerType.FlushEntity            : config.AppendListeners(ListenerType.FlushEntity, new [] { (IFlushEntityEventListener)o }); break; 
                case ListenerType.Load                   : config.AppendListeners(ListenerType.Load, new [] { (ILoadEventListener)o }); break; 
                case ListenerType.LoadCollection         : config.AppendListeners(ListenerType.LoadCollection, new [] { (IInitializeCollectionEventListener)o }); break; 
                case ListenerType.Lock                   : config.AppendListeners(ListenerType.Lock, new [] { (ILockEventListener)o }); break; 
                case ListenerType.Refresh                : config.AppendListeners(ListenerType.Refresh, new [] { (IRefreshEventListener)o }); break; 
                case ListenerType.Replicate              : config.AppendListeners(ListenerType.Replicate, new [] { (IReplicateEventListener)o }); break;
                case ListenerType.SaveUpdate             : config.AppendListeners(ListenerType.SaveUpdate, new [] { (ISaveOrUpdateEventListener)o }); break; 
                case ListenerType.Save                   : config.AppendListeners(ListenerType.Save, new [] { (ISaveOrUpdateEventListener)o }); break; 
                case ListenerType.PreUpdate              : config.AppendListeners(ListenerType.PreUpdate, new [] { (IPreUpdateEventListener)o }); break; 
                case ListenerType.Update                 : config.AppendListeners(ListenerType.Update, new [] { (ISaveOrUpdateEventListener)o }); break; 
                case ListenerType.PreLoad                : config.AppendListeners(ListenerType.PreLoad, new [] { (IPreLoadEventListener)o }); break; 
                case ListenerType.PreDelete              : config.AppendListeners(ListenerType.PreDelete, new [] { (IPreDeleteEventListener)o }); break; 
                case ListenerType.PreInsert              : config.AppendListeners(ListenerType.PreInsert, new [] { (IPreInsertEventListener)o }); break; 
                case ListenerType.PostLoad               : config.AppendListeners(ListenerType.PostLoad, new [] { (IPostLoadEventListener)o }); break; 
                case ListenerType.PostInsert             : config.AppendListeners(ListenerType.PostInsert, new [] { (IPostInsertEventListener)o }); break; 
                case ListenerType.PostUpdate             : config.AppendListeners(ListenerType.PostUpdate, new [] { (IPostUpdateEventListener)o }); break; 
                case ListenerType.PostDelete             : config.AppendListeners(ListenerType.PostDelete, new [] { (IPostDeleteEventListener)o }); break; 
                case ListenerType.PostCommitUpdate       : config.AppendListeners(ListenerType.PostCommitUpdate, new [] { (IPostUpdateEventListener)o }); break; 
                case ListenerType.PostCommitInsert       : config.AppendListeners(ListenerType.PostCommitInsert, new [] { (IPostInsertEventListener)o }); break; 
                case ListenerType.PostCommitDelete       : config.AppendListeners(ListenerType.PostCommitDelete, new [] { (IPostDeleteEventListener)o }); break; 
                case ListenerType.PreCollectionRecreate  : config.AppendListeners(ListenerType.PreCollectionRecreate, new [] { (IPreCollectionRecreateEventListener)o }); break;
                case ListenerType.PreCollectionRemove    : config.AppendListeners(ListenerType.PreCollectionRemove, new [] { (IPreCollectionRemoveEventListener)o }); break;
                case ListenerType.PreCollectionUpdate    : config.AppendListeners(ListenerType.PreCollectionUpdate, new [] { (IPreCollectionUpdateEventListener)o }); break;
                case ListenerType.PostCollectionRecreate : config.AppendListeners(ListenerType.PostCollectionRecreate, new [] { (IPostCollectionRecreateEventListener)o }); break;
                case ListenerType.PostCollectionRemove   : config.AppendListeners(ListenerType.PostCollectionRemove, new [] { (IPostCollectionRemoveEventListener)o }); break;
                case ListenerType.PostCollectionUpdate   : config.AppendListeners(ListenerType.PostCollectionUpdate, new [] { (IPostCollectionUpdateEventListener)o }); break;
            }
            return config;
        }
    }
}
