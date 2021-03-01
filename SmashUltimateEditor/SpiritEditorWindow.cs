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
using System.Windows.Forms;

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
            dataTbls.tabs.TabIndexChanged += new System.EventHandler(dataTbls.SetSaveTabChange);
            dataTbls.progress = randomizeProgress;
            dataTbls.informativeLabel = labelInformative;

            if (dataTbls.battleData.HasData())
            {
                buildFighterDataTab(dataTbls?.battleData?.battle_id?.First());
            }
            LoadAllFiles();
        }

        private void buildFighterDataTab(string battle_id)
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.battle_id;
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
            }
            catch (Exception ex)
            {
                UiHelper.PopUpMessage(String.Format("Couldn't read XML.  Is it encrypted?\r\n{0}", ex.Message));
            }

            return fileDbType;
        }

        public void LoadAllFiles()
        {
            var config = new Config();
            var directory = config.file_directory_preload;
            string[] fileNames;
            try
            {
                Directory.CreateDirectory(directory);
                fileNames = Directory.GetFiles(directory);
            }
            catch(Exception ex)
            {
                UiHelper.PopUpMessage(ex.Message);
                return;
            }
            var dbTypes = new List<string>();
            string dbTypeCSV;

            foreach (string fileName in fileNames)
            {
                dbTypes.AddRange(OpenDbWithFileName(fileName));
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

        private void ExportBattleFile_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            BattleDataOptions singleBattle = new BattleDataOptions();
            FighterDataOptions fighters = new FighterDataOptions();

            singleBattle.AddBattle(dataTbls.battleData.GetBattle(dataTbls.selectedBattle.battle_id));
            fighters.AddFighters(dataTbls.selectedFighters);

            var saveDialog = new SaveFileDialog()
            {
                Title = "Export Spirit Battle",
                Filter = "PRC|*.prc*",
                FileName = String.Format("{0}_{1}", dataTbls.config.file_name, singleBattle.battle_id.First()),
                InitialDirectory = dataTbls.config.file_directory_custom_battles
            };

            var result = saveDialog.ShowDialog();
            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(saveDialog?.FileName))
            {
                FileHelper.Save(singleBattle, fighters, Path.GetDirectoryName(saveDialog.FileName), Path.GetFileName(saveDialog.FileName), unencrypted:true, encrypted:false);
                UiHelper.SetInformativeLabel(ref labelInformative, "Export Complete.");
            }
        }
        private void ExportAllForSwitch_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();

            var openFolderDialog = new FolderBrowserDialog
            {

                Description = "Export Battles for Switch.",
                SelectedPath = dataTbls.config.file_directory_custom_battles
            };

            var result = openFolderDialog.ShowDialog();
            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openFolderDialog?.SelectedPath))
            {
                FileHelper.Save(dataTbls.battleData, dataTbls.fighterData, openFolderDialog.SelectedPath, dataTbls.config.file_name_encr, unencrypted: false, encrypted: true, useFolderStructure : true);
                FileHelper.CopyPreloadFiles(openFolderDialog.SelectedPath);
                FileHelper.CopySpiritImages(openFolderDialog.SelectedPath);
                UiHelper.SetInformativeLabel(ref labelInformative, "Export Complete.");
            }

        }
        private void ExportModForRelease_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            BattleDataOptions singleBattle = new BattleDataOptions();
            FighterDataOptions fighters = new FighterDataOptions();

            singleBattle.AddBattle(dataTbls.battleData.GetBattle(dataTbls.selectedBattle.battle_id));
            fighters.AddFighters(dataTbls.selectedFighters);

            var openFolderDialog = new FolderBrowserDialog
            {
                Description = "Export Mod for Release.",
                SelectedPath = dataTbls.config.file_directory_custom_battles
            };

            var result = openFolderDialog.ShowDialog();
            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openFolderDialog?.SelectedPath))
            {
                var fileName = String.Format("{0}_{1}", dataTbls.config.file_name, singleBattle.battle_id.First());

                FileHelper.Save(singleBattle, fighters, openFolderDialog.SelectedPath, dataTbls.config.file_name_encr, unencrypted: false, encrypted: true, useFolderStructure : true); // Save encrypted version for releasing straight to Switch.
                
                FileHelper.CopyPreloadFiles(openFolderDialog.SelectedPath);
                FileHelper.CopySpiritImagesForBattle(openFolderDialog.SelectedPath, dataTbls.selectedBattle.battle_id);
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
            var result = openDialog.ShowDialog();
            try
            {
                if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
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

                    UiHelper.SetInformativeLabel(ref labelInformative, "Import Complete.");
                }
            }
            catch 
            { 
                UiHelper.PopUpMessage("Couldn't import battles due to error."); 
            };
        }

        private void RandomizeAllTool_Click(object sender, EventArgs e)
        {
            int seed = TryGetSeed();

            // If seed isn't positive, get random one. 
            dataTbls.RandomizeAll(seed);
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
