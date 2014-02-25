using System;
using System.Linq;
using System.Reflection;


namespace Acr.Reflection {

    public static class ObjectUtil {

        public static void TryCastAction<T>(this object obj, Action<T> ifCast) where T : class {
            var cast = obj as T;
            if (cast != null) {
                ifCast(cast);
            }
        }


        public static T GetProperty<T>(this object obj, string propertyName) {
            return (T)obj.GetProperty(propertyName);
        }


        public static object GetProperty(this object obj, string propertyName) {
            var property = obj
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .FirstOrDefault(x => x.Name == propertyName);

            Verify.IsNotNull(property, "Property not found");
            Verify.IsTrue(property.CanRead, "Property cannot be read");

            return property.GetValue(obj, null);
        }


        public static void SetProperty(this object obj, string propertyName, object value) {
            var property = obj
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .FirstOrDefault(x => x.Name == propertyName);

            Verify.IsNotNull(property, "Property not found");
            Verify.IsTrue(property.CanWrite, "Property cannot be written to");

            property.SetValue(obj, value, null);
        }
    }
}