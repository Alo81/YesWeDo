using Microsoft.WindowsAPICodePack.Dialogs;
using paracobNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YesweDo.Helpers;
using YesWeDo.DataTableCollections;
using YesWeDo.DataTables;
using YesWeDo.Helpers;
using static YesweDo.Enums;
using static YesweDo.Extensions;

namespace YesweDo
{
    public partial class SpiritEditorWindow : Form
    {

        Stopwatch watch;
        public SpiritEditorWindow()
        {
            watch = new Stopwatch();
            watch.Start();
            InitializeComponent();
            //this.
            // this.DataContext = // Must be a window for this to work(?)  Essentially for us to data bind.  Have an intermediary object that has all the data to be maniuplated.  
            // Convert to getting all objects from Factorys?
            // Genericize UI from actual logic with a ViewModel
            // Implement unit tests.  
        }

        private async void SpiritEditorWindow_Shown(object sender, EventArgs e)
        {
            UiHelper.ChangeControlsEnabled(this, false);
            await LoadDataAsync();
            UiHelper.ChangeControlsEnabled(this, true);
        }

        private async Task LoadDataAsync()
        {
            dataTbls = new DataTbls();

            StatsHelper.GetPairedValues(dataTbls.dataOptions, "ui_stage_id", "stage_additional_setting");
            //StatsHelper.GetBattleIdsWhereFieldIsLikeValue(dataTbls.fighterData, "color", "10");
            //StatsHelper.GetNumericStatsForField(dataTbls.fighterData, "attack");
            //StatsHelper.GetNumericStatsForField(dataTbls.fighterData, "defense");
            //StatsHelper.GetNumericStatsForField(dataTbls.fighterData, "cpu_lv");
            //StatsHelper.GetAttributeValues(dataTbls.dataOptions, "DataTbl.ToolTipAttribute");

            dataTbls.tabs = tabControlData;
            //dataTbls.tabs.TabIndexChanged += new System.EventHandler(dataTbls.SetSaveTabChange);
            dataTbls.progress = randomizeProgress;
            dataTbls.informativeLabel = labelInformative;

            if (dataTbls.config.check_for_updates)
            {
                Task.Factory.StartNew(() => NetworkHelper.UpdateCheck());
            }

            FileHelper.CreateDirectories(dataTbls.config.GetFileDirectories());
            await LoadAllFiles();

            if (dataTbls.battleData.HasData())
            {
                buildFighterDataTab((string)dataTbls?.battleData?.GetPropertyValuesFromName("battle_id")?.First());
                dataTbls.UpdateEventsForDbValues();
            }
            else
            {
                UiHelper.PopUpMessage($"Be sure to add a SpiritBattle DB (ui_spirits_battle_db.prc) to application directory\r\n" +
                    $"And\r\n" +
                    $"Download ParamLabels.csv from \"Tools\" dropdown.\r\n" +
                    $"Then restart application.");
            }
        }

        private void buildFighterDataTab(string battle_id)
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.GetPropertyValuesFromName("battle_id").ToList();
            dataTbls.SetSelecteds(battle_id);
        }

        private async void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            dataTbls.SetSelecteds((string)dropdownSpiritData.SelectedItem);
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
            var openDialog = new OpenFileDialog() { Title = "Open Db.", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory};
            var result = openDialog.ShowDialog();
            List<string> dbTypes = new List<string>();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                var task = Task<DataOptions>.Run(() => OpenDbWithFileName(openDialog.FileName));

                dbTypes.AddRange(AddResultsToDataTbls(task.Result));
                dataTbls.RefreshTabs();
                var dbTypeCSV = UiHelper.ListToCSV(dbTypes);

