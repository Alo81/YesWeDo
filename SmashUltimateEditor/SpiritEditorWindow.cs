using Microsoft.WindowsAPICodePack.Dialogs;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db;
using SmashUltimateEditor.DataTables.ui_item_db;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YesWeDo.DataTableCollections;
using YesWeDo.DataTables.ui_spirit_db;
using YesWeDo.Helpers;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public partial class SpiritEditorWindow : Form
    {
        public SpiritEditorWindow()
        {
            InitializeComponent();

            //this.
            // this.DataContext = // Must be a window for this to work(?)  Essentially for us to data bind.  Have an intermediary object that has all the data to be maniuplated.  
            // Convert to getting all objects from Factorys?
            // Genericize UI from actual logic with a ViewModel
            // Implement unit tests.  

            dataTbls = new DataTbls();
            dataTbls.tabs = tabControlData;
            //dataTbls.tabs.TabIndexChanged += new System.EventHandler(dataTbls.SetSaveTabChange);
            dataTbls.progress = randomizeProgress;
            dataTbls.informativeLabel = labelInformative;

            FileHelper.CreateDirectories(dataTbls.config.GetFileDirectories());
            LoadAllFiles();

            if (dataTbls.battleData.HasData())
            {
                buildFighterDataTab((string)dataTbls?.battleData?.GetPropertyValuesFromName("battle_id")?.First());
            }
            else
            {
                UiHelper.PopUpMessage($"Be sure to add a SpiritBattle DB (ui_spirits_battle_db.prc) to application directory\r\n" +
                    $"And\r\n" +
                    $"Download ParamLabels.csv from \"Tools\" dropdown.\r\n" +
                    $"Then restart application.");
            }

            if (dataTbls.config.check_for_updates)
            {
                NetworkHelper.UpdateCheck();
            }
        }

        private void buildFighterDataTab(string battle_id)
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.GetPropertyValuesFromName("battle_id").ToList();
            dataTbls.SetSelectedBattle(battle_id);
            dataTbls.SetSelectedFighters(battle_id);
        }

        private async void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            dataTbls.SetSelectedBattle((string)dropdownSpiritData.SelectedItem);
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);
            dataTbls.RefreshTabs();
        }

        private async void btnAddFighter_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();

            Fighter newFighter = dataTbls.selectedBattle.GetNewFighter();
            newFighter.SetAllValuesToDefault();

            dataTbls.fighterData.AddFighter(newFighter);
            dataTbls.selectedFighters.Add(newFighter);
            dataTbls.RefreshTabs();
        }
        private async void btnCopyFighter_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            int selectedTab = dataTbls.tabs.SelectedIndex;
            selectedTab += selectedTab > 0 ? -1 : 0;

            Fighter newFighter = dataTbls.selectedFighters[selectedTab].Copy();

            dataTbls.fighterData.AddFighter(newFighter);
            dataTbls.selectedFighters.Add(newFighter);
            dataTbls.RefreshTabs();
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            int index = dataTbls.tabs.SelectedIndex;
            var battle = dataTbls.selectedBattle;

            int seed = TryGetSeed();
            Random rnd = new Random(seed);
            if (index == 0)
            {
                battle.Randomize(ref rnd, dataTbls);
                battle.Cleanup(ref rnd, dataTbls.selectedFighters.Count, dataTbls);
            }
            else
            {
                index -= 1; // We're looking at fighters now, start at index 0.
                var fighter = dataTbls.selectedFighters[index];

                var isMain = index == 0;     // Set first fighter to main.  
                var isLoseEscort = index == 1 && battle.IsLoseEscort();     // Set second fighter to ally, if Lose Escort result type.  
                var isBoss = index == 0 && battle.IsBossType();     // Set second fighter to ally, if Lose Escort result type.  
                fighter.Randomize(ref rnd, dataTbls);
                fighter.Cleanup(ref rnd, isMain, isLoseEscort, dataTbls.fighterData.Fighters, isBoss);
            }
            dataTbls.RefreshTabs();
            UiHelper.SetInformativeLabel(ref labelInformative, $"Page Randomized.");
        }

        private void OpenDbFile_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory};
            var result = openDialog.ShowDialog();
            List<string> dbTypes;

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                dbTypes = OpenDbWithFileName(openDialog.FileName);
                dataTbls.RefreshTabs();
                var dbTypeCSV = UiHelper.ListToCSV(dbTypes);

                UiHelper.SetInformativeLabel(ref labelInformative, $"Opened {dbTypeCSV}");
            }
        }

        public List<string> OpenDbWithFileName(string fileName)
        {
            var fileDbType = new List<string>();
            try
            {
                var results = XmlHelper.ReadXML(fileName, dataTbls.config.labels_file_location);

                if (results.GetDataOptionsFromUnderlyingType(typeof(Battle)).GetCount() + results.GetDataOptionsFromUnderlyingType(typeof(Fighter)).GetCount() > 0)
                {
                    if (results.GetDataOptionsFromUnderlyingType(typeof(Battle)).GetCount() > 0)
                    {
                        dataTbls.battleData = (BattleDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Battle));
                        fileDbType.Add("Battle");
                    }
                    if (results.GetDataOptionsFromUnderlyingType(typeof(Fighter)).GetCount() > 0)
                    {
                        dataTbls.fighterData = (FighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Fighter));
                        fileDbType.Add("Fighter");
                    }
                    
                    buildFighterDataTab(dataTbls.battleData.GetBattleAtIndex(0).battle_id);
                }
                if (results.GetDataOptionsFromUnderlyingType(typeof(Event)).GetCount() > 0)
                {
                    dataTbls.eventData = (EventDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Event));
                    dataTbls.eventData.SetFoundEventTypes(dataTbls.battleData.event_type);
                    dataTbls.UpdateEventsForDbValues();

                    fileDbType.Add("Event");
                }
                if (results.GetDataOptionsFromUnderlyingType(typeof(Item)).GetCount() > 0)
                {
                    dataTbls.itemData = (ItemDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Item));
                    var itemEvents = dataTbls.itemData.GetAsEvents();
                    dataTbls.eventData.AddUniqueEvents(itemEvents);
                    fileDbType.Add("Item");
                }
                if (results.GetDataOptionsFromUnderlyingType(typeof(SpiritFighter)).GetCount() > 0)
                {
                    dataTbls.spiritFighterData = (SpiritFighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(SpiritFighter));
                    fileDbType.Add("Spirit Fighter");
                }
                if (results.GetDataOptionsFromUnderlyingType(typeof(SpiritBoard)).GetCount() > 0)
                {
                    dataTbls.spiritBoardData = (SpiritBoardDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(SpiritBoard));
                    fileDbType.Add("Spirit Board");
                }
                if (results.GetDataOptionsFromUnderlyingType(typeof(Spirit)).GetCount() > 0)
                {
                    dataTbls.spiritData = (SpiritDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Spirit));
                    fileDbType.Add("Spirit");
                }
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(String.Format("Couldn't read XML.  Is it encrypted?\r\n{0}", ex.Message));
            }

            return fileDbType;
        }

        public string OpenMsbtWithFileName(string fileName)
        {
            return FileHelper.OpenMsbtWithFilename(dataTbls.battleData.GetBattles(), fileName);
        }

        public void LoadAllFiles()
        {
            var config = dataTbls.config;
            var directory = config.file_directory_preload;
            string[] fileNames;
            try
            {
                fileNames = Directory.GetFiles(directory);
            }
            catch(Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return;
            }
            var dbTypes = new List<string>();
            string dbTypeCSV;

            foreach (string fileName in fileNames.Where(x => x.EndsWith(Defs.dbFileExtension)))
            {
                dbTypes.AddRange(OpenDbWithFileName(fileName));
            }
            foreach (string fileName in fileNames.Where(x => x.EndsWith(Defs.textFileExtension)))
            {
                dbTypes.Add(OpenMsbtWithFileName(fileName));
            }

            dbTypeCSV = UiHelper.ListToCSV(dbTypes);

            if (fileNames.Length > 0)
            {
                UiHelper.SetInformativeLabel(ref labelInformative, $"Loaded files: {dbTypeCSV}");
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            dataTbls.Save();
            UiHelper.SetInformativeLabel(ref labelInformative, $"File saved.");
        }

        private void SaveAsFile_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog() 
            { 
                Title = String.Format("Save Spirit Battles"), 
                Filter = "PRC|*.prc*", 
                FileName = dataTbls.config.file_name_encr, 
                InitialDirectory = dataTbls.config.file_directory 
            };

            var result = saveDialog.ShowDialog();
            if(!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(saveDialog?.FileName))
            {
                dataTbls.Save(saveDialog.FileName);
                UiHelper.SetInformativeLabel(ref labelInformative, $"File saved.");
            }
        }
        private void ExportModForRelease_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();

            var dialog = FileHelper.GetCommonSaveFolderDialog();

            foreach(var filter in EnumUtil<Export_Filters>.GetValues())
            {
                dialog.Filters.Add(new CommonFileDialogFilter(filter, "directory"));
            }

            dialog.Title = "Export Mod for Release.";
            dialog.InitialDirectory = dataTbls.config.file_directory_custom_battles;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && !String.IsNullOrWhiteSpace(dialog?.FileName))
            {
                var selectedFilter = dialog.Filters[dialog.SelectedFileTypeIndex-1];
                var standalone = selectedFilter.DisplayName.Contains(Export_Filters.Standalone.ToString());
                var packaged = selectedFilter.DisplayName.Contains(Export_Filters.Packaged.ToString());

                if (standalone)
                {
                    FileHelper.ExportStandalone(dataTbls, dialog.FileName);
                }

                // Save encrypted version for releasing straight to Switch.
                if (packaged)
                {
                    FileHelper.ExportPackaged(dataTbls, dialog.FileName);
                }
                UiHelper.SetInformativeLabel(ref labelInformative, "Export Complete.");
            }
        }

        private void ImportBattle_Click(object sender, EventArgs e)
        {
            var importDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory };
            var result = importDialog.ShowDialog();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(importDialog?.FileName))
            {
                var results = XmlHelper.ReadXML(importDialog.FileName, dataTbls.config.labels_file_location);

                BattleDataOptions battles = (BattleDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Battle));
                FighterDataOptions fighters = (FighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Fighter));

                dataTbls.ImportBattle(battles, fighters);

                var battle_id = battles.GetBattleAtIndex(0).battle_id;
                dropdownSpiritData.SelectedItem = battle_id;

                UiHelper.SetInformativeLabel(ref labelInformative, "Import Complete.");
            }
        }
        private void ImportBattleOverFile_Click(object sender, EventArgs e)
        {
            var importDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory };
            var result = importDialog.ShowDialog();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(importDialog?.FileName))
            {
                var results = XmlHelper.ReadXML(importDialog.FileName, dataTbls.config.labels_file_location);

                results.SetBattleIdsForAll(dataTbls.selectedBattle.battle_id);

                BattleDataOptions battles = (BattleDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Battle));
                FighterDataOptions fighters = (FighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Fighter));

                dataTbls.ImportBattle(battles, fighters);

                var battle_id = battles.GetBattleAtIndex(0).battle_id;
                dropdownSpiritData.SelectedItem = battle_id;

                UiHelper.SetInformativeLabel(ref labelInformative, "Import Complete.");
            }
        }

        private void ImportFolderFile_Click(object sender, EventArgs e)
        {
            var openDialog = new CommonOpenFileDialog() { Title = "Replace loaded battles with all battles in folder", InitialDirectory = dataTbls.config.file_directory_custom_battles, IsFolderPicker = true };
            try
            {
                if ((openDialog.ShowDialog() == CommonFileDialogResult.Ok) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
                {
                    var files = Directory.EnumerateFiles(openDialog.FileName).Where(x => x.Contains(".prc"));
                    foreach (string file in files)
                    {
                        var results = XmlHelper.ReadXML(file, dataTbls.config.labels_file_location);
                        BattleDataOptions battles = (BattleDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Battle));
                        FighterDataOptions fighters = (FighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Fighter));

                        dataTbls.battleData.ReplaceBattles(battles);
                        dataTbls.fighterData.ReplaceFighters(fighters);

                        var battle_id = battles.GetBattleAtIndex(0).battle_id;

                        if (dataTbls.selectedBattle.battle_id == battle_id)
                        {
                            dataTbls.SetSelectedBattle(battle_id);
                            dataTbls.SetSelectedFighters(battle_id);
                        }
                    }
                    var selected_battle_id = dataTbls.selectedBattle.battle_id;
                    dropdownSpiritData.SelectedItem = selected_battle_id;

                    dataTbls.RefreshTabs();

                    UiHelper.SetInformativeLabel(ref labelInformative, "Import Complete.");
                }
            }
            catch 
            { 
                UiHelper.PopUpMessage("Couldn't import battles due to error."); 
            };
        }

        private async void RandomizeAllTool_Click(object sender, EventArgs e)
        {
            int seed = TryGetSeed();
            var allTasks = new Task[dataTbls.config.randomizer_iterations];

            UiHelper.ChangeControlsEnabled(this, false);

            FileHelper.SaveSpiritTitles(dataTbls.battleData.GetBattles(), dataTbls.config.file_directory_preload);

            // If seed isn't positive, get random one. 
            for (int i = 0; i < dataTbls.config.randomizer_iterations; i++)
            {
                allTasks[i] = Task.Run(() => dataTbls.ExecuteRandomizer(i, seed+i, seed));
                Thread.Sleep(50);
            }

            await Task.WhenAll(allTasks);

            UiHelper.ChangeControlsEnabled(this, true);
            UiHelper.PopUpMessage(String.Format("Spirit Battles Randomized {0} times.\r\nChaos: {1}. \r\nSeed: {2}\r\nLocation: {3}", dataTbls.config.randomizer_iterations, dataTbls.config.chaos, seed, dataTbls.config.file_directory_randomized));
        }

        private void GetParamLabels_Click(object sender, EventArgs e)
        {
            NetworkHelper.DownloadParamLabels(dataTbls.config.labels_file_location);
        }

        private void CloseApplication_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private int TryGetSeed()
        {
            try
            {
                var seed = Int32.Parse(textboxSeed.Text);
                return seed == -1 ? RandomizerHelper.GetRandomInt() : seed;
            }
            catch
            {
                return RandomizerHelper.GetRandomInt();
            }
        }
    }
}
