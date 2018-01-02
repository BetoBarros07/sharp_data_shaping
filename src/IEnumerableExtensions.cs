using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace O7.SharpDataShaping
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, params string[] fields)
        {
            if (source is null)
                throw new ArgumentNullException("source");
            var propertiesInfo = new List<PropertyInfo>();
            foreach (var field in fields)
            {
                var propertyName = field.Trim();
                var type = typeof(TSource);
                var property = type.GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property is null)
                    throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                propertiesInfo.Add(property);
            }

            var shapedList = new List<ExpandoObject>();
            foreach (TSource sourceObject in source)
            {
                var shapedObject = new ExpandoObject();
                foreach (var propertyInfo in propertiesInfo)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)shapedObject).Add(propertyInfo.Name, propertyValue);
                }
                yield return shapedObject;
            }
        }
    }
}