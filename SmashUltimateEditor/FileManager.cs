using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SmashUltimateEditor
{
    public class FileManager
    {
        public static void ReadXML(string fileName)
        {
            List<BattleDataTbl> battleDataList = new List<BattleDataTbl>();
            List<FighterDataTbl> fightDataList = new List<FighterDataTbl>();
            bool parseData = false;
            IDataTbl dataTable = new BattleDataTbl();

            using Stream stream = new FileStream(fileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(stream);

            // Read the whole file.  
            while (reader.Read())
            {
                switch (reader?.GetAttribute("hash"))
                {
                    case Defs.SPIRIT_BATTLE_DATA_XML:
                        dataTable = new BattleDataTbl();
                        parseData = true;
                        break;
                    case Defs.FIGHTER_DATA_XML:
                        dataTable = new FighterDataTbl();
                        parseData = true;
                        break;
                }

                if (parseData)
                {
                    // Read until start of data.  
                    ReadUntilType("struct", reader);
                    // Lists have index values, so keep reading until we've left the list.  
                    while (reader.GetAttribute("index") != null)
                    {
                        dataTable = (IDataTbl)Activator.CreateInstance(dataTable.GetType());
                        dataTable.BuildFromXml(reader);
                        if (dataTable is BattleDataTbl battleTbl)
                            battleDataList.Add(battleTbl);
                        else if (dataTable is FighterDataTbl fighterTbl)
                            fightDataList.Add(fighterTbl);
                        reader.Read();
                        reader.Read();
                    }

                    //Left the list.  Don't parse the next lines.  
                    parseData = false;
                    Console.WriteLine("{0} Table Complete.", dataTable.GetType().ToString());
                }
            }
            Console.WriteLine("List Built.");
            GetFieldsValues(battleDataList);
            GetFieldsValues(fightDataList);
        }

        public static List<KeyValuePair<string, string>> GetFieldsValues(List<BattleDataTbl> dataTbl)
        {
            List<string> fields = new List<string>(typeof(BattleDataTbl).GetFields().Select(x => x.Name));
            var storage = new List<KeyValuePair<string, string>>();

            foreach (BattleDataTbl tbl in dataTbl)
            {
                foreach (string field in fields)
                {
                    var val = tbl.GetType().GetField(field).GetValue(tbl);
                    var kvp = new KeyValuePair<string, string>(field, val?.ToString() ?? "");
                    if (!storage.Contains(kvp))
                    { 
                        storage.Add(kvp);
                        if (DataParse.BattleDataTblDupes.ContainsKey(kvp.Key))
                        {
                            var dupeField = DataParse.BattleDataTblDupes[kvp.Key];
                            var dupeKvp = new KeyValuePair<string, string>(dupeField, val?.ToString() ?? "");
                            if (!storage.Contains(dupeKvp))
                            {
                                storage.Add(dupeKvp);
                            }
                        }
                    }
                }
            }

            storage.OrderBy(x => x.Key);
            return storage;
        }

        public static List<KeyValuePair<string, string>> GetFieldsValues(List<FighterDataTbl> dataTbl)
        {
            List<string> fields = new List<string>(typeof(FighterDataTbl).GetFields().Select(x => x.Name));
            var storage = new List<KeyValuePair<string, string>>();

            foreach (FighterDataTbl tbl in dataTbl)
            {
                foreach (string field in fields)
                {
                    var val = tbl.GetType().GetField(field).GetValue(tbl);
                    var kvp = new KeyValuePair<string, string>(field, val?.ToString() ?? "");
                    if (!storage.Contains(kvp))
                    {
                        storage.Add(kvp);
                        if (DataParse.FighterDataTblDupes.ContainsKey(kvp.Key))
                        {
                            var dupeField = DataParse.FighterDataTblDupes[kvp.Key];
                            var dupeKvp = new KeyValuePair<string, string>(dupeField, val?.ToString() ?? "");
                            if (!storage.Contains(dupeKvp))
                            {
                                storage.Add(dupeKvp);
                            }
                        }
                    }
                }
            }

            storage.OrderBy(x => x.Key);
            return storage;
        }

        // Maybe use a list of tuples for string/Node pairs?  
        public static void ReadUntilType(string stopper, XmlReader reader, XmlNodeType node = XmlNodeType.Element)
        {
            while (reader.NodeType != node ||  reader.Name != stopper)
            {
                reader.Read();
            }

        }
    }
}
