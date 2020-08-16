using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SmashUltimateEditor.DataTables
{
    public class DataTbl
    {
        internal TabPage page;
        internal int pageCount { get { return page == null ? 0 : 1; } }

        public void BuildPage(DataTbls dataTbls, string name)
        {
            TabPage page = UiHelper.GetEmptyTabPage(dataTbls.pageCount);
            Point currentPos = new Point(0, 0);
            Button b = UiHelper.GetEmptySaveButton(UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
            dataTbls.SetSaveButtonMethod(ref b);

            page.Controls.Add(b);

            page.Name = name;
            page.Text = name;
            LabelBox lb;

            Type tableType = this.GetType();
            foreach (FieldInfo field in tableType.GetFields().OrderBy(x => x.Name))
            {
                lb = new LabelBox();

                // Range values?  Use a textbox.
                if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetTextBox(this.GetValueFromName(field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                }
                else if (Defs.MII_MOVES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    List<String> miiFighterMoves = new List<String>();
                    string fighter = this.GetValueFromName("fighter_kind");
                    // Is this a mii fighter?  If so, use his moves.  Otherwise, just use default moves.  
                    int mod = Defs.miiFighterMod.ContainsKey(fighter) ? Defs.miiFighterMod[fighter] : 0;

                    for (int i = 0; i < 3; i++)
                    {
                        // THIS IS HARDCODED.  FIX.  (?)
                        miiFighterMoves.Add(Extensions.EnumUtil<Enums.mii_sp_n_opt>.GetByValue(i + mod));
                    }
                    lb.SetComboBox(this.GetValueFromName(field.Name), miiFighterMoves, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                }
                // If boolean, our range can be true/false. 
                else if (field.FieldType == typeof(bool))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    List<String> boolNames = new List<String>()
                    {
                        "false",
                        "true"
                    };
                    lb.SetComboBox(this.GetValueFromName(field.Name).Equals("true") ? "true" : "false", boolNames, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                }
                //Else - use a combo box with preset list.  
                else
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetComboBox(this.GetValueFromName(field.Name), dataTbls.GetOptionsFromTypeAndName(tableType.Name, field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                }

                page.Controls.Add(lb.label);
                if (lb.IsComboSet())
                {
                    page.Controls.Add(lb.combo);
                }
                else if (lb.IsTextboxSet())
                {
                    page.Controls.Add(lb.text);
                }
            }

            this.page = page;
        }
        public string GetValueFromName(string name)
        {
            return this.GetType().GetField(name).GetValue(this)?.ToString() ?? "";
        }
        public void SetValueFromName(string name, object val)
        {
            this.GetType().GetField(name).SetValue(this, val);
        }
    }
}
