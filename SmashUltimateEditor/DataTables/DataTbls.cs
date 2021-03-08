using paracobNET;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db;
using SmashUltimateEditor.DataTables.ui_item_db;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static SmashUltimateEditor.DataTables.DataTbl;
using static SmashUltimateEditor.Enums;
using static System.Windows.Forms.TabControl;

namespace SmashUltimateEditor
{
    public partial class DataTbls
    {
        public List<Fighter> selectedFighters;
        public Battle selectedBattle;
        public Stack<TabPage> tabStorage;

        public Config config;

        public TabControl tabs;
        public ProgressBar progress;
        public Label informativeLabel;

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
            return GetOptionsOfType(dataOptions, type);
        }
        public static IDataOptions GetOptionsOfType(List<IDataOptions> options, Type type)
        {
            return options.First(x => x.GetType() == type);
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

        public int pageCount { get { return selectedFighters.Sum(x => x.pageCount) + selectedBattle.pageCount; } }
        public int tabCount { get { return tabs.TabPages.Count; } }
        public int tabStorageCount { get { return tabStorage.Count; } }
        public int totalTabCount { get { return tabCount + tabStorageCount; } }
        public bool HasEnoughPages { get { return tabCount >= pageCount; } }

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

        public DataTbls()
        {
            dataOptions = new List<IDataOptions>();
            selectedFighters = new List<Fighter>();
            selectedBattle = new Battle();
            config = new Config();
            tabStorage = new Stack<TabPage>();

            // If there is no labels file, ask the user if they would like one.  
            if (!FileHelper.FileExists(config.labels_file_location) && UiHelper.PopUpQuestion("No Labels file found.  Would you like to download now?"))
            {
                FileHelper.DownloadParamLabels(config.labels_file_location);
            }

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
                var page = tabStorage.Pop();
                var pages = UiHelper.GetPagesAsList(page);
                for (int j = 0; j < pages.Count; j++)
                {
                    var subPage = pages[j];
                    if (subPage != null)
                        SetEventTypeForPage(ref subPage);
                }
                tabStorage.Push(page);
            }
        }

