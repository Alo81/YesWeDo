﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YesweDo.Helpers;
using YesWeDo.DataTableCollections;
using YesWeDo.DataTables;
using YesWeDo.Helpers;
using static System.Windows.Forms.TabControl;
using static YesweDo.Enums;
using static YesWeDo.DataTables.DataTbl;

namespace YesweDo
{
    public partial class DataTbls
    {
        public List<Fighter> selectedFighters;
        public Battle selectedBattle { get; set; }
        public Spirit selectedSpirit { get; set; }
        public Stack<TabPage> tabStorage;

        public Config config;

        public TabControl tabs;
        public ProgressBar progress;
        public Label informativeLabel;

        public Form _editSpiritDetailsForm;
        public Form editSpiritDetailsForm
        {
            get 
            {
                if(_editSpiritDetailsForm == null)
                {
                    _editSpiritDetailsForm = UiHelper.GetEmptySpiritDataForm();
                    var tabs = UiHelper.GetEmptyTabControl();
                    _editSpiritDetailsForm.Controls.Add(tabs);

                    foreach (var subPage in UiHelper.GetPagesAsList(EmptySpiritPage, inclusive: false))
                    {
                        subPage.AutoScroll = true;
                        tabs.TabPages.Add(subPage);
                    }
                }
                return _editSpiritDetailsForm;
            }
            set { _editSpiritDetailsForm = value; }
        }

