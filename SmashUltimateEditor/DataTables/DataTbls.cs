using paracobNET;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_spirits_battle_db;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static SmashUltimateEditor.DataTables.DataTbl;
using static System.Windows.Forms.TabControl;

namespace SmashUltimateEditor
{
    public partial class DataTbls
    {
        public BattleDataOptions battleData;
        public FighterDataOptions fighterData;
        public EventDataOptions eventData;
        public ItemDataOptions itemData;
        public SpiritFighterDataOptions spiritFighterData;

        public List<Fighter> selectedFighters;
        public Battle selectedBattle;
        public Fighter selectedFighter;
        public Queue<TabPage> tabStorage = new Queue<TabPage>();

        public Config config;

        public TabControl tabs;
        public ProgressBar progress;
        public bool encrypt;
        public bool decrypt;

        public int pageCount { get { return selectedFighters.Sum(x => x.pageCount) + selectedBattle.pageCount; } }
        public int tabCount { get { return tabs.TabPages.Count; } }
        public TabPage EmptyBattlePage
        {
            get
            {
                var page = BuildEmptyPage(this, typeof(Battle));
                SetEventOnChange(ref page);
                if(eventData.GetCount() != 0)
                    SetEventTypeForPage(ref page);
                return page;
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
        public string FileLocation
        {
            get
            {
                return encrypt ? config.file_location_encr : config.file_location;
            }
        }

        public DataTbls()
        {
            battleData = new BattleDataOptions();
            fighterData = new FighterDataOptions();
            eventData = new EventDataOptions();
            itemData = new ItemDataOptions();
            spiritFighterData = new SpiritFighterDataOptions();
            selectedFighters = new List<Fighter>();
            selectedBattle = new Battle();
            selectedFighter = new Fighter();
            config = new Config();

            encrypt = false;
            decrypt = false;

            var results = ReadXML(config.file_location);
            //ref battleData, ref fighterData
            battleData.SetBattles(results.GetBattles());
            fighterData.SetFighters(results.GetFighters());
            eventData.SetEvents(results.GetEvents());
        }

        public void EmptySpiritData()
        {
            battleData = new BattleDataOptions();
            fighterData = new FighterDataOptions();
        }
        public void UpdateEventsForDbValues()
        {
            if(eventData.GetCount() == 0)
            {
                return;
            }

            // Visible pages.
            for(int i = 0; i < tabs.TabCount; i++)
            {
                var page = tabs.TabPages[i];
                SetEventTypeForPage(ref page);
            }

            // Stored pages.
            for (int i = 0; i < tabStorage.Count; i++)
            {
                var page = tabStorage.Dequeue();
                SetEventTypeForPage(ref page);
                tabStorage.Enqueue(page);
            }
        }

        public void Save()
        {
            Save(FileLocation);
        }

        public void Save(string fileLocation)
        {
            Save(battleData, fighterData, fileLocation);
        }
        public void Save(BattleDataOptions battleData, FighterDataOptions fighterData)
        {
            Save(battleData, fighterData, FileLocation);
        }

        // This isn't following proper standard.  Fix it.  
        public void SaveRandomized(BattleDataOptions battleData, FighterDataOptions fighterData)
        {
            Save(battleData, fighterData, config.file_directory_randomized + config.file_name_encr);
        }

        // We should make it so directory and name are passed separately.  That way we can build a modified name, or add a modified directory.  
        public void Save(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, string file_name = "")
        {
            SaveLocal();
            if (encrypt)
            {
                SaveToEncryptedFile(battleData, fighterData, fileLocation);
            }

            if(decrypt)
            {
                SaveToFile(battleData, fighterData, fileLocation + "_unencr");
            }
        }

        public void SaveToEncryptedFile(BattleDataOptions battleData, FighterDataOptions fighterData, string fileLocation, EventDataOptions eventData = null)
        {
            var doc = BuildXml(battleData, fighterData);

            AssmebleEncrypted(doc.ToXmlDocument(), fileLocation);
        }

        public void SaveToFile(BattleDataOptions battleData, FighterDataOptions fighterData,  string fileLocation, EventDataOptions eventData = null)
        {
            var doc = BuildXml(battleData, fighterData);
            WriteXmlToFile(fileLocation, doc);
        }

        public void SaveLocal()
        {
            SaveBattle();
            SaveFighters();
        }

        public void ImportBattle(string file_loc)
        {

            var battles = new BattleDataOptions();
            var fighters = new FighterDataOptions();
            var results = ReadXML(file_loc);
            
            battles.SetBattles(results.GetBattles());
            fighters.SetFighters(results.GetFighters());

            battleData.ReplaceBattles(battles);
            fighterData.ReplaceFighters(fighters);

            var battle_id = battles.GetBattleAtIndex(0).battle_id;

            SetSelectedBattle(battle_id);
            SetSelectedFighters(battle_id);
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
            int index = battleData.GetBattleIndex(selectedBattle);
            if (index >= 0)
            {
                battleData.ReplaceBattleAtIndex(index, selectedBattle);
            }
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
                int index = fighterData.GetFighterIndex(selectedFighters[i]);
                if (index >= 0)
                {
                    fighterData.ReplaceFighterAtIndex(index, selectedFighters[i]);
                }
            }
        }

        public void SetupRandomizeProgress()
        {
            progress.Visible = true;
            progress.Minimum = 0;
            progress.Maximum = battleData.GetCount() * 4;
            progress.Value = progress.Minimum;
            progress.Step = 1;
        }

        public void RandomizeAll(int seed = -1)
        {
            Random rnd = new Random(seed);
            FighterDataOptions randomizedFighters = new FighterDataOptions();
            Fighter randomizedFighter;
            int fighterCount;
            List<string> unlockableFighters = spiritFighterData.chara_id;

            SetupRandomizeProgress();

            foreach (Battle battle in battleData.GetBattles())
            {
                battle.Randomize(ref rnd, this);

                // Save after randomizing, as we'll be setting battle_Type to HP, and modifying fighter to make one a boss.  
                var isUnlockableFighterType = spiritFighterData.IsUnlockableFighter(battle.battle_id);

                // If lose escort, need at least 2 fighters.  
                fighterCount = battle.IsLoseEscort() ?
                    RandomizerHelper.fighterLoseEscortDistribution[rnd.Next(RandomizerHelper.fighterLoseEscortDistribution.Count)]
                    :
                    RandomizerHelper.fighterDistribution[rnd.Next(RandomizerHelper.fighterDistribution.Count)];

                battle.Cleanup(ref rnd, fighterCount, this);

                progress.PerformStep();

                for (int i = 0; i < fighterCount; i++)
                {
                    string unlockableFighter = null;

                    var isMain = i == 0;     // Set first fighter to main.  
                    var isUnlockableFighter = isMain && isUnlockableFighterType;     // If unlockable fighter, we will explicitly set a fighter to match the spirit.  
                    var isBoss = isMain && battle.IsBossType();     // Set first fighter to main.  
                    var isLoseEscort = i == 1 && battle.IsLoseEscort();     // Set second fighter to ally, if Lose Escort result type.  

                    randomizedFighter = battle.GetNewFighter();
                    randomizedFighter.Randomize(ref rnd, this);

                    if (isUnlockableFighter)
                    {
                        int fighterIndex = rnd.Next(unlockableFighters.Count);
                        unlockableFighter = unlockableFighters[fighterIndex];
                        unlockableFighters.RemoveAt(fighterIndex);
                    }

                    randomizedFighter.Cleanup(ref rnd, isMain, isLoseEscort, fighterData.Fighters, isBoss, unlockableFighter);
                    randomizedFighter.StockCheck(fighterCount);

                    randomizedFighters.AddFighter(randomizedFighter);
                    progress.PerformStep();
                }

            }
            SaveRandomized(battleData, randomizedFighters);
            RefreshTabs();
            progress.Visible = false;
            MessageBox.Show(String.Format("Spirit Battles Randomized.\r\nChaos: {0}. \r\nLocation: {1}", config.chaos.ToString(), FileLocation + "_Randomized"));
        }

        public void RemoveFighterFromButtonNamedIndex(object sender, EventArgs e)
        {
            int index = Int32.Parse(((Button)sender).Name);
            selectedFighters.Remove(fighterData.GetFighterAtIndex(index));
            fighterData.RemoveFighterAtIndex(index);
            RefreshTabs();
        }
        
        public void SetRemoveFighterButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(RemoveFighterFromButtonNamedIndex);
        }

