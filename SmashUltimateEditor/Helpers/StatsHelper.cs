using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YesWeDo.DataTableCollections;
using YesWeDo.DataTables;
using static YesWeDo.DataTables.DataTbl;

namespace YesweDo.Helpers
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
        public static void GetNumericStatsForField(IDataOptions tbl, string field)
        {
            var results = tbl.dataList.GroupBy(x => x.GetPropertyValueFromName(field))
              .Select(g => new
              {
                  Field = Convert.ToInt32(g.Key),
                  Count = g.Select(l => l.GetPropertyValueFromName(field)).Count()
              }).OrderBy(x => x.Count);

            var leastCommon = results.Where(x => x.Field > 1).First();
            var midCommon = results.ElementAt(results.Count() / 2);
            var mostCommon = results.Last();

            results = results.OrderBy(x => x.Field);

            var min = results.Where(x => x.Field > 1).First();
            var avg = results.Average(x => x.Field);
            var max = results.Last();

            var vals = new List<int>() { leastCommon.Field, midCommon.Field, mostCommon.Field, min.Field, max.Field };

            var examples = new List<IDataTbl>();

            examples.AddRange(GetTblsWhereFieldIsValue(tbl, field, vals));
        }

        public static IEnumerable<string> GetBattleIdsWhereFieldIsValue(IDataOptions tbl, string field, string val)
        {
            var results = tbl.dataList.Where(x => x.GetPropertyValueFromName(field) == val);
            var battleIdStrings = results.Select(x => x.GetPropertyValueFromName("battle_id"));

            return battleIdStrings;
        }
        public static IEnumerable<string> GetBattleIdsWhereFieldIsLikeValue(IDataOptions tbl, string field, string val)
        {
            var results = tbl.dataList.Where(x => x.GetPropertyValueFromName(field).Contains(val));
            var battleIdStrings = results.Select(x => x.GetPropertyValueFromName("battle_id")).Distinct();

            return battleIdStrings;
        }
        public static IEnumerable<IDataTbl> GetTblsWhereFieldIsValue(IDataOptions tbl, string field, IEnumerable<int> vals)
        {
            var results = new List<IDataTbl>();
            foreach (var val in vals)
            {
                results.Add(tbl.dataList.Where(x => x.GetPropertyValueFromName(field) == val.ToString()).FirstOrDefault());
            }

            return results;
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
        public static void GetAttributeValues(IEnumerable<IDataOptions> tbls, string attribute)
        {
            var results = new Dictionary<string, string>();
            foreach(var options in tbls)
            {
                foreach (var tbl in options.dataList)
                {
                    foreach (var prop in tbl.GetType().GetProperties())
                    {
                        var attributeInfo = prop?.GetCustomAttribute<ToolTipAttribute>()?.ToolTip;
                        if(attributeInfo != null)
                        {
                            results.TryAdd(prop.Name, attributeInfo);
                        }
                    }
                }
            }
            results.Distinct();
        }

        public static void WriteHashSetToFile(IEnumerable<string> dict, string fileName)
        {
            using StreamWriter writer = new StreamWriter(fileName);

            foreach (var word in dict.OrderBy(x => x))
            {
                writer.WriteLine($"_{word}");
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

        public static void CreateDictionaryFromFiles(string inFolderName, string outFileName, string fileTypeToSearch = "out.prc", string delim = "_")
        {
            var dict = new HashSet<string>();

            GetDictionaryFromFiles(inFolderName, fileTypeToSearch, ref dict);

            SplitDictionaryOnDelim(ref dict, delim);

            WriteHashSetToFile(dict, outFileName);
        }

        public static void CreateDictionaryFromFilesByLength(string inFolderName, string outFileName, string fileTypeToSearch = "out.prc", string delim = "_")
        {
            var dict = new HashSet<string>();

            GetDictionaryFromFiles(inFolderName, fileTypeToSearch, ref dict);

            SplitDictionaryOnDelim(ref dict, delim);
            for(int i = 0; i < dict.Max(x => x.Length); i++)
            {
                WriteHashSetToFile(dict.Where(x => x.Length == (i)), $"{outFileName}_U_{(i+1)}");
            }
        }

        public static void SplitDictionaryOnDelim(ref HashSet<string> dict, string delim)
        {
            var subDict = new HashSet<string>();
            foreach(var word in dict)
            {
                var subWords = word.Split(delim);

                foreach (var subWord in subWords.Where(x => x.Length > 0 && !Char.IsDigit(x[0])))
                {
                    subDict.Add(subWord);
                }
            }

            foreach(var word in subDict)
            {
                dict.Add(word);
            }
        }

        public static void GetDictionaryFromFiles(string inFolderName, string fileType, ref HashSet<string> dict)
        {
            var delim = '_';
            var dir = new DirectoryInfo(inFolderName);

            foreach (var folder in dir.EnumerateDirectories())
            {
                GetDictionaryFromFiles(folder.FullName, fileType, ref dict);
            }

            foreach(var subFile in dir.EnumerateFiles().Where(x => x.Name.EndsWith(fileType)))
            {
                var stream = XmlHelper.GetStreamFromFile(subFile.FullName);
                var reader = XmlHelper.GetXmlReaderFromStream(stream);

                while (!reader.EOF)
                {
                    reader.Read();
                    var hash = reader?.GetAttribute("hash");
                    if(hash != null)
                    {
                        dict.Add(hash);
                        reader.Read();
                        dict.Add(reader?.Value);
                    }
                }
            }




        }




    }
}
