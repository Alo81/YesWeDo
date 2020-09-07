using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void PopUpCallingClass(string message = "Error from: ")
        {
            int skipFrames = 1;
            var method = new StackTrace().GetFrame(skipFrames).GetMethod();

            MessageBox.Show(    String.Concat(message, String.Format("{0} - {1}", method.Name, method.DeclaringType.Name)  )   );
        }
    }
}
