using System;
using System.Reflection;

namespace CodeCovered.GeoShop.Infrastructure
{
    public static class Validate
    {
        public static void ThatArgumentNotNull<T>(Func<T> func) where T : class
        {
            string argName = GetArgumentName(func); 

            T value = func();

            if (value == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' may not be null", argName));
        }

        private static string GetArgumentName<T>(Func<T> func)
        {
            FieldInfo[] fields = func.Target.GetType().GetFields(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (fields.Length != 1)
                throw new NotSupportedException("Invalid argument");

            return fields[0].Name;
        }
    }
}