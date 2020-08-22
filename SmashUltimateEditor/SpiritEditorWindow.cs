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

        private void buildFighterDataTab()
        {
            this.dropdownSpiritData.DataSource = dataTbls.battleData.battle_id;
        }

        private async void dropdownSpiritData_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTbls.SetSelectedBattle((string)dropdownSpiritData.SelectedItem);
            dataTbls.SetSelectedFighters((string)dropdownSpiritData.SelectedItem);

            List<TabPage> pages = await UiHelper.BuildTabs(dataTbls);
            UiHelper.SetTabs(ref dataTbls, ref tabControlData);
        }
    }
}
