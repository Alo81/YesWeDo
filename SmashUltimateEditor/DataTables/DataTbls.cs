using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmashUltimateEditor
{
    public class DataTbls
    {
        public BattleDataTbls battleData;
        public FighterDataTbls fighterData;
        public FighterDataTbls selectedFighters;
        public BattleDataTbl selectedBattle;
        public FighterDataTbl selectedFighter;

        public int pageCount { get { return selectedFighters.pageCount + selectedBattle.pageCount; } }

        public DataTbls()
        {
            battleData = new BattleDataTbls();
            fighterData = new FighterDataTbls();
            selectedFighters = new FighterDataTbls();
            selectedBattle = new BattleDataTbl();
            selectedFighter = new FighterDataTbl();

            ReadXML(Defs.FILE_LOCATION);
        }

        public void Save(object sender, EventArgs e)
        {
            SaveBattle();
            SaveFighters();
            WriteXML(Defs.FILE_LOCATION + "progout.prc");
        }

        private void SaveBattle()
        {
            selectedBattle.UpdateValues();
            int index = battleData.battleDataList.FindIndex(x => x == selectedBattle);
            battleData.battleDataList[index] = selectedBattle;
        }

        private void SaveFighters()
        {
            foreach(FighterDataTbl fighter in selectedFighters.GetFighters())
            {
                fighter.UpdateValues();
                int index = fighterData.fighterDataList.FindIndex(x => x == fighter);
                fighterData.fighterDataList[index] = fighter;
            }
        }

        public void SetSaveButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(Save);
        }
        public void SetSelectedBattle(string battle_id)
        {
            selectedBattle = battleData.GetBattle(battle_id);
        }
        public void SetSelectedFighters(string battle_id)
        {
            selectedFighters.fighterDataList = fighterData.GetFightersByBattleId(battle_id);
        }

        public void SetSelectedFighter()
        {

        }

        public List<string> GetOptionsFromTypeAndName(string type, string name)
        {
            switch (type)
            {
                case "FighterDataTbl":
                    return (List<string>)fighterData.GetType().GetProperty(name).GetValue(fighterData) ?? new List<string>();
                case "BattleDataTbl":
                    return (List<string>)battleData.GetType().GetProperty(name).GetValue(battleData) ?? new List<string>();
                default:
                    return null;
            }
        }



        // Methods
        public void ReadXML(string fileName)
        {
            battleData.battleDataList = new List<BattleDataTbl>();
            fighterData.fighterDataList = new List<FighterDataTbl>();
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
                            battleData.battleDataList.Add(battleTbl);
                        else if (dataTable is FighterDataTbl fighterTbl)
                            fighterData.fighterDataList.Add(fighterTbl);
                        reader.Read();
                        reader.Read();
                    }

                    //Left the list.  Don't parse the next lines.  
                    parseData = false;
                    Console.WriteLine("{0} Table Complete.", dataTable.GetType().ToString());
                }
            }
            Console.WriteLine("List Built.");
        }
        public void WriteXML(string fileName)
        {
            using StreamWriter writer = new StreamWriter(fileName);

            var type = fighterData.GetFighters().GetType().Name;
            type = type.Remove(type.Length - 2);

            List<string> list = new List<string>
            {
                "Data1", "Data2", "Data3"
            };

            XDocument doc =
                new XDocument(new XDeclaration("1.0", "utf-8", ""),
                    new XElement("struct",
                        // <list hash="battle_data_tbl">	// <*DataList.Type* hash="*DataTbl.Type*">
                        new XElement(type, 
                        new XAttribute("hash", battleData.GetXmlName()),
                            //<struct index="0">	// <struct index="*DataListItem.GetIndex*">
                            battleData.GetBattles().Select(battle =>
                            new XElement("struct",
                            new XAttribute("index", 
                            battleData.GetBattles().FindIndex(ind => ind.battle_id == battle.battle_id)),
                                //<hash40 hash="battle_id">default</hash40>	// <*DataListItem.Type* hash="*DataListItem.FieldName*">*DataListItem.FieldValue*</>
                                battle.GetType().GetFields().Select( field => 
                                new XElement(field.FieldType.Name, 
                                new XAttribute("hash", DataParse.NameFixer(field.Name)), battle.GetValueFromName(field.Name))
                                    )
                                )
                            )
                        ),
                        // <list hash="fighter_data_tbl">	// <*DataList.Type* hash="*DataTbl.Type*">
                        new XElement(type,
                        new XAttribute("hash", fighterData.GetXmlName()),
                            //<struct index="0">	// <struct index="*DataListItem.GetIndex*">
                            fighterData.GetFighters().Select(fighter =>
                            new XElement("struct",
                            new XAttribute("index",
                            fighterData.GetFighters().FindIndex(ind => ind == fighter)),
                                //<hash40 hash="battle_id">default</hash40>	// <*DataListItem.Type* hash="*DataListItem.FieldName*">*DataListItem.FieldValue*</>
                                fighter.GetType().GetFields().Select(field =>
                               new XElement(field.FieldType.Name,
                               new XAttribute("hash", DataParse.NameFixer(field.Name)), fighter.GetValueFromName(field.Name))
                                    )
                                )
                            )
                        )
                    )
                );

            writer.Write(DataParse.ReplaceTypes(doc.ToString()).ToLower());
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
            while (reader.NodeType != node || reader.Name != stopper)
            {
                reader.Read();
            }

        }
    }
}
