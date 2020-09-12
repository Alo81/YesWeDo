using Microsoft.WindowsAPICodePack.Dialogs;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmashUltimateEditor
{
    public partial class SpiritEditorWindow : Form
    {
        public SpiritEditorWindow()
        {
            InitializeComponent();
            dataTbls = new DataTbls();
            dataTbls.tabs = tabControlData;
            dataTbls.tabs.TabIndexChanged += new System.EventHandler(dataTbls.SetSaveTabChange);
            dataTbls.progress = randomizeProgress;
            dataTbls.encrypt = checkBoxEncrypt.Checked;
            dataTbls.decrypt = checkBoxDecrypt.Checked;

            buildFighterDataTab(dataTbls.battleData.battle_id.First());
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
            Fighter newFighter = dataTbls.selectedFighters[0].Copy();

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
        }

        private void OpenDbFile_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory};
            var result = openDialog.ShowDialog();
            List<string> dbType;

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                dbType = OpenDbWithFileName(openDialog.FileName);
                UiHelper.PopUpMessage($"Opened {dbType}");
            }
        }

        public List<string> OpenDbWithFileName(string fileName)
        {

            try
            {
                var battles = new BattleDataOptions();
                var fighters = new FighterDataOptions();
                var events = new EventDataOptions();
                var items = new ItemDataOptions();
                var spiritFighters = new SpiritFighterDataOptions();
                var fileDbType = new List<string>();

                var results = dataTbls.ReadXML(fileName);

                battles.SetBattles(results.GetBattles());
                fighters.SetFighters(results.GetFighters());
                events.SetEvents(results.GetEvents());
                items.SetItems(results.GetItems());
                spiritFighters.SetSpiritFighters(results.GetSpiritFighters());

                if (battles.GetCount() > 0)
                {
                    dataTbls.EmptySpiritData();
                    dataTbls.battleData = battles;
                    dataTbls.fighterData = fighters;
                    var battle_id = dataTbls.battleData.GetBattleAtIndex(0).battle_id;

                    buildFighterDataTab(battle_id);

                    fileDbType.Add("Battle");
                }
                if (events.GetCount() > 0)
                {
                    dataTbls.eventData = events;
                    dataTbls.UpdateEventsForDbValues();

                    fileDbType.Add("Event");
                }
                if (items.GetCount() > 0)
                {
                    dataTbls.itemData = items;
                    var itemEvents = dataTbls.itemData.GetAsEvents();
                    dataTbls.eventData.AddUniqueEvents(itemEvents);
                    fileDbType.Add("Item");
                }
                if (spiritFighters.GetCount() > 0)
                {
                    dataTbls.spiritFighterData = spiritFighters;
                    fileDbType.Add("Spirit Fighter");
                }
                return fileDbType;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Couldn't read XML.  Is it encrypted?\r\n{0}", ex.Message));
                return null;
            }
        }

        public void LoadAllFiles()
        {
            var config = new Config();
            var directory = config.file_directory_preload;
            var fileNames = Directory.GetFiles(directory);
            var dbTypes = new List<string>();
            var fileNamesCSV = "";

            foreach (string fileName in fileNames)
            {
                dbTypes.AddRange(OpenDbWithFileName(fileName));
            }

            foreach(string dbType in dbTypes)
            {
                fileNamesCSV += "\r\n" + dbType + ",";
            }

            if (fileNames.Length > 0)
            {
                fileNamesCSV = fileNamesCSV.Substring(0, fileNamesCSV.Length - 1);
                UiHelper.PopUpMessage($"Loaded files: {fileNamesCSV}");
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            dataTbls.Save();
        }

        private void SaveAsFile_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog() 
            { 
                Title = String.Format("Save {0} Spirit Battles", dataTbls.encrypt ? "Encrypted" : "Unencrypted"), 
                Filter = "PRC|*.prc*", 
                FileName = dataTbls.encrypt ? dataTbls.config.file_name_encr : dataTbls.config.file_name, 
                InitialDirectory = dataTbls.encrypt ? dataTbls.config.file_directory_encr : dataTbls.config.file_directory 
            };

            var result = saveDialog.ShowDialog();
            if(!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(saveDialog?.FileName))
            {
                dataTbls.Save(saveDialog.FileName);
            }
        }

        // Modify this to use a dialog prompt so we can choose where to save it.  
        private void ExportBattleFile_Click(object sender, EventArgs e)
        {
            dataTbls.SaveLocal();
            BattleDataOptions singleBattle = new BattleDataOptions();
            singleBattle.AddBattle(dataTbls.battleData.GetBattle(dataTbls.selectedBattle.battle_id));
            FighterDataOptions fighters = new FighterDataOptions();
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
                dataTbls.Save(singleBattle, fighters, saveDialog.FileName);
            }
        }

        private void ImportBattle_Click(object sender, EventArgs e)
        {
            var importDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory };
            var result = importDialog.ShowDialog();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(importDialog?.FileName))
            {
                dataTbls.ImportBattle(importDialog.FileName);
                dataTbls.RefreshTabs();
            }
        }
        private void ImportFolderFile_Click(object sender, EventArgs e)
        {
            var openDialog = new CommonOpenFileDialog() { Title = "Replace loaded battles with all battles in folder", InitialDirectory = dataTbls.config.file_directory_custom_battles, IsFolderPicker = true };
            openDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                var files = Directory.EnumerateFiles(openDialog.FileName).Where(x => x.Contains(".prc"));

                var battles = new BattleDataOptions();
                var fighters = new FighterDataOptions();
                foreach (string file in files)
                {
                    var results = dataTbls.ReadXML(file);
                    battles.SetBattles(results.GetBattles());
                    fighters.SetFighters(results.GetFighters());

                    dataTbls.battleData.ReplaceBattles(battles);
                    dataTbls.fighterData.ReplaceFighters(fighters);

                    var battle_id = battles.GetBattleAtIndex(0).battle_id;

                    if (dataTbls.selectedBattle.battle_id == battle_id)
                    {
                        dataTbls.SetSelectedBattle(battle_id);
                        dataTbls.SetSelectedFighters(battle_id);
                    }

                }
            }
            dataTbls.RefreshTabs();
        }

        private void RandomizeAllTool_Click(object sender, EventArgs e)
        {
            int seed = TryGetSeed();

            // If seed isn't positive, get random one. 
            dataTbls.RandomizeAll(seed);
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

        private void checkBoxEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            dataTbls.encrypt = checkBoxEncrypt.Checked;
        }

        private void checkBoxDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            dataTbls.decrypt = checkBoxDecrypt.Checked;
        }
    }
}
