using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmashUltimateEditor.Helpers
{
    class StatsHelper
    {
        public static void GetCountPerField(IDataOptions tbl, string field)
        {
            var results = tbl.dataList.GroupBy(x => x.GetPropertyValueFromName(field))
              .Select(g => new
              {
                  Field = g.Key,
                  Count = g.Select(l => l.GetPropertyValueFromName(field)).Count()
              }).OrderByDescending(x => x.Count);
        }

        public static IEnumerable<string> GetBattlesWhereFieldIsValue(IDataOptions tbl, string field, string val)
        {
            var results = tbl.dataList.Where(x => x.GetPropertyValueFromName(field) == val);
            var battleIdStrings = results.Select(x => x.GetPropertyValueFromName("battle_id"));

            return battleIdStrings;
        }
        public static void GetNamesPerType(Type type)
        {
            var allProperties = new List<string>();
            var dataTblTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass
                                    && t.IsSubclassOf(typeof(DataTbl)));

            var dataTblPropertiesMatchingType = dataTblTypes.Select(x => x.GetProperties().Where(y => y.PropertyType == type));

            foreach (var prop in dataTblPropertiesMatchingType)
            {
                foreach(var indProp in prop)
                {
                    allProperties.Add(indProp.Name);
                }
            }
        }

        public static void WriteHashSetToFile(HashSet<string> dict, string fileName)
        {
            using StreamWriter writer = new StreamWriter(fileName);

            foreach (var word in dict.OrderBy(x => x))
            {
                writer.WriteLine(word);
            }

            writer.Close();
            writer.Dispose();
        }

        public static void GetDictionaryFromLabels(string inFileName, string outFileName)
        {
            var delim = '_';
            var file = File.Open(inFileName, FileMode.Open);
            var reader = new StreamReader(file);
            HashSet<string> dict = new HashSet<string>();
            HashSet<string> comboDict = new HashSet<string>();

            while (!reader.EndOfStream)
            {
                var label = reader.ReadLine();
                var subWords = label.Split(delim);

                foreach(var word in subWords.Where(x => x.Length > 0 && !Char.IsDigit(x[0])))
                {
                    dict.Add(word);
                }
            }

            WriteHashSetToFile(dict, outFileName);
        }




    }
}
