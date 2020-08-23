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
            if (rowCount == 0 && current.X == 0)
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
        public static Button GetEmptyRemoveFighterButton(Point pos)
        {
            Button b = new Button();
            b.Location = pos;
            b.Text = "Remove Fighter";

            return b;
        }

        public static void SetTabs(DataTbls dataTbls, ref TabControl tab)
        {
            // Empty tab pages and control.
            tab.TabPages.Clear();
            tab.TabPages.Add(dataTbls.selectedBattle.page);

            for (int i = 0; i < dataTbls.selectedFighters.Count; i++)
            {
                tab.TabPages.Add(dataTbls.selectedFighters[i].page);
            }
        }

        public static void BuildTabs(DataTbls dataTbls)
        {
            var tasks = new List<Task<TabPage>>();
            var pages = new List<TabPage>();
            tasks.Add(dataTbls.selectedBattle.BuildPageAsync(dataTbls, dataTbls.selectedBattle.battle_id));

            for (int i = 0; i < dataTbls.selectedFighters.Count; i++)
            {
                tasks.Add(dataTbls.selectedFighters[i].BuildPageAsync(dataTbls, dataTbls.selectedFighters[i].fighter_kind));
            }

            foreach (var task in tasks)
            {
                pages.Add(task.Result);
            }
        }
    }
}
