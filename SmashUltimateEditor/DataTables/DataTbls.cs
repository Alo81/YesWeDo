using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static SmashUltimateEditor.DataTables.DataTbl;
using static System.Windows.Forms.TabControl;

namespace SmashUltimateEditor
{
    public class DataTbls
    {
        public BattleDataTbls battleData;
        public FighterDataTbls fighterData;
        public List<Fighter> selectedFighters;
        public Battle selectedBattle;
        public Fighter selectedFighter;
        public Queue<TabPage> tabStorage = new Queue<TabPage>();

        public TabControl tabs;

        public int pageCount { get { return selectedFighters.Sum(x => x.pageCount) + selectedBattle.pageCount; } }
        public int tabCount { get { return tabs.TabPages.Count; } }
        public TabPage EmptyBattlePage
        {
            get
            {
                return BuildEmptyPage(this, typeof(Battle));
            } 
        }

        public TabPage EmptyFighterPage
        {
            get
            {
                return BuildEmptyPage(this, typeof(Fighter));
            }
        }

    public bool HasEnoughPages { get { return tabs.TabPages.Count >= pageCount; } }

        public DataTbls()
        {
            battleData = new BattleDataTbls();
            fighterData = new FighterDataTbls();
            selectedFighters = new List<Fighter>();
            selectedBattle = new Battle();
            selectedFighter = new Fighter();

            ReadXML(Defs.FILE_LOCATION);
        }

        public void Save(BattleDataTbls battleData, FighterDataTbls fighterData, string fileLocation = Defs.FILE_LOCATION)
        {
            SaveBattle();
            SaveFighters();
            WriteXML(fileLocation, battleData, fighterData);
        }

        public void SaveBattle()
        {
            TabPage battlePage = tabs.TabPages.Count > 0 ? tabs.TabPages[0] : null;
            // No battle?
            if(battlePage is null)
            {
                return;
            }
            selectedBattle.UpdateTblValues(battlePage);
            int index = battleData.battleDataList.FindIndex(x => x == selectedBattle);
            battleData.battleDataList[index] = selectedBattle;
        }

        public void SaveFighters()
        {
            TabPageCollection fighterPages = tabs.TabPages;
            // No fighters?
            if (fighterPages.Count <= 1)
            {
                return;
            }

            for (int i = 0; i< fighterPages.Count-1; i++)
            {
                // Match on tab index, which is assigned to Fighter when it updates page values.  
                selectedFighters[i].UpdateTblValues(fighterPages[i+1]);
                int index = fighterData.fighterDataList.FindIndex(x => x == selectedFighters[i]);
                fighterData.fighterDataList[index] = selectedFighters[i];
            }
        }

        public void RandomizeAll(int seed = -1)
        {
            Random rnd = new Random();
            FighterDataTbls randomizedFighters = new FighterDataTbls();
            Fighter randomizedFighter = new Fighter();
            int fighterCount;
            int eventCount;
            List<int> fighterDistribution = BuildMinDistributionList(1, 8);
            List<int> eventDistribution = BuildMinDistributionList(1, 3, 1, 0);

            foreach (Battle battle in battleData.battleDataList)
            {
                battle.Randomize(rnd, this);
                eventCount = eventDistribution[rnd.Next(eventDistribution.Count)];
                fighterCount = fighterDistribution[rnd.Next(fighterDistribution.Count)];

                for (int j = 1; j <= eventCount; j++)
                {
                    var randEvent = battleData.events[rnd.Next(battleData.events.Count)];
                    var eventNum = String.Format("event{0}_", j);
                    // event1_type, event1_ label,  event1_ start_time, event1_ range_time, event1_ count, event1_ damage
                    battle.SetValueFromName(eventNum + "type", randEvent.Item1);
                    battle.SetValueFromName(eventNum + "label", randEvent.Item2);
                    battle.SetValueFromName(eventNum + "start_time", randEvent.Item3.ToString());
                    battle.SetValueFromName(eventNum + "range_time", randEvent.Item4.ToString());
                    battle.SetValueFromName(eventNum + "count", randEvent.Item5.ToString());
                    battle.SetValueFromName(eventNum + "damage", randEvent.Item6.ToString());
                }

                for(int i = 0; i < fighterCount; i++)
                {
                    randomizedFighter = new Fighter() { battle_id = battle.battle_id, spirit_name = battle.battle_id, entry_type = "main_type"};
                    if(i!= 0)
                    {
                        randomizedFighter.entry_type = "sub_type";
                    }
                    randomizedFighter.Randomize(rnd, this);

                    randomizedFighters.AddFighter(randomizedFighter);
                }

            }
            Save(battleData, randomizedFighters, Defs.FILE_LOCATION + "_Randomized");
            BuildTabs();
        }

        public List<int> BuildMinDistributionList(int preferred, int max, double mod1 = 1.0, double mod2 = 0.5)
        {
            var lint = new List<int>();

            for (int i = preferred; i <= max; i++)
            {
                for (int j = preferred; j <= i*mod1; j++)
                {
                    lint.Add(j);
                }
            }
            // Make one fighter more likely significantly.  
            for (int i = (int)(lint.Count * mod2); i > 0; i--)
            {
                lint.Add(preferred);
            }

            return lint;
        }