        public TabPageCollection editSpiritDetailsPages
        {
            get
            {
                return editSpiritDetailsForm.Controls.OfType<TabControl>().FirstOrDefault()?.TabPages;
            }
        }

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
        public SpiritBoardDataOptions spiritBoardData
        {
            get { return (SpiritBoardDataOptions)GetOptionsOfType(typeof(SpiritBoardDataOptions)); }
            set { UpdateDataOptions(value); }
        }
        public SpiritDataOptions spiritData
        {
            get { return (SpiritDataOptions)GetOptionsOfType(typeof(SpiritDataOptions)); }
            set { UpdateDataOptions(value); }
        }
        public SpiritLayoutDataOptions spiritLayoutData
        {
            get { return (SpiritLayoutDataOptions)GetOptionsOfType(typeof(SpiritLayoutDataOptions)); }
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
        public TabPage EmptySpiritPage
        {
            get
            {
                return Spirit.BuildEmptyPage(this);
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
                NetworkHelper.DownloadParamLabels(config.labels_file_location);
            }

            var results = XmlHelper.ReadXML(config.file_location, config.labels_file_location, config.labels_file_user_location);

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
            FileHelper.Save(battleData, fighterData, Path.GetDirectoryName(fileLocation), Path.GetFileName(fileLocation), spiritData : spiritData, spiritBoardData: spiritBoardData);
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
        public void SaveSpiritDb(Spirit spirit)
        {
            var spiritDbPages = editSpiritDetailsPages;
            // No Spirits?
            if (spiritDbPages is null)
            {
                return;
            }

            foreach (TabPage subPage in spiritDbPages)
            {
                // No Spirits?
                if (subPage is null)
                {
                    return;
                }
                spirit.UpdateTblValues(subPage);
            }
            int index = spiritData.GetItemIndex(spirit);
            if (index >= 0)
            {
                spiritData.ReplaceItemAtIndex(spirit, index);
            }
        }
        #endregion
        public void SetRemoveFighterButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(RemoveFighterFromButtonNamedIndex);
        }

        public void SetEditSpiritDetailsButtonMethod(ref Button b)
        {
            b.Click += new System.EventHandler(EditSpiritDetails);
        }
        public void SetDlcBoardBoxMethod(ref ComboBox b)
        {
            b.SelectedValueChanged += new System.EventHandler(EditSpiritDetails);
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

        public void EditSpiritDetails(object sender, EventArgs e)
        {
            try
            {
                selectedSpirit = spiritData.GetSpiritByName(selectedBattle.battle_id);
                if(selectedSpirit != null)
                {
                    var spiritDetailsPage = editSpiritDetailsForm;

                    var tabControl = spiritDetailsPage.Controls.OfType<TabControl>().FirstOrDefault();

                    foreach (var subPage in UiHelper.GetPagesFromTabControl(tabControl))
                    {
                        selectedSpirit.UpdatePageValues(subPage);
                    }

                    spiritDetailsPage.ShowDialog();
                    SaveSpiritDb(selectedSpirit);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch(Exception ex)
            {
                UiHelper.PopUpMessage("Spirit does not have Spirit Db Entry.");
            }
        }
        public void ChangeSpiritBoard(object sender, EventArgs e)
        {
            try
            {
                var box = (ComboBox)sender;
                if (spiritBoardData.HasData())
                {
                    var selectedBoard = box.SelectedValue.ToString();

                    spiritBoardData.SetSpiritToBoard(selectedBoard, selectedBattle.battle_id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage("Spirit does not have Spirit Db Entry.");
            }
        }

        public void LoadSpiritImageFromButton(object sender, EventArgs e)
        {
            var index = UiHelper.GetSpiritButtonIndexFromButton((Button)sender);

            var openDialog = new OpenFileDialog() { Title = $"Import Spirit Image {index}.", Filter = "BNTX|*.bntx*"};
            var result = openDialog.ShowDialog();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                FileHelper.LoadSpiritImageWithName(openDialog.FileName, Defs.spiritUiLocations[(int)index].Item1, selectedBattle.battle_id, selectedSpirit?.is_dlc ?? false);
                UiHelper.SetInformativeLabel(ref informativeLabel, $"Spirit Image {index} imported.");
            }
        }

        public void ImportBattle(BattleDataOptions battles, FighterDataOptions fighters)
        {
            battleData.ReplaceBattles(battles);
            fighterData.ReplaceFighters(fighters);
        }

        public void ImportBattle(DataOptions tbls)
        {
            battleData.ReplaceBattles((BattleDataOptions)tbls.GetDataOptionsFromUnderlyingType(typeof(Battle)));
            fighterData.ReplaceFighters((FighterDataOptions)tbls.GetDataOptionsFromUnderlyingType(typeof(Fighter)));
            spiritData.ReplaceSpirits((SpiritDataOptions)tbls.GetDataOptionsFromUnderlyingType(typeof(Spirit)));
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
                        // If first page of Battle.  
                        if (subPage.TabIndex == (int)Battle_Page.Basics)
                        {
                            if (spiritData.GetSpiritByName(selectedBattle.battle_id) == null)   //If there is no spirit data, disable the button.  
                                UiHelper.SetSpiritDetailsButton(subPage, false);
                            else
                                UiHelper.SetSpiritDetailsButton(subPage, true);
                            var matchedBoard = spiritBoardData.GetBoardOfSpirit(selectedBattle.battle_id);

                            // Explicit false to disable this for now.  
                            if (!String.IsNullOrWhiteSpace(matchedBoard) && false)
                            {
                                UiHelper.SetValueOfDLCBox(subPage, matchedBoard);
                            }
                        }
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
                                UiHelper.SetFighterButton(subPage, false);
                            else
                                UiHelper.SetFighterButton(subPage, true);
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
        public void SetSelecteds(string battle_id)
        {
            SetSelectedBattle(battle_id);
            SetSelectedFighters(battle_id);
            SetSelectedSpirit(battle_id);
        }

        public void SetSelectedBattle(string battle_id)
        {
            selectedBattle = battleData.GetBattle(battle_id);
        }
        public void SetSelectedFighters(string battle_id)
        {
            selectedFighters = fighterData.GetFightersByBattleId(battle_id);
        }
        public void SetSelectedSpirit(string battle_id)
        {
            selectedSpirit = spiritData.GetSpiritByName(battle_id);
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
                if (type == typeof(Spirit))
                {
                    return spiritData.GetOptionsFromName(name) ?? new List<string>();
                }
                if (type == typeof(SpiritBoard))
                {
                    return spiritBoardData.GetOptionsFromName(name) ?? new List<string>();
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
            var form = UiHelper.GetRandomizerProgressWindow(iteration);
            var controlPoint = new Point();

            var bar = UiHelper.GetRandomizerProgressBar(ref controlPoint, battleData.GetCount());

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
