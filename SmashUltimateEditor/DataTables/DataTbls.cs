using paracobNET;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db;
using SmashUltimateEditor.DataTables.ui_item_db;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using static SmashUltimateEditor.Enums;
using static System.Windows.Forms.TabControl;

namespace SmashUltimateEditor
{
    public partial class DataTbls
    {
        public List<IDataOptions> dataOptions;

        public BattleDataOptions battleData
        {
            get { return (BattleDataOptions)GetOptionsOfType(typeof(BattleDataOptions)); }
            set { UpdateDataOptions(value); }
        }
        public FighterDataOptions fighterData
        {
            get { return (FighterDataOptions)GetOptionsOfType(typeof(FighterDataOptions)); }
            set { UpdateDataOptions(value); }
        }
        public EventDataOptions eventData
        {
            get { return (EventDataOptions)GetOptionsOfType(typeof(EventDataOptions)); }
            set { UpdateDataOptions(value); }
        }
        public ItemDataOptions itemData
        {
            get { return (ItemDataOptions)GetOptionsOfType(typeof(ItemDataOptions)); }
            set { UpdateDataOptions(value); }
        }
        public SpiritFighterDataOptions spiritFighterData
        {
            get { return (SpiritFighterDataOptions)GetOptionsOfType(typeof(SpiritFighterDataOptions)); }
            set { UpdateDataOptions(value); }
        }

        public IDataOptions GetOptionsOfType(Type type)
        {
            return dataOptions.Where(x => x.GetType() == type).First();
        }

        public void AddDataOptions(IDataOptions options)
        {
            dataOptions.Add(options);
        }
        public IDataOptions GetDataOptions(IDataOptions options)
        {
            return GetOptionsOfType(options.GetType());
        }
        public void RemoveDataOptions(IDataOptions options)
        {
            IDataOptions oldOptions = GetOptionsOfType(options.GetType());
            dataOptions.Remove(oldOptions);
        }
        public void UpdateDataOptions(IDataOptions options)
        {
            RemoveDataOptions(options);
            dataOptions.Add(options);
        }

        public List<Fighter> selectedFighters;
        public Battle selectedBattle;
        public Queue<TabPage> tabStorage = new Queue<TabPage>();

        public Config config;

        public TabControl tabs;
        public ProgressBar progress;

        public int pageCount { get { return selectedFighters.Sum(x => x.pageCount) + selectedBattle.pageCount; } }
        public int tabCount { get { return tabs.TabPages.Count; } }
        public TabPage EmptyBattlePage
        {
            get
            {
                return Battle.BuildEmptyPage(this);
            } 
        }

        public TabPage EmptyFighterPage
        {
            get
            {
                return Fighter.BuildEmptyPage(this);
            }
        }

        public bool HasEnoughPages { get { return tabs.TabPages.Count >= pageCount; } }

        public DataTbls()
        {
            dataOptions = new List<IDataOptions>();
            selectedFighters = new List<Fighter>();
            selectedBattle = new Battle();
            config = new Config();

            var results = XmlHelper.ReadXML(config.file_location, config.labels_file_location);

            foreach(Type child in DataTbl.GetChildrenTypes())
            {
                IDataOptions opt = results.GetDataOptionsFromUnderlyingType(child);

                if(opt != null)
                {
                    dataOptions.Add(opt);
                }
            }
        }

        public void EmptySpiritData()
        {
            var options = dataOptions.OfType<BattleDataOptions>();

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
                var pages = UiHelper.GetPagesAsList(page);
                for(int j = 0; j < pages.Count; j++)
                {
                    var subPage = pages[j];
                    if(subPage != null)
                        SetEventTypeForPage(ref subPage);
                }
            }

            // Stored pages.
            for (int i = 0; i < tabStorage.Count; i++)
            {
                var page = tabStorage.Dequeue();
                var pages = UiHelper.GetPagesAsList(page);
                for (int j = 0; j < pages.Count; j++)
                {
                    var subPage = pages[j];
                    if (subPage != null)
                        SetEventTypeForPage(ref subPage);
                }
                tabStorage.Enqueue(page);
            }
        }
        public void Save()
        {
            Save(config.file_location);
        }

        public void Save(string fileLocation)
        {
            SaveLocal();    // Save immediately before sending battle and fighter data.  
            FileHelper.Save(battleData, fighterData, Path.GetDirectoryName(fileLocation), Path.GetFileName(fileLocation));
        }

