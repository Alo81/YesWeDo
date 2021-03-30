using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using YesweDo.Helpers;

namespace YesweDo
{
    public static class Extensions
    {
        public class EnumUtil<T> where T : struct, IConvertible
        {
            public static bool Contains(string value)
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                return Enum.IsDefined(typeof(T), value);
            }
            public static string GetByValue(string value)
            {
                try
                {
                    // This should only be a one character digit string.  If it is a number, good to try.  If not, return empty.  
                    if (Char.IsDigit(value[0]))
                    {
                        return GetByValue((int)Char.GetNumericValue(value[0]));
                    }
                    else
                    {
                        return "";
                    }
                }
                catch(Exception ex)
                {
                    UiHelper.PopUpMessage(ex.Message);
                    return "";
                }
            }
            public static string GetByValue(int value)
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                var values = (T[])Enum.GetValues(typeof(T));
                return values[value].ToString();
            }
            public static T GetByName(string name)
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                var values = (T[])Enum.GetValues(typeof(T));
                return values.FirstOrDefault(x => x.ToString().Equals(name));
            }
            public static List<string> GetValuesSorted()
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                var sorted = GetValues();
                return sorted.OrderBy(x => x.ToString()).ToList();
            }

            public static IEnumerable<string> GetValues()
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                return ((T[])Enum.GetValues(typeof(T))).Select(x => x.ToString());
            }

        }
    }

    public static class DocumentExtension
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }

    public static class ObjectExtension
    {
        public static string GetPropertyValueFromName(this object obj, string name)
        {
            return obj?.GetType()?.GetProperty(name)?.GetValue(obj)?.ToString() ?? "";
        }
        public static string GetFieldValueFromName(this object obj, string name)
        {
            return obj?.GetType()?.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(obj)?.ToString()?.ToLower() ?? "";
        }
        public static PropertyInfo GetPropertyFromName(this object obj, string name)
        {
            return obj?.GetType()?.GetProperty(name) ?? null;
        }

        public static IEnumerable<Type> GetChildrenTypes(this object obj)
        {
            var type = obj.GetType();

            var children = Assembly.GetExecutingAssembly().GetTypes().Where
                (t => t.IsClass &&
                type.IsAssignableFrom(t) &&
                t != type);

            return children;
        }

        public static void SetValueFromName(this object obj, string name, string val)
        {
            PropertyInfo field = obj?.GetType()?.GetProperty(name);
            // If val is null, interpret as empty string for our purposes.  
            val ??= "";
            if(field == null)
            {
                return;
            }
            try
            {
                field?.SetValue(obj, Convert.ChangeType(val, field.PropertyType));
            }
            catch
            {
                try
                {
                    field?.SetValue(obj, 0);    // Could be attempting to set numeric value to empty string.  Try 0 for default value.  
                }
                catch(Exception ex2)
                {
                    UiHelper.PopUpMessage(String.Format("Could not set value: {0} for {1}.\r\n\r\n{2}", val, name, ex2.Message));
                }
            }
        }
    }
}
