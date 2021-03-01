using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmashUltimateEditor.DataTables.DataTbl;
using static SmashUltimateEditor.Enums;
using static System.Windows.Forms.TabControl;

namespace SmashUltimateEditor.Helpers
{
    class UiHelper
    {
        public static Point IncrementPoint(ref Point current, int rowCount, Ui_Element ui)
        {
            //First added element.  
            if (rowCount == 0 && current.X == 0)
            {
                current.X = Defs.LABEL_PADDING;
            }
            // If we've got a full column, move to the next one.  
            else if (rowCount > 0 && current.Y != 0 && rowCount % Defs.ROWS == 0)
            {
                current.X += Defs.BOX_WIDTH > Defs.LABEL_WIDTH ? Defs.BOX_WIDTH : Defs.LABEL_WIDTH;
                current.X += Defs.LABEL_PADDING;
                current.Y = 0;
            }
            // Add another row.  
            else
            {
                // Figure out which UI element we have.  
                switch (ui)
                {
                    case Ui_Element.Box:
                        current.Y += Defs.BOX_HEIGHT + Defs.BOX_PADDING;
                        break;
                    case Ui_Element.Button:
                        current.Y += Defs.BUTTON_HEIGHT + Defs.BUTTON_PADDING;
                        break;
                    case Ui_Element.Label:
                        current.Y += Defs.LABEL_HEIGHT + Defs.LABEL_PADDING;
                        break;
                    default:
                        current.Y += Defs.LABEL_HEIGHT + Defs.LABEL_PADDING;
                        break;
                }
            }
            return current;
        }

        public static void MovePointToNewColumn(TabPage page, ref Point current)
        {
            // Force buttons onto new column.  
            while (page.Controls.Count % Defs.ROWS != 0)
            {
                page.Controls.Add(new Label() { Location = UiHelper.IncrementPoint(ref current, page.Controls.Count, Ui_Element.Label) });
            }
        }

        public static Array GetSubpagesFromType(Type type)
        {
            if(type == typeof(Battle))
            {
                return Enum.GetValues(typeof(Battle_Page));
            }
            else if (type == typeof(Fighter))
            {
                return Enum.GetValues(typeof(Fighter_Page));
            }
            else    // If no subpages are defined, get a single empty name tab.  
            {
                return new string[] { "" };
            }
        }

        public static string GetPageNameFromType(Type type)
        {
            return type.Name;
        }

        public static int GetUiPageFromProperty(PropertyInfo field)
        {
            return field?.GetCustomAttributes(true)?.OfType<PageAttribute>()?.FirstOrDefault()?.Page ?? 0;
        }

        public static TabPage GetEmptyTabPage()
        {

            TabPage tabPage = new TabPage()
            {
                Location = new System.Drawing.Point(4, 24),
                Padding = new System.Windows.Forms.Padding(3),
                UseVisualStyleBackColor = true,
                AutoScroll = true
            };
            return tabPage;
        }

        public static TabPage GetEmptyTabPageFromType(Type type)
        {
            var page = GetEmptyTabPage();
            page.Name = GetPageNameFromType(type);

            return page;
        }

        public static Button GetEmptyButton()
        {
            Button b = new Button();
            b.Height = Defs.BUTTON_HEIGHT;
            b.Width = Defs.BUTTON_WIDTH;

            return b;
        }

        public static Button GetEmptyRemoveFighterButton(Point pos)
        {
            var b = GetEmptyButton();
            b.Location = pos;
            b.Text = b.Name = Defs.REMOVE_FIGHTER_BUTTON_STRING;

            return b;
        }

        public static Button GetEmptySpiritImageButton(Point pos)
        {
            var b = GetEmptyButton();
            b.Location = pos;
            b.Text = Defs.SPIRIT_IMAGE_BUTTON_STRING;

            return b;
        }

        public static int? GetSpiritButtonIndexFromButton(Button b)
        {
            var name = b.Name;
            foreach(char c in name)
            {
                if (Char.IsDigit(c))
                {
                    return (int)Char.GetNumericValue(c);
                }
            }
            return null;
        }

        public static IEnumerable<Button> GetButtons(TabPage page)
        {
            return page.Controls.OfType<Button>();
        }

        public static Button GetRemoveFighterButtonFromButtons(IEnumerable<Button> buttons)
        {
            return buttons.FirstOrDefault(x => x.Text == Defs.REMOVE_FIGHTER_BUTTON_STRING);
        }

        public static Button GetFighterButton(TabPage page)
        {
            var buttons = GetButtons(page);
            return GetRemoveFighterButtonFromButtons(buttons);
        }

        public static void DisableFighterButton(TabPage page)
        {
            GetFighterButton(page).Enabled = false;
        }

        public static void EnableFighterButton(TabPage page)
        {
            GetFighterButton(page).Enabled = true;
        }

        public static void SetupRandomizeProgress(ProgressBar progress, int max)
        {
            progress.Visible = true;
            progress.Minimum = 0;
            progress.Maximum = max;
            progress.Value = progress.Minimum;
            progress.Step = 1;
        }

        public static void SetPageName(TabPage page, string tabName, int collectionIndex)
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
        public static List<TabPage> GetPagesAsList(TabPage page, bool inclusive = true)
        {
            List<TabPage> pages = new List<TabPage>();

            if (inclusive)  // If inclusive, add this page and its subpages.  Else, only add subpages.  
            {
                pages.Add(page);
            }
            var subPages = page.Controls.OfType<TabControl>().First().TabPages;
            foreach(TabPage subPage in subPages)
            {
                pages.Add(subPage);
            }

            return pages;
        }
        public static List<TabPage> GetPagesFromTabControl(TabControl tabControl)
        {
            List<TabPage> pages = new List<TabPage>();

            var controlPages = tabControl.TabPages;

            foreach(TabPage page in controlPages)
            {
                pages.Add(page);
            }

            return pages;
        }
        public static List<TabPage> GetAllPagesFromTabControl(TabControl tabControl)
        {
            List<TabPage> pages = new List<TabPage>();

            var controlPages = tabControl.TabPages;

            foreach (TabPage page in controlPages)
            {
                pages.Add(page);
                foreach (TabControl subTabs in page.Controls.OfType<TabControl>())
                {
                    pages.AddRange(GetAllPagesFromTabControl(subTabs));
                }
            }

            return pages;
        }

        public static string GetLastFolder(string path)
        {
            return new DirectoryInfo(path).Name;
        }

        public static void PopUpMessage(string message)
        {
            Task.Factory.StartNew(() => MessageBox.Show(message));
        }
        public static void SetInformativeLabel(ref Label label, string message)
        {
            label.Text = message;

            // Cycle color.
            Color c = label.BackColor;
            int r = c.R;    int g = c.G;    int b = c.B;
            int t;
            if(r == 255)
            {
                t = g;
                g = r;
                r = t;
            }
            else if (g == 255)
            {
                t = b;
                b = g;
                g = t;
            }
            else if (b == 255)
            {
                t = r;
                r = b;
                b = t;
            }

            label.BackColor = Color.FromArgb(r, g, b);
        }
    }
}