        public void SetSaveTabChange(object sender, EventArgs e)
        {
            SaveLocal();
        }
        
        public void RefreshTabs()
        {
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
                    selectedBattle.CorrectEventLabels(ref page, this);
                    selectedBattle.UpdatePageValues(ref page, i, selectedBattle.battle_id, collectionIndex);
                }
                else
                {
                    if(i == 1)
                    {
                        if(tabCount == 2)
                            DisableFighterButton(ref page);
                        else
                            EnableFighterButton(ref page);
                    }
                    var collectionIndex = fighterData.GetFighterIndex(selectedFighters[i - 1]);
                    selectedFighters[i - 1].UpdatePageValues(ref page, i, selectedFighters[i - 1].fighter_kind, collectionIndex);
                }
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

        public void DisableFighterButton(ref TabPage page)
        {
            Button button = page.Controls.OfType<Button>().FirstOrDefault();
            button.Enabled = false;
        }

        public void EnableFighterButton(ref TabPage page)
        {
            Button button = page.Controls.OfType<Button>().FirstOrDefault();
            button.Enabled = true;
        }

        public void SetEventTypeForPage(ref TabPage page)
        {
            foreach (ComboBox control in page.Controls.OfType<ComboBox>())
            {
                if (Regex.IsMatch(control.Name, "event.*type"))
                {
                    control.DataSource = eventData.event_type;
                }
            }
        }

