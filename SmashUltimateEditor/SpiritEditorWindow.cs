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
            buildFighterDataTab();
        }

        private void dropdownFighterData_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            /*
            dataTbls.selectedFighters = dataTbls.fighterData.fighterDataList.First(x => x.spirit_name == (string)dropdownFighterData.SelectedValue);
            this.dropdownFighterEntryType.SelectedItem = dataTbls.selectedFighters[0].entry_type;
            this.dropdownFighterFighterKind.SelectedItem = dataTbls.selectedFighters[0].fighter_kind;
            this.textBoxFighterHp.Text = dataTbls.selectedFighters[0].hp.ToString();
            */
        }

        private void buildFighterDataTab()
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.battle_id;

            /*
            dataTbls.selectedFighters = dataTbls.fighterData.GetBattleFighters((string)dropdownSpiritData.SelectedItem);

            this.dropdownFighterData.DataSource = dataTbls.fighterData.GetBattleFighters((string)dropdownSpiritData.SelectedItem);
            this.dropdownFighterEntryType.DataSource = dataTbls.fighterData.entry_type;
            this.dropdownFighterFighterKind.DataSource = dataTbls.fighterData.fighter_kind;
            this.labelFighterHp.Text += String.Format(" | ({0} - {1})", Defs.HP_MIN, Defs.HP_MAX); 
            */
        }

        private async void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.SetSelectedBattle((string)dropdownSpiritData.SelectedItem);
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);
            // Empty tab pages and control.
            tabControlData.TabPages.Clear();
            tabPages = new List<TabPage>();

            await UiHelper.BuildTabs(dataTbls);
            UiHelper.SetTabs(ref dataTbls, ref tabControlData);
        }
    }
}
