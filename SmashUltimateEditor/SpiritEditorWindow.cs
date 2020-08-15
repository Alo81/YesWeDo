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

        }

        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            //EnumUtil<sub_rule_opt>.Contains("metal_rule");
            buildFighterDataTab();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buildFighterDataTab()
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.battle_id;
            this.dropdownFighterData.DataSource = dataTbls.fighterData.spirit_name;
            this.dropdownFighterEntryType.DataSource = dataTbls.fighterData.entry_type;
            this.dropdownFighterFighterKind.DataSource = dataTbls.fighterData.fighter_kind;
            this.labelFighterHp.Text += String.Format(" | ({0} - {1})", Defs.HP_MIN, Defs.HP_MAX); 
        }

        private void dropdownFighterData_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dataTbls.selectedFighter = dataTbls.fighterData.fighterDataList.First(x => x.spirit_name == (string)dropdownFighterData.SelectedValue);
            this.dropdownFighterEntryType.SelectedItem = dataTbls.selectedFighter.entry_type;
            this.dropdownFighterFighterKind.SelectedItem = dataTbls.selectedFighter.fighter_kind;
            this.textBoxFighterHp.Text = dataTbls.selectedFighter.hp.ToString();
        }

        private void dropdownFighterFighterKind_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
