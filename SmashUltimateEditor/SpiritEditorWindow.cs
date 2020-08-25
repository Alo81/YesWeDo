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
            dataTbls.SaveBattle();
            dataTbls.SaveFighters();
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
            dataTbls.Save(dataTbls.battleData, dataTbls.fighterData);
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            dataTbls.RandomizeAll();
        }
    }
}
