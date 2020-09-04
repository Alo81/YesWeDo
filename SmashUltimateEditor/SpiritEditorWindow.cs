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
            dataTbls.encrypt = checkBoxEncrypt.Checked;
            dataTbls.decrypt = checkBoxDecrypt.Checked;

            buildFighterDataTab(dataTbls.battleData.battle_id.First());


            // Find all unlockable characters.  Could have probably just done this by hand but at this point, committed.  
            var x = dataTbls.fighterData.fighter_kind.RemoveAll(x => Defs.EXCLUDED_FIGHTERS.Contains(x));
            var y = dataTbls.fighterData.battle_id.Where(x => Defs.UNLOCKABLE_FIGHTERS.Contains(x));
            var z = dataTbls.battleData.GetBattles().Where(x => x.GetValueFromName("_0x18e536d4f7") != 0.ToString());
            var a = dataTbls.battleData.GetBattles().Where(x => 
            x.GetValueFromName("_0x0d6f19abae").ToUpper() == "FALSE" 
            &&
            x.GetValueFromName("_0x18e536d4f7") != 0.ToString()
            );


            var b = dataTbls.battleData.GetBattles().Where(x =>
            x.GetValueFromName("_0x0d6f19abae").ToUpper() == "FALSE"
            &&
            x.GetValueFromName("_0x18e536d4f7") == 2.ToString()
            );
            //_0x0d6f19abae
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
                battle.Randomize(rnd, dataTbls, false);
                battle.Cleanup(ref rnd, dataTbls.battleData.events, dataTbls.selectedFighters.Count);
            }
            else
            {
                index -= 1; // We're looking at fighters now, start at index 0.
                var fighter = dataTbls.selectedFighters[index];

                var isMain = index == 0;     // Set first fighter to main.  
                var isLoseEscort = index == 1 && battle.IsLoseEscort();     // Set second fighter to ally, if Lose Escort result type.  
                var isBoss = index == 0 && battle.IsBossType();     // Set second fighter to ally, if Lose Escort result type.  
                fighter.Randomize(rnd, dataTbls, false);
                fighter.Cleanup(ref rnd, isMain, isLoseEscort, dataTbls.fighterData.Fighters, isBoss);
            }
            dataTbls.RefreshTabs();
        }

        private void OpenDbFile_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog() { Title = "Import Unencrypted Spirit Battle", Filter = "PRC|*.prc*", InitialDirectory = dataTbls.config.file_directory};
            var result = openDialog.ShowDialog();

            if (!result.Equals(DialogResult.Cancel) && !String.IsNullOrWhiteSpace(openDialog?.FileName))
            {
                try
                {
                    dataTbls.EmptySpiritData();
                    dataTbls.ReadXML(openDialog.FileName, ref dataTbls.battleData, ref dataTbls.fighterData);

                    var battle_id = dataTbls.battleData.GetBattleAtIndex(0).battle_id;

                    buildFighterDataTab(battle_id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Couldn't read XML.  Is it encrypted?\r\n{0}", ex.Message));
                    return;
                }
            }
            dataTbls.RefreshTabs();
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