        public void SetEventOnChange(ref TabPage page)
        {
            foreach(ComboBox control in page.Controls.OfType<ComboBox>())
            {
                if (Regex.IsMatch(control.Name, "event.*type"))
                {
                    control.SelectedIndexChanged += SetEventLabelOptions;
                }
            }
        }

        public void SetEventLabelOptions(object sender, EventArgs e = null)
        {
            if(eventData.GetCount() == 0)
            {
                return;
            }

            var combo = ((ComboBox)sender);
            SetEventLabelOptions(combo, tabs.SelectedTab);
        }
        public void SetEventLabelOptions(ComboBox combo, TabPage page)
        {
            if (eventData.GetCount() == 0)
            {
                return;
            }

            var labelType = combo?.SelectedItem?.ToString();
            var labelComboName = "event#_label";

            foreach (char character in combo.Name)
            {
                if (Char.IsDigit(character))
                {
                    labelComboName = labelComboName.Replace('#', character);
                    break;
                }
            }

            SetEventLabelOptions(labelComboName, labelType, page);
        }
        public void SetEventLabelOptions(string labelComboName, string labelType, TabPage page)
        {
            if (eventData.GetCount() == 0)
            {
                return;
            }

            var controls = page.Controls.OfType<ComboBox>();

            var labels = eventData.GetLabelsOfType(labelType);

            foreach (ComboBox control in controls)
            {
                if (control.Name == labelComboName)
                {
                    control.DataSource = labels;
                    return;
                }
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

        public List<string> GetOptionsFromTypeAndName(Type type, string name)
        {
            Type fighterType = fighterData.GetContainerType();
            Type battleType = battleData.GetContainerType();

            if (type == fighterType)
            {
                return fighterData.GetOptionsFromName(name) ?? new List<string>();
            }
            if (type == battleType)
            {
                return battleData.GetOptionsFromName(name) ?? new List<string>();
            }
            return null;
        }

        public string GetModeFromTypeAndName(Type type, string name)
        {
            Type fighterType = fighterData.GetContainerType();
            Type battleType = battleData.GetContainerType();

            var options = new List<string>();

            if (type == fighterType)
            {
                options = fighterData.GetOptionsFromName(name) ?? new List<string>();
            }
            if (type == battleType)
            {
                options = battleData.GetOptionsFromName(name) ?? new List<string>();
            }

            var groups = options.GroupBy(v => v);
            int maxCount = groups.Max(g => g.Count());
            string mode = groups.First(g => g.Count() == maxCount).Key;

            return mode;
        }

        // Methods
        public DataOptions ReadXML(string fileName)
        {
            bool parseData = false;
            bool firstPass = false;
            bool sharedXmlName;
            IDataTbl dataTable;
            var results = new DataOptions();

            // Copy the stream to memory, so we're not holding the resource open.  
            MemoryStream stream = new MemoryStream();

            using (Stream fileStream = new FileStream(fileName, FileMode.Open))
            {
                fileStream.CopyTo(stream);
                stream.Position = 0;
            }

            XmlReader reader = XmlReader.Create(stream);

            try
            {
                reader.Read();
                // Read the whole file.  
                while (!reader.EOF)
                {
                    dataTable = GetDataTblFromXmlName(reader?.GetAttribute("hash"));

                    if(dataTable != null)
                    {
                        parseData = true;
                        firstPass = true;
                        sharedXmlName = dataTable.GetType() == typeof(DataTbl);

                        if (sharedXmlName)
                        {
                            MemoryStream firstLevelCopy = new MemoryStream();
                            // Store the position, read from the beginning, restore the position.
                            var position = stream.Position;
                            stream.Position = 0;
                            dataTable = DetermineXmlTypeFromFirstLevel(stream);
                            stream.Position = position;
                        }
                    }

                    if (parseData)
                    {
                        // Read until start of data.  
                        XmlHelper.ReadUntilName(reader, stopper: "struct");

                        // Lists have index values, so keep reading until we've left the list.  
                        // Added first pass to handle when a struct is not in a list.  
                        while (firstPass || reader.GetAttribute("index") != null)
                        {
                            dataTable = (IDataTbl)Activator.CreateInstance(dataTable.GetType());
                            dataTable.BuildFromXml(reader);

                            results.AddDataTbl(dataTable);

                            XmlHelper.ReadUntilNodeType(reader, node: XmlNodeType.Element);

                            firstPass = false;
                        }

                        //Left the list.  Don't parse the next lines.  
                        parseData = false;
                        Console.WriteLine("{0} Table Complete.", dataTable.GetType().ToString());
                        
                        continue;
                    }

                    reader.Read();
                }
                return results;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



        public XDocument BuildXml(BattleDataOptions battleData, FighterDataOptions fighterData)
        {

            var type = fighterData.GetFighters().GetType().Name.ToLower();
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

            return doc;
        }

        public void WriteXmlToFile(string fileName, XDocument xmlDoc)
        {
            using StreamWriter writer = new StreamWriter(fileName);

            writer.Write(xmlDoc.Declaration.ToString() + "\r\n" + xmlDoc.ToString());
            writer.Close();
            writer.Dispose();
        }

        #region PRC Cryptography

        static ParamFile file { get; set; }
        static XmlDocument xml { get; set; }
        static string labelName { get; set; }
        static OrderedDictionary<ulong, string> hashToStringLabels { get; set; }
        static OrderedDictionary<string, ulong> stringToHashLabels { get; set; }

        void AssmebleEncrypted(XmlDocument doc, string fileLocation)
        {
            labelName = config.labels_file_location;
            stringToHashLabels = new OrderedDictionary<string, ulong>();
            if (!string.IsNullOrEmpty(labelName))
                stringToHashLabels = LabelIO.GetStringHashDict(labelName);

            file = new ParamFile(Node2ParamStruct(doc.DocumentElement));

            file.Save(fileLocation);
        }

        static XmlNode Param2Node(IParam param)
        {
            switch (param.TypeKey)
            {
                case ParamType.@struct:
                    return ParamStruct2Node(param as ParamStruct);
                case ParamType.list:
                    return ParamArray2Node(param as ParamList);
                default:
                    return ParamValue2Node(param as ParamValue);
            }
        }

        static XmlNode ParamStruct2Node(ParamStruct structure)
        {
            XmlNode xmlNode = xml.CreateElement(ParamType.@struct.ToString());
            foreach (var node in structure.Nodes)
            {
                XmlNode childNode = Param2Node(node.Value);
                XmlAttribute attr = xml.CreateAttribute("hash");
                attr.Value = Hash40Util.FormatToString(node.Key, hashToStringLabels);
                childNode.Attributes.Append(attr);
                xmlNode.AppendChild(childNode);
            }
            return xmlNode;
        }

        static XmlNode ParamArray2Node(ParamList array)
        {
            XmlNode xmlNode = xml.CreateElement(ParamType.list.ToString());
            XmlAttribute mainAttr = xml.CreateAttribute("size");
            mainAttr.Value = array.Nodes.Count.ToString();
            xmlNode.Attributes.Append(mainAttr);
            for (int i = 0; i < array.Nodes.Count; i++)
            {
                XmlNode childNode = Param2Node(array.Nodes[i]);
                XmlAttribute attr = xml.CreateAttribute("index");
                attr.Value = i.ToString();
                childNode.Attributes.Append(attr);
                xmlNode.AppendChild(childNode);
            }
            return xmlNode;
        }

        static XmlNode ParamValue2Node(ParamValue value)
        {
            XmlNode xmlNode = xml.CreateElement(value.TypeKey.ToString());
            XmlText text = xml.CreateTextNode(value.ToString(hashToStringLabels));
            xmlNode.AppendChild(text);
            return xmlNode;
        }

        static IParam Node2Param(XmlNode node)
        {
            try
            {
                if (!Enum.IsDefined(typeof(ParamType), node.Name)) 
                {
                    //throw new FormatException($"\"{node.Name}\" is not a valid param type");
                }
                ParamType type = (ParamType)Enum.Parse(typeof(ParamType), node.Name);
                switch (type)
                {
                    case ParamType.@struct:
                        return Node2ParamStruct(node);
                    case ParamType.list:
                        return Node2ParamArray(node);
                    default:
                        return Node2ParamValue(node, type);
                }
            }
            catch (Exception e)
            {
                //recursively add param node context to exceptions until we exit
                string trace = "Trace: " + node.Name;
                foreach (XmlAttribute attr in node.Attributes)
                    trace += $" ({attr.Name}=\"{attr.Value}\")";
                string message = trace + Environment.NewLine + e.Message;
                if (e.InnerException == null)
                    throw new Exception(message, e);
                else
                    throw new Exception(message, e.InnerException);
            }
        }

        static ParamStruct Node2ParamStruct(XmlNode node)
        {
            Hash40Pairs<IParam> childParams = new Hash40Pairs<IParam>();
            foreach (XmlNode child in node.ChildNodes)
                childParams.Add(child.Attributes["hash"].Value, stringToHashLabels, Node2Param(child));
            return new ParamStruct(childParams);
        }

        static ParamList Node2ParamArray(XmlNode node)
        {
            int count = node.ChildNodes.Count;
            List<IParam> children = new List<IParam>(count);
            for (int i = 0; i < count; i++)
                children.Add(Node2Param(node.ChildNodes[i]));
            return new ParamList(children);
        }

        static ParamValue Node2ParamValue(XmlNode node, ParamType type)
        {
            ParamValue param = new ParamValue(type);
            param.SetValue(node.InnerText, stringToHashLabels);
            return param;
        }

        enum BuildMode
        {
            Disassemble,
            Assemble,
            Invalid
        }
        #endregion
    }
}
