using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static SmashUltimateEditor.Fighter;

namespace SmashUltimateEditor
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

                var sorted = (T[]) Enum.GetValues(typeof(T));
                return sorted.OrderBy(x => x.ToString()).Select(x => x.ToString()).ToList();
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
}