        #region Saving
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
        #endregion
        public void SetRemoveFighterButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(RemoveFighterFromButtonNamedIndex);
        }

        public void SetLoadSpiritImageButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(LoadSpiritImageFromButton);
        }
        public void RemoveFighterFromButtonNamedIndex(object sender, EventArgs e)
        {
            int index = Int32.Parse(((Button)sender).Name);
            selectedFighters.Remove(fighterData.GetFighterAtIndex(index));
            fighterData.RemoveFighterAtIndex(index);
            RefreshTabs();
        }

        public void LoadSpiritImageFromButton(object sender, EventArgs e)
        {
            var index = UiHelper.GetSpiritButtonIndexFromButton((Button)sender);

            var openDialog = new OpenFileDialog() { Title = $"Import Spirit Image {index}.", Filter = "BNTX|*.bntx*"};
            var result = openDialog.ShowDialog();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                var source = openDialog.FileName;
                var fileDetails = Defs.spiritUiLocations[(int)index];
                var name = fileDetails.Item1.Replace(Defs.FILE_WILDCARD_PATTERN, selectedBattle.battle_id);
                var dest = config.file_directory_spirit_images;

                FileHelper.CopyFile(source, dest, name);
                UiHelper.SetInformativeLabel(ref informativeLabel, $"Spirit Image {index} imported.");
            }
        }

        public void ImportBattle(BattleDataOptions battles, FighterDataOptions fighters)
        {
            battleData.ReplaceBattles(battles);
            fighterData.ReplaceFighters(fighters);
        }

        public void RefreshTabs()
        {
            PopulateTabs();
            BuildEmptyTabs();

            foreach(TabPage page in UiHelper.GetPagesFromTabControl(tabs))
            {
                // Battle
                if (page.TabIndex == (int)Top_Level_Page.Battle)
                {
                    UiHelper.SetPageName(page, selectedBattle.battle_id, battleData.GetBattleIndex(selectedBattle));

                    foreach (var subPage in UiHelper.GetPagesAsList(page, inclusive : false))
                    {
                        UpdateBattlePageValues(subPage);
                    }
                }
                else if(page.TabIndex >= (int)Top_Level_Page.Fighters)
                {
                    var pageIndex = page.TabIndex - 1;
                    UiHelper.SetPageName(page, selectedFighters[pageIndex].fighter_kind, fighterData.GetFighterIndex(selectedFighters[pageIndex]));


                    foreach (var subPage in UiHelper.GetPagesAsList(page, inclusive: false))
                    {
                        UpdateFighterPageValues(subPage, pageIndex);

                        // If first fighter, and first page.  
                        if(pageIndex == 0 && subPage.TabIndex == (int)Fighter_Page.Attributes)
                        {
                            if (selectedFighters.Count == 1)
                                UiHelper.DisableFighterButton(subPage);
                            else
                                UiHelper.EnableFighterButton(subPage);
                        }
                    }
                }
                else
                {

                }
            }
        }
        public void BuildEmptyTabs()
        {
            while (!HasEnoughPages)
            {
                if (tabCount == 0)
                {
                    tabs.TabPages.Add(EmptyBattlePage);
                }
                else
                {
                    tabs.TabPages.Add(EmptyFighterPage);
                }
            }
        }

        public void HideTabs()
        {
            while(pageCount < tabCount)
            {
                tabStorage.Push(tabs.TabPages[tabCount-1]);
                tabs.TabPages.RemoveAt(tabCount-1);
            }
        }

        public void ShowTabs()
        {
            while (tabStorage.Count > 0 && !HasEnoughPages)
            {
                tabs.TabPages.Add(tabStorage.Pop());
            }
        }

        public void PopulateTabs()
        {
            if (tabs is null)
            {
                tabs = new TabControl();
            }
            HideTabs();
            ShowTabs();
        }

        public void UpdateBattlePageValues(TabPage page)
        {
            selectedBattle.UpdatePageValues(page);
        }
        
        public void UpdateFighterPageValues(TabPage page, int index)
        {
            var collectionIndex = fighterData.GetFighterIndex(selectedFighters[index]);
            selectedFighters[index].UpdateFighterPageValues(page, collectionIndex);
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

        public void SetEventLabelOptions(ComboBox combo, TabPage page)
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
            if (type?.GetProperty(name)?.GetCustomAttribute<ExcludedAttribute>()?.Excluded ?? false)    // If this field is excluded, don't build out for it.  
            {
            }
            else
            {
                if (type == typeof(Fighter))
                {
                    return fighterData.GetOptionsFromName(name) ?? new List<string>();
                }
                if (type == typeof(Battle))
                {
                    return battleData.GetOptionsFromName(name) ?? new List<string>();
                }
            }
            return new List<string>();
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

        #region Randomizer
        public async void ExecuteRandomizer(int iteration, int seed, int characterUnlockSeed)
        {
            Form form = new Form()
            {
                Text = $"Randomizer {iteration} progress.",
                Size = new Size(300, 100),
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, 100*(iteration))
            };

            var controlPoint = new Point();

            var bar = new ProgressBar();
            bar.Size = new Size(Defs.LABEL_WIDTH, Defs.LABEL_HEIGHT);
            bar.Location = UiHelper.IncrementPoint(ref controlPoint, 0, Ui_Element.Label);
            UiHelper.SetupRandomizeProgress(bar, battleData.GetCount());

            form.Controls.Add(bar);
            form.Show();

            var randomizedSpirits = await RandomizeAll(bar, seed, characterUnlockSeed, iteration);

            var randomizedBattleData = (BattleDataOptions)GetOptionsOfType(randomizedSpirits, typeof(BattleDataOptions));
            var randomizedFighterData = (FighterDataOptions)GetOptionsOfType(randomizedSpirits, typeof(FighterDataOptions));

            FileHelper.SaveRandomized(randomizedBattleData, randomizedFighterData, characterUnlockSeed, iteration);

            form.Hide();
        }

        public async Task<List<IDataOptions>> RandomizeAll(ProgressBar progressBar, int seed, int characterUnlockSeed, int iteration = 0)
        {
            Random rnd = new Random(seed);
            Random unlockableRnd;


            BattleDataOptions randomizedBattleData;
            FighterDataOptions randomizedFighters;
            Fighter randomizedFighter;
            int fighterCount;

            unlockableRnd = new Random(characterUnlockSeed);
            randomizedFighters = new FighterDataOptions();
            randomizedBattleData = this.battleData.Copy();
            List<string> unlockableFighters = this.spiritFighterData.unlockable_fighters_string;

            foreach (Battle battle in randomizedBattleData.GetBattles())
            {
                battle.Randomize(ref rnd, this);
                // If lose escort, need at least 2 fighters.  
                fighterCount = battle.IsLoseEscort() ?
                    RandomizerHelper.fighterLoseEscortDistribution[rnd.Next(RandomizerHelper.fighterLoseEscortDistribution.Count)]
                    :
                    RandomizerHelper.fighterDistribution[rnd.Next(RandomizerHelper.fighterDistribution.Count)];

                // Save after randomizing, as cleanup will modify it.  
                var isUnlockableFighterType = this.spiritFighterData.IsUnlockableFighter(battle.battle_id);
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

                    randomizedFighter.Cleanup(ref rnd, isMain, isEscort, this.fighterData.Fighters, isBoss, unlockableFighter);
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

                progressBar.PerformStep();
            }

            return new List<IDataOptions>() { randomizedBattleData, randomizedFighters };
        }
        #endregion
    }
}
