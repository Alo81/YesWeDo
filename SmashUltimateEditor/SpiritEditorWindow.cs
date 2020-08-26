using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            textboxSeed.Text = RandomizerHelper.GetRandomInt().ToString();
            buildFighterDataTab();
        }

        private void buildFighterDataTab()
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.battle_id;
            dataTbls.SetSelectedBattle((string)dropdownSpiritData.SelectedItem);
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);
        }

        private async void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.Save();
            dataTbls.SetSelectedBattle((string)dropdownSpiritData.SelectedItem);
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);
            dataTbls.BuildTabs();
        }

        private async void btnAddFighter_Click(object sender, EventArgs e)
        {
            string battleId = dataTbls.selectedBattle.battle_id;

            Fighter newFighter = dataTbls.selectedFighters[0].ShallowCopy();

            dataTbls.fighterData.AddFighter(newFighter);
            dataTbls.selectedFighters.Add(newFighter);
            dataTbls.BuildTabs();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataTbls.SaveToFile(dataTbls.battleData, dataTbls.fighterData);
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            int seed;
            try
            {
                seed = Int32.Parse(textboxSeed.Text);
            }
            catch
            {
                seed = -1;
            }

            // If seed isn't positive, get random one. 
            dataTbls.RandomizeAll(seed < 0? RandomizerHelper.GetRandomInt() : seed);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            dataTbls.ExportCurrentBattle();
        }
    }
}
