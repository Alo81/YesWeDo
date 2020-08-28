using Microsoft.WindowsAPICodePack.Dialogs;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            textboxSeed.Text = RandomizerHelper.GetRandomInt().ToString();
            buildFighterDataTab(dataTbls.battleData.battle_id.First());
        }

        private void buildFighterDataTab(string battle_id)
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.battle_id;
            dataTbls.SetSelectedBattle(battle_id);
            dataTbls.SetSelectedFighters(battle_id);
        }

        private async void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.Save();
            dataTbls.SetSelectedBattle((string)dropdownSpiritData.SelectedItem);
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);
            dataTbls.RefreshTabs();
        }

        private async void btnAddFighter_Click(object sender, EventArgs e)
        {
            Fighter newFighter = dataTbls.selectedFighters[0].Copy();

            dataTbls.fighterData.AddFighter(newFighter);
            dataTbls.selectedFighters.Add(newFighter);
            dataTbls.RefreshTabs();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataTbls.SaveToFile(dataTbls.battleData, dataTbls.fighterData);
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            dataTbls.Save();
            int index = dataTbls.tabs.SelectedIndex;
            var battle = dataTbls.selectedBattle;

            int seed = TryGetSeed();
            Random rnd = new Random(seed);
            if (index == 0)
            {
                battle.Randomize(rnd, dataTbls);
                battle.Cleanup(ref rnd, dataTbls.battleData.events, dataTbls.selectedFighters.Count);
            }
            else
            {
                index -= 1; // We're looking at fighters now, start at index 0.
                var fighter = dataTbls.selectedFighters[index];

                var isMain = index == 0;     // Set first fighter to main.  
                var isLoseEscort = index == 1 && battle.IsLoseEscort();     // Set second fighter to ally, if Lose Escort result type.  
                var isBoss = index == 0 && battle.IsBossType();     // Set second fighter to ally, if Lose Escort result type.  
                fighter.Randomize(rnd, dataTbls);
                dataTbls.FighterRandomizeCleanup(ref fighter, ref rnd, isMain, isLoseEscort, isBoss);
            }
            dataTbls.RefreshTabs();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            dataTbls.ExportCurrentBattle();
        }

        private void OpenDbFile_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = Defs.FILE_DIRECTORY};
            openDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                dataTbls.EmptySpiritData();
                dataTbls.ReadXML(openDialog.FileName, ref dataTbls.battleData, ref dataTbls.fighterData);

                var battle_id = dataTbls.battleData.GetBattleAtIndex(0).battle_id;

                buildFighterDataTab(battle_id);
            }
            dataTbls.RefreshTabs();
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            dataTbls.SaveToFile();
        }

        private void SaveAsFile_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog() { Title = "Save Unencrypted Spirit Battles", Filter = "PRC|*.prc*", FileName = Defs.FILE_NAME, InitialDirectory = Defs.FILE_DIRECTORY };

            saveDialog.ShowDialog();
            if(!String.IsNullOrWhiteSpace(saveDialog?.FileName))
            {
                dataTbls.SaveToFile(saveDialog.FileName);
            }
        }

        // Modify this to use a dialog prompt so we can choose where to save it.  
        private void ExportBattleFile_Click(object sender, EventArgs e)
        {
            dataTbls.ExportCurrentBattle();
        }

        private void ImportBattle_Click(object sender, EventArgs e)
        {
            var importDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = Defs.FILE_DIRECTORY };
            importDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(importDialog?.FileName))
            {
                var battles = new BattleDataOptions();
                var fighters = new FighterDataOptions();
                dataTbls.ReadXML(importDialog.FileName, ref battles, ref fighters);

                dataTbls.battleData.ReplaceBattles(battles);
                dataTbls.fighterData.ReplaceFighters(fighters);

                var battle_id = battles.GetBattleAtIndex(0).battle_id;

                dataTbls.SetSelectedBattle(battle_id);
                dataTbls.SetSelectedFighters(battle_id);
            }
            dataTbls.RefreshTabs();
        }
        private void ImportFolderFile_Click(object sender, EventArgs e)
        {
            var openDialog = new CommonOpenFileDialog() { Title = "Replace loaded battles with all battles in folder", InitialDirectory = Defs.CUSTOM_BATTLES_DIRECTORY, IsFolderPicker = true };
            openDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                var files = Directory.EnumerateFiles(openDialog.FileName).Where(x => x.Contains(".prc"));

                var battles = new BattleDataOptions();
                var fighters = new FighterDataOptions();
                foreach (string file in files)
                {
                    dataTbls.ReadXML(file, ref battles, ref fighters);

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
                return Int32.Parse(textboxSeed.Text);
            }
            catch
            {
                return RandomizerHelper.GetRandomInt();
            }
        }
    }
}