                UiHelper.SetInformativeLabel(ref labelInformative, $"Opened {dbTypeCSV}");
            }
        }

        public async Task<DataOptions> OpenDbWithFileName(string fileName)
        {
            try
            {
                return XmlHelper.ReadXML(fileName, dataTbls.config.labels_file_location);
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(String.Format("Couldn't read XML.  Is it encrypted?\r\n{0}", ex.Message));
                return null;
            }
        }

        public List<string> AddResultsToDataTbls(DataOptions results)
        {
            var fileDbType = new List<string>();

            var types = results.GetContainerTypes();

            if (types.Contains(typeof(Battle)) && types.Contains(typeof(Fighter)))
            {
                if (types.Contains(typeof(Battle)))
                {
                    dataTbls.battleData = (BattleDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Battle));
                    fileDbType.Add("Battle");
                }
                if (types.Contains(typeof(Fighter)))
                {
                    dataTbls.fighterData = (FighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Fighter));
                    fileDbType.Add("Fighter");
                }

                buildFighterDataTab(dataTbls.battleData.GetBattleAtIndex(0).battle_id);
            }
            if (types.Exists(x => x.IsSubclassOf(typeof(Event))))
            {
                dataTbls.eventData = (EventDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Event));
                dataTbls.eventData.SetFoundEventTypes(dataTbls.battleData.event_type);  //Some unique values in battleData, so we combine the two sets.  

                fileDbType.Add("Event");
            }
            if (types.Contains(typeof(Item)))
            {
                dataTbls.itemData = (ItemDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Item));
                var itemEvents = dataTbls.itemData.GetAsEvents();
                dataTbls.eventData.AddUniqueEvents(itemEvents);
                fileDbType.Add("Item");
            }
            if (types.Contains(typeof(SpiritFighter)))
            {
                dataTbls.spiritFighterData = (SpiritFighterDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(SpiritFighter));
                fileDbType.Add("Spirit Fighter");
            }
            if (types.Contains(typeof(SpiritBoard)))
            {
                dataTbls.spiritBoardData = (SpiritBoardDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(SpiritBoard));
                fileDbType.Add("Spirit Board");
            }
            if (types.Contains(typeof(Spirit)))
            {
                dataTbls.spiritData = (SpiritDataOptions)results.GetDataOptionsFromUnderlyingType(typeof(Spirit));
                fileDbType.Add("Spirit");
            }
            if (types.Contains(typeof(SpiritAbilities)))
            {
                var abilities = results.GetItemsOfType(typeof(SpiritAbilities));
                var names = abilities?.Select(x => x?.GetPropertyValueFromName(SpiritAbilities.fieldKey)).ToList();
                dataTbls.battleData.AddRecommendedSkills(names);
                dataTbls.fighterData.AddNewAbilities(names);
                fileDbType.Add("Spirit Ability");
            }
            if (types.Contains(typeof(Bgm)))
            {
                var bgms = results.GetItemsOfType(typeof(Bgm));
                var names = bgms?.Select(x => x?.GetPropertyValueFromName(Bgm.fieldKey));
                dataTbls.battleData.stage_bgm = names;
                fileDbType.Add("BGM");
            }
            if (types.Contains(typeof(Stage)))
            {
                var stages = results.GetItemsOfType(typeof(Stage));
                var names = stages?.Select(x => x?.GetPropertyValueFromName(Stage.fieldKey));
                dataTbls.battleData.ui_stage_id = names;
                fileDbType.Add("Stage");
            }

            return fileDbType;
        }

        public string OpenMsbtWithFileName(string fileName)
        {
            return FileHelper.OpenMsbtWithFilename(dataTbls.battleData.GetBattles(), fileName);
        }

        public async Task LoadAllFiles()
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
            var allTasks = new List<Task<DataOptions>>();

            foreach (string fileName in fileNames.Where(x => x.EndsWith(Defs.dbFileExtension)))
            {
                allTasks.Add(Task.Run(() => OpenDbWithFileName(fileName)));
            }
            foreach (string fileName in fileNames.Where(x => x.EndsWith(Defs.textFileExtension)))
            {
                dbTypes.Add(OpenMsbtWithFileName(fileName));
            }

            while (allTasks.Any<Task<DataOptions>>())
            {
                var task = await Task.WhenAny(allTasks);
                allTasks.Remove(task);
                dbTypes.AddRange(AddResultsToDataTbls(await task));
            }


            dbTypeCSV = UiHelper.ListToCSV(dbTypes);

            watch.Stop();
            if (fileNames.Length > 0)
            {
                UiHelper.SetInformativeLabel(ref labelInformative, $"Loaded files: {dbTypeCSV} | Took {watch.ElapsedMilliseconds} milli-seconds.");
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

                // Save encrypted version for releasing straight to Switch.
                if (packaged)
                {
                    FileHelper.ExportPackaged(dataTbls, dialog.FileName);
                }

                if (standalone)
                {
                    FileHelper.ExportStandalone(dataTbls, dialog.FileName);
                }

                UiHelper.SetInformativeLabel(ref labelInformative, "Export Complete.");
            }
        }

        private void ImportBattle_Click(object sender, EventArgs e)
        {
            var importDialog = FileHelper.GetImportBattleFileDialog(title : "Import Custom Spirit", initialDirectory: dataTbls.config.file_directory_custom_battles);

            if (importDialog.ShowDialog() == CommonFileDialogResult.Ok && !String.IsNullOrWhiteSpace(importDialog?.FileName))
            {
                ImportBattleFromTypeAndFile(importDialog.SelectedFileTypeIndex, importDialog.FileName);
            }
        }

        private void ImportBattleFromTypeAndFile(int selectedFileType, string fileName, bool setBattle = true)
        {
            DataOptions options = new DataOptions();

            if (selectedFileType == (int)Import_Filters.json)
            {
                var importedBattle = XmlHelper.DeserializeFromFile(fileName);

                options.AddDataTbl(importedBattle.battle);
                options.AddRangeDataTbl(importedBattle.fighters);
                options.AddDataTbl(importedBattle.spirit);
            }
            else if (selectedFileType == (int)Import_Filters.prc)
            {
                options = XmlHelper.ReadXML(fileName, dataTbls.config.labels_file_location);
            }
            else
            {
                return;
            }

            SaveImportToDataTbls(options);
            if (setBattle)
            {
                var battle = (Battle)options.GetItemsOfType(typeof(Battle)).FirstOrDefault();
                SetSelectedBattleByBattle(battle);
            }
        }
        private void ImportBattleOverFile_Click(object sender, EventArgs e)
        {
            var importDialog = FileHelper.GetImportBattleFileDialog(title : "Import Custom Spirit Over Current.", initialDirectory: dataTbls.config.file_directory_custom_battles);

            if (importDialog.ShowDialog() == CommonFileDialogResult.Ok && !String.IsNullOrWhiteSpace(importDialog?.FileName))
            {
                DataOptions options = new DataOptions();

                if (importDialog.SelectedFileTypeIndex == (int)Import_Filters.json)
                {
                    var importedBattle = XmlHelper.DeserializeFromFile(importDialog.FileName);

                    importedBattle.battle.msbtUpdated = true;   // We want to be sure we write the new title when we save.  

                    options.AddDataTbl(importedBattle.battle);
                    options.AddRangeDataTbl(importedBattle.fighters);
                }
                else if (importDialog.SelectedFileTypeIndex == (int)Import_Filters.prc)
                {
                    options = XmlHelper.ReadXML(importDialog.FileName, dataTbls.config.labels_file_location);
                }
                options.SetBattleIdsForAll(dataTbls.selectedBattle.battle_id);

                var battle = (Battle)options.GetItemsOfType(typeof(Battle)).FirstOrDefault();

                SaveImportToDataTbls(options);
                SetSelectedBattleByBattle(battle);
            }
        }

        private void SaveImportToDataTbls(DataOptions options)
        {
            dataTbls.ImportBattle(options);
        }

        private void SetSelectedBattleByBattle(Battle battle)
        {
            dropdownSpiritData.SelectedItem = battle.battle_id;

            UiHelper.SetInformativeLabel(ref labelInformative, "Import Complete.");
        }

        private void ImportFolderFile_Click(object sender, EventArgs e)
        {
            var openDialog = new CommonOpenFileDialog() { Title = "Replace loaded battles with all JSON battles in folder", InitialDirectory = dataTbls.config.file_directory_custom_battles, IsFolderPicker = true };

            try
            {
                if ((openDialog.ShowDialog() == CommonFileDialogResult.Ok) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
                {
                    var enumerator = new EnumerationOptions() { RecurseSubdirectories = true };
                    var files = Directory.EnumerateFiles(openDialog.FileName, "*.json", enumerator);
                    ImportAllBattlesFromFiles(files);

                    if (UiHelper.PopUpQuestion("JSON battles imported.  Import PRC Battles? (This can be VERY slow, and unnecessary if you don't have old exported custom battles.)"))
                    {
                        files = Directory.EnumerateFiles(openDialog.FileName, "*.PRC", enumerator);
                        ImportAllBattlesFromFiles(files);
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

        private void ImportAllBattlesFromFiles(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                int fileType =
                    file.EndsWith(Import_Filters.json.ToString()) ?
                    (int)Import_Filters.json
                :
                    file.EndsWith(Import_Filters.prc.ToString()) ?
                    (int)Import_Filters.prc
                :
                    -1;

                ImportBattleFromTypeAndFile(fileType, file, false);
            }
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