        public void RemoveFighterFromButtonNamedIndex(object sender, EventArgs e)
        {
            int index = Int32.Parse(((Button)sender).Name);
            selectedFighters.Remove(fighterData.fighterDataList[index]);
            fighterData.fighterDataList.RemoveAt(index);
            BuildTabs();
        }
        
        public void SetRemoveFighterButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(RemoveFighterFromButtonNamedIndex);
        }
        
        public void BuildTabs()
        {
            //var tasks = new List<Task<TabPage>>();
            TabPage page;
            if(tabs is null)
            {
                tabs = new TabControl();
            }
            ShowTabs();
            BuildEmptyTabs();

            for (int i = 0; i < pageCount; i++)
            {
                page = tabs.TabPages[i];
                // Battle
                if (i == 0)
                {
                    var collectionIndex = battleData.GetBattleIndex(selectedBattle);
                    selectedBattle.UpdatePageValues(ref page, i, selectedBattle.battle_id, collectionIndex);
                }
                else
                {
                    var collectionIndex = fighterData.GetFighterIndex(selectedFighters[i - 1]);
                    selectedFighters[i - 1].UpdatePageValues(ref page, i, selectedFighters[i - 1].fighter_kind, collectionIndex);
                }
                //tabs.TabPages.Add(page);
            }

            HideTabs();
        }

        public void BuildEmptyTabs()
        {
            TabPage page;

            if (tabCount == 0)
            {
                page = EmptyBattlePage;
                tabs.TabPages.Add(page);
            }

            while (!HasEnoughPages)
            {
                page = EmptyFighterPage;
                tabs.TabPages.Add(page);
            }
        }

        public void HideTabs()
        {
            for (int i = pageCount; i < tabCount;)
            {
                tabStorage.Enqueue(tabs.TabPages[i]);
                tabs.TabPages.RemoveAt(i);
            }
        }

        public void ShowTabs()
        {
            while(tabStorage.Count > 0 && !HasEnoughPages)
            {
                tabs.TabPages.Add(tabStorage.Dequeue());
            }
        }



        public void SetSelectedBattle(string battle_id)
        {
            selectedBattle = battleData.GetBattle(battle_id);
        }
        public void SetSelectedFighters(string battle_id)
        {
            selectedFighters = fighterData.GetFightersByBattleId(battle_id);
        }

        public List<string> GetOptionsFromTypeAndName(string type, string name)
        {
            switch (type)
            {
                case "Fighter":
                    return (List<string>)fighterData.GetType().GetProperty(name).GetValue(fighterData) ?? new List<string>();
                case "Battle":
                    return (List<string>)battleData.GetType().GetProperty(name).GetValue(battleData) ?? new List<string>();
                default:
                    return null;
            }
        }

        // Methods
        public void ReadXML(string fileName)
        {
            battleData.battleDataList = new List<Battle>();
            fighterData.fighterDataList = new List<Fighter>();
            bool parseData = false;
            IDataTbl dataTable = new Battle();

            using Stream stream = new FileStream(fileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(stream);

            // Read the whole file.  
            while (reader.Read())
            {
                switch (reader?.GetAttribute("hash"))
                {
                    case Defs.SPIRIT_BATTLE_DATA_XML:
                        dataTable = new Battle();
                        parseData = true;
                        break;
                    case Defs.FIGHTER_DATA_XML:
                        dataTable = new Fighter();
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
                        if (dataTable is Battle battleTbl)
                            battleData.battleDataList.Add(battleTbl);
                        else if (dataTable is Fighter fighterTbl)
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
        public void WriteXML(string fileName, BattleDataTbls battleData, FighterDataTbls fighterData)
        {
            using StreamWriter writer = new StreamWriter(fileName);

            var type = fighterData.GetFighters().GetType().Name;
            type = type.Remove(type.Length - 2);

            XDocument doc =
                new XDocument(new XDeclaration("1.0", "utf-8", null),
                    new XElement("struct",
                        // <list hash="battle_data_tbl">	// <*DataList.Type* hash="*DataTbl.Type*">
                        new XElement(type, 
                        new XAttribute("hash", battleData.GetXmlName()),
                            //<struct index="0">	// <struct index="*DataListItem.GetIndex*">
                            battleData.GetBattles().Select(battle =>
                            battle.GetAsXElement(battleData.GetBattleIndex(battle)
                            )
                            )
                        ),
                        // <list hash="fighter_data_tbl">	// <*DataList.Type* hash="*DataTbl.Type*">
                        new XElement(type,
                        new XAttribute("hash", fighterData.GetXmlName()),
                            //<struct index="0">	// <struct index="*DataListItem.GetIndex*">
                            fighterData.GetFighters().Select(fighter =>
                            fighter.GetAsXElement(fighterData.GetFighterIndex(fighter)
                            )
                            )
                        )
                    )
                );

            writer.Write(doc.Declaration.ToString() + "\r\n" + DataParse.ReplaceTypes(doc.ToString()).ToLower());
            writer.Close();
            writer.Dispose();
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
