using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;


namespace Acr.Nh.Types {

    public class DefaultStringType : IUserType {

        #region IUserType Members

        public virtual object NullSafeGet(IDataReader rs, string[] names, object owner) {
            return NHibernateUtil.String.NullSafeGet(rs, names[0]);
        }


        public virtual void NullSafeSet(IDbCommand cmd, object value, int index) {
            ((IDataParameter)cmd.Parameters[index]).Value = (value == null
                ? String.Empty
                : ((string)value).Trim()
            );
        }


        public virtual object DeepCopy(object value) {
            return value == null ? null : string.Copy((string)value);
        }


        public virtual object Replace(object original, object target, object owner) {
            return original;
        }


        public virtual object Assemble(object cached, object owner) {
            return DeepCopy(cached);
        }


        public virtual object Disassemble(object value) {
            return DeepCopy(value);
        }


        public virtual SqlType[] SqlTypes {
            get { return new[] { new SqlType(DbType.String) }; }
        }


        public Type ReturnedType {
            get { return typeof(string); }
        }


        public bool IsMutable {
            get { return false; }
        }


        public new bool Equals(object x, object y) {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null) 
                return false;
            
            return x.Equals(y);
        }


        public int GetHashCode(object x) {
            return x.GetHashCode();
        }

        #endregion
    }
}