        public void SetSaveTabChange(object sender, EventArgs e)
        {
            SaveLocal();
        }
        public void SaveLocal()
        {
            SaveBattle();
            SaveFighters();
        }
        public void SaveBattle()
        {
            TabPage battlePage = tabs.TabPages.Count > 0 ? tabs.TabPages[0] : null;
            // No battle?
            if (battlePage is null)
            {
                return;
            }
            // Update fields on page, then fields on subpages. 
            selectedBattle.UpdateTblValues(battlePage);
            foreach (TabPage subPage in battlePage.Controls.OfType<TabControl>().First().TabPages)
            {
                // No battle?
                if (subPage is null)
                {
                    return;
                }
                selectedBattle.UpdateTblValues(subPage);
            }
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

            for (int i = 0; i < fighterPages.Count - 1; i++)
            {
                // Match on tab index, which is assigned to Fighter when it updates page values.  
                // Update fields on page, then fields on subpages. 
                selectedFighters[i].UpdateTblValues(fighterPages[i + 1]);

                foreach (TabPage subPage in fighterPages[i + 1].Controls.OfType<TabControl>().First().TabPages)
                {
                    subPage.Select();
                    selectedFighters[i].UpdateTblValues(subPage);
                }
                int index = fighterData.GetFighterIndex(selectedFighters[i]);
                if (index >= 0)
                {
                    fighterData.ReplaceFighterAtIndex(index, selectedFighters[i]);
                }
            }
        }
        public void SetRemoveFighterButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(RemoveFighterFromButtonNamedIndex);
        }
        public void RemoveFighterFromButtonNamedIndex(object sender, EventArgs e)
        {
            int index = Int32.Parse(((Button)sender).Name);
            selectedFighters.Remove(fighterData.GetFighterAtIndex(index));
            fighterData.RemoveFighterAtIndex(index);
            RefreshTabs();
        }

        public void ImportBattle(BattleDataOptions battles, FighterDataOptions fighters)
        {
            battleData.ReplaceBattles(battles);
            fighterData.ReplaceFighters(fighters);
        }

        public void RandomizeAll(int seed = -1)
        {
            Random rnd = new Random(seed);
            Random unlockableRnd;

            BattleDataOptions randomizedBattleData;
            FighterDataOptions randomizedFighters;
            Fighter randomizedFighter;
            int fighterCount;

            for (int iteration = 0; iteration < config.randomizer_iterations; iteration++)
            {
                unlockableRnd = new Random(seed);
                randomizedFighters = new FighterDataOptions();
                randomizedBattleData = battleData.Copy();
                List<string> unlockableFighters = spiritFighterData.unlockable_fighters_string;

                UiHelper.SetupRandomizeProgress(ref progress, randomizedBattleData.GetCount());

                foreach (Battle battle in randomizedBattleData.GetBattles())
                {
                    battle.Randomize(ref rnd, this);
                    // If lose escort, need at least 2 fighters.  
                    fighterCount = battle.IsLoseEscort() ?
                        RandomizerHelper.fighterLoseEscortDistribution[rnd.Next(RandomizerHelper.fighterLoseEscortDistribution.Count)]
                        :
                        RandomizerHelper.fighterDistribution[rnd.Next(RandomizerHelper.fighterDistribution.Count)];

                    // Save after randomizing, as cleanup will modify it.  
                    var isUnlockableFighterType = spiritFighterData.IsUnlockableFighter(battle.battle_id);
                    var isBossType = battle.IsBossType();
                    var isLoseEscort = battle.IsLoseEscort();

                    battle.Cleanup(ref rnd, fighterCount, this, isUnlockableFighterType);

                    var fighterSum = 0;
                    for (int i = 0; i < fighterCount; i++)
                    {
                        string unlockableFighter = null;

                        var isMain = i == 0;     // Set first fighter to main.  
                        var isUnlockableFighter = isMain && isUnlockableFighterType;     // If unlockable fighter, we will explicitly set a fighter to match the spirit.  
                        var isBoss = isMain && isBossType;     // Set first fighter to main.  
                        var isEscort = i == 1 && battle.IsLoseEscort();     // Set second fighter to ally, if Lose Escort result type.  

                        randomizedFighter = battle.GetNewFighter();
                        randomizedFighter.Randomize(ref rnd, this);

                        if (isUnlockableFighter)
                        {
                            int fighterIndex = unlockableRnd.Next(unlockableFighters.Count);
                            unlockableFighter = unlockableFighters[fighterIndex];
                            unlockableFighters.RemoveAt(fighterIndex);
                        }

                        randomizedFighter.Cleanup(ref rnd, isMain, isEscort, fighterData.Fighters, isBoss, unlockableFighter);
                        fighterSum += randomizedFighter.stock == 0 ? 1 : randomizedFighter.stock;

                        randomizedFighters.AddFighter(randomizedFighter);
                    }
                    // Do a total fighter check, and adjust stock accordingly. 
                    if (fighterSum > Defs.FIGHTER_COUNT_STOCK_CUTOFF)
                    {
                        for (int i = randomizedFighters.GetCount() - fighterCount; i < randomizedFighters.GetCount(); i++)
                        {
                            randomizedFighters.GetFighterAtIndex(i).StockCheck(fighterSum);
                        }
                    }

                    progress.PerformStep();
                }
                FileHelper.SaveRandomized(randomizedBattleData, randomizedFighters, seed, iteration);
            }
            RefreshTabs();
            progress.Visible = false;
            MessageBox.Show(String.Format("Spirit Battles Randomized {0} times.\r\nChaos: {1}. \r\nSeed: {2}\r\nLocation: {3}", config.randomizer_iterations, config.chaos, seed, config.file_directory_randomized));
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
                    UiHelper.SetPageName(ref page, selectedBattle.battle_id, battleData.GetBattleIndex(selectedBattle));

                    TabPageCollection subPages = page.Controls.OfType<TabControl>().First().TabPages;
                    for (int j = 0; j < subPages.Count; j++)
                    {
                        var subPage = subPages[j];
                        UpdateBattlePageValues(ref subPage, j);
                        subPages[j] = subPage;
                    }
                }
                // Fighters
                else
                {
                    UiHelper.SetPageName(ref page, selectedFighters[i-1].fighter_kind, fighterData.GetFighterIndex(selectedFighters[i-1]));
                    TabPageCollection subPages = page.Controls.OfType<TabControl>().First().TabPages;
                    for (int j = 0; j < subPages.Count; j++)
                    {
                        var subPage = subPages[j];
                        if (i == 1 && j == 0)   //i is first character, j is first tab.  
                        {
                            if (selectedFighters.Count == 1)
                                UiHelper.DisableFighterButton(ref subPage);
                            else
                                UiHelper.EnableFighterButton(ref subPage);
                        }
                        UpdateFighterPageValues(ref subPage, i);
                        subPages[j] = subPage;
                    }
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
                page = EmptyEventPage;
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
            while (tabStorage.Count > 0 && !HasEnoughPages)
            {
                tabs.TabPages.Add(tabStorage.Dequeue());
            }
        }

        public void UpdateBattlePageValues(ref TabPage page, int i = 0)
        {
            var collectionIndex = battleData.GetBattleIndex(selectedBattle);
            selectedBattle.UpdatePageValues(ref page, i, collectionIndex);
        }
        
        public void UpdateFighterPageValues(ref TabPage page, int i)
        {
            var collectionIndex = fighterData.GetFighterIndex(selectedFighters[i - 1]);
            selectedFighters[i - 1].UpdatePageValues(ref page, i, collectionIndex);
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
            foreach(ComboBox control in page.Controls.OfType<ComboBox>().Where(x => Regex.IsMatch(x.Name, "event.*type")))
            {
                control.SelectedIndexChanged += SetEventLabelOptions;
            }
        }

        public void SetEventLabelOptions(object sender, EventArgs e = null)
        {
            if (eventData.GetCount() == 0)
            {
                return;
            }

            var combo = ((ComboBox)sender);

            // Get events page.  
            foreach(TabPage page in UiHelper.GetAllPagesFromTabControl(tabs).Where(x => x.Name == Enums.Battle_Page.Events.ToString()))
            {
                eventData.SetEventLabelOptions(combo, page);
            }
        }
        public void SetMiiFighterSpecials(object sender, EventArgs e = null)
        {
            var combo = ((ComboBox)sender);

            if (!Defs.miiFighterMod.ContainsKey((string)combo.SelectedValue))
            {
                return;
            }

            // For fighters, we care about unique instances, cuz there can be more than one.  How do we handle?  
            var allTabs = UiHelper.GetPagesFromTabControl(tabs);

            for (int i = 0; i < allTabs.Count; i++)
            {
                var page = allTabs[i];
                if (page.Name == Enums.Top_Level_Page.Fighters.ToString())
                {
                    var subPages = UiHelper.GetAllPagesFromTabControl(tabs);
                    for (int j = 0; j < subPages.Count; j++)
                    {
                        var subPage = subPages[j];
                        if (subPage?.Name == Enums.Fighter_Page.Mii.ToString())
                        {
                            // Get current fighter_kind
                            //Fighter.SetMiiFighterSpecials(combo, ref subPage);
                            return;
                        }
                    }
                }
            }
        }

        public void SetEventLabelOptions(ComboBox combo, ref TabPage page)
        {
            eventData.SetEventLabelOptions(combo, page);
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
            if (type == typeof(Fighter))
            {
                return fighterData.GetOptionsFromName(name) ?? new List<string>();
            }
            if (type == typeof(Battle))
            {
                return battleData.GetOptionsFromName(name) ?? new List<string>();
            }
            return null;
        }

        public string GetModeFromTypeAndName(Type type, string name)
        {
            var options = new List<string>();

            if (type == typeof(Fighter))
            {
                options = fighterData.GetOptionsFromName(name) ?? new List<string>();
            }
            if (type == typeof(Battle))
            {
                options = battleData.GetOptionsFromName(name) ?? new List<string>();
            }

            var groups = options.GroupBy(v => v);
            int maxCount = groups.Max(g => g.Count());
            string mode = groups.First(g => g.Count() == maxCount).Key;

            return mode;
        }
    }
}
