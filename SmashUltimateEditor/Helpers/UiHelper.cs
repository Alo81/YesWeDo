using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

        public static void DisableFighterButton(ref TabPage page)
        {
            Button button = page.Controls.OfType<Button>().FirstOrDefault();
            button.Enabled = false;
        }

        public static void EnableFighterButton(ref TabPage page)
        {
            Button button = page.Controls.OfType<Button>().FirstOrDefault();
            button.Enabled = true;
        }

        public static void SetupRandomizeProgress(ref ProgressBar progress, int max)
        {
            progress.Visible = true;
            progress.Minimum = 0;
            progress.Maximum = max;
            progress.Value = progress.Minimum;
            progress.Step = 1;
        }

        public static void SetPageName(ref TabPage page, string tabName, int collectionIndex)
        {
            page.Text = String.Format("{0} | [{1}]", tabName, collectionIndex);
        }
        public static void PopUpCallingClass(string message = "Error from: ")
        {
            int skipFrames = 1;
            var method = new StackTrace().GetFrame(skipFrames).GetMethod();

            MessageBox.Show(    String.Concat(message, String.Format("{0} - {1}", method.Name, method.DeclaringType.Name)  )   );
        }

        public static string ListToCSV(List<string> strings)
        {
            var csv = "";
            if (strings != null)
            {
                foreach (string text in strings)
                {
                    csv += text + ",";
                }
                if (csv.Length > 0)
                {
                    csv = csv.Substring(0, csv.Length - 1);
                }
            }

            return csv;
        }
        public static List<TabPage> GetPagesAsList(TabPage page)
        {
            List<TabPage> pages = new List<TabPage>();

            pages.Add(page);
            var subPages = page.Controls.OfType<TabControl>().First().TabPages;
            foreach(TabPage subPage in subPages)
            {
                pages.Add(subPage);
            }

            return pages;
        }

        public static void PopUpMessage(string message)
        {
            MessageBox.Show(message);
        }
        public static void SetInformativeLabel(ref Label label, string message)
        {
            label.Text = message;
        }
    }
}
