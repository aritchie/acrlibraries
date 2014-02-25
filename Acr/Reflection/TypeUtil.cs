using System;
using System.Linq;
using System.Reflection;


namespace Acr.Reflection {

    public static class TypeUtil {

        public static bool IsSimpleType(this Type type) {
            return (
                (type == typeof(bool)) ||
                (type == typeof(int)) ||
                (type == typeof(short)) ||
                (type == typeof(long)) ||
                (type == typeof(decimal)) ||
                (type == typeof(double)) ||
                (type == typeof(char)) ||
                (type == typeof(string))
            );
        }


        public static bool HasParameterlessConstructor(this Type type) {
            return type.GetTypeInfo().DeclaredConstructors.Any();
        }


        public static bool IsComplexType(this Type type) {
            return !type.IsSimpleType();
        }


        /// <summary>
        /// Use to get around retard .NET IsAssignableFrom method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsImplementationOf<T>(this Type type) {
            return type.IsImplementationOf(typeof(T));
        }


        public static bool IsImplementationOf(this Type type, Type interfaceType) {
            Verify.IsTrue(interfaceType.GetTypeInfo().IsInterface, "You can only check interfaces");
            return type
                .GetTypeInfo()
                .ImplementedInterfaces
                .Any(t => t == interfaceType);
        }


        // does not work with GetType or boxed type
        public static bool IsNullableType(this Type type) {
            return (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }


        /// <summary>
        /// Gets around nullable types to get to the real type (if the type is even nullable)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetActualType(this Type type) {
            return (type.IsNullableType() ? 
                Nullable.GetUnderlyingType(type) :
                type
            );
        }


        public static bool HasProperty(this Type type, string propertyName, Type ofType = null) {
            var p = type
                .GetTypeInfo()
                .DeclaredProperties
                .FirstOrDefault(x => x.Name == propertyName);

            if (p == null)
                return false;

            return (ofType == null || ofType == p.PropertyType);
        }
    }
}
