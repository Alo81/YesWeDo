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

        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            //EnumUtil<sub_rule_opt>.Contains("metal_rule");
            buildFighterDataTab();
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

        private void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);
            listBoxFighterSpirit.DataSource = dataTbls.selectedFighters.spirit_name;

            // Empty tab pages and control.
            tabControlData.TabPages.Clear();
            tabPages = new List<TabPage>();

            TabPage fighterPage;
            int i = 0;
            foreach (FighterDataTbl fighter in dataTbls.selectedFighters.fighterDataList)
            {
                fighterPage = TabPageHelper.GetEmptyTabPage(i++);
                fighterPage.Name = fighter.spirit_name;
                fighterPage.Text = fighterPage.Name;
                tabPages.Add(fighterPage);
            }
            tabControlData.TabPages.AddRange(tabPages.ToArray());
        }

        private void listBoxFighterSpirit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
