using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmashUltimateEditor.Helpers
{
    class UiHelper
    {
        public static Point IncrementPoint(ref Point current, int rowCount)
        {
            rowCount -= 1;
            if (rowCount+1 == 0 && current.X == 0)
            {
                current.X = Defs.PADDING;
            }
            else if (rowCount > 0 && current.Y != 0 && rowCount % Defs.ROWS == 0)
            {
                current.X += Defs.BOX_WIDTH > Defs.LABEL_WIDTH ? Defs.BOX_WIDTH : Defs.LABEL_WIDTH;
                current.X += Defs.PADDING;
                current.Y = 0;
            }
            else
            {
                current.Y += Defs.LABEL_HEIGHT + Defs.PADDING;
            }
            return current;
        }

        public static TabPage GetEmptyTabPage(int page = 0)
        {
        
            TabPage tabPage = new TabPage()
            {
                Location = new System.Drawing.Point(4, 24),
                Padding = new System.Windows.Forms.Padding(3),
                TabIndex = page,
                UseVisualStyleBackColor = true, 
                AutoScroll = true
            };
            return tabPage;
        }
        public static Button GetEmptySaveButton(Point pos)
        {
            Button b = new Button();
            b.Location = pos;
            b.Text = "Save";

            return b;
        }

        public static void SetTabs(ref DataTbls dataTbls, ref TabControl tab)
        {
            // Empty tab pages and control.
            tab.TabPages.Clear();
            tab.TabPages.Add(dataTbls.selectedBattle.page);

            for (int i = 0; i < dataTbls.selectedFighters.fighterDataList.Count; i++)
            {
                tab.TabPages.Add(dataTbls.selectedFighters.fighterDataList[i].page);
            }
        }

        public static async Task BuildTabs(DataTbls dataTbls)
        {
            dataTbls.selectedBattle.BuildPage(dataTbls, dataTbls.selectedBattle.battle_id);

            for (int i = 0; i < dataTbls.selectedFighters.fighterDataList.Count; i++)
            {
                dataTbls.selectedFighters.fighterDataList[i].BuildPage(dataTbls, dataTbls.selectedFighters.fighterDataList[i].fighter_kind);
            }
        }
    }
}
