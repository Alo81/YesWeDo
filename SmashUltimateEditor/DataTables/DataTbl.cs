using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor.DataTables
{
    public class DataTbl
    {
        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class OrderAttribute : Attribute
        {
            private readonly int order_;
            public OrderAttribute([CallerLineNumber] int order = 0)
            {
                order_ = order;
            }

            public int Order { get { return order_; } }
        }

        internal TabPage page;
        internal int pageCount { get { return page == null ? 0 : 1; } }

        public void UpdateValues()
        {
            foreach (ComboBox combo in page.Controls.OfType<ComboBox>())
            {
                var value = combo?.SelectedItem?.ToString() ?? "";
                if (combo.Name == "mii_color")
                {
                    value = ((int)EnumUtil<Enums.mii_color_opt>.GetByName(combo?.SelectedItem?.ToString() ?? "")).ToString();
                }
                else if(combo.Name == "mii_sp_n")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_n_opt>.GetByName(combo?.SelectedItem?.ToString() ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_s")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_s_opt>.GetByName(combo?.SelectedItem?.ToString() ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_hi")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_hi_opt>.GetByName(combo?.SelectedItem?.ToString() ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_lw")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_lw_opt>.GetByName(combo?.SelectedItem?.ToString() ?? "")).ToString();
                }
                SetValueFromName(combo.Name, value);
            }
            foreach (TextBox text in page.Controls.OfType<TextBox>())
            {
                SetValueFromName(text.Name, text.Text);
            }
        }

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
                    lb.SetTextBox(field.Name, this.GetValueFromName(field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
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
                    lb.SetComboBox(field.Name, this.GetValueFromName(field.Name), miiFighterMoves, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
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
                    lb.SetComboBox(field.Name, this.GetValueFromName(field.Name), boolNames, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                }
                //Else - use a combo box with preset list.  
                else
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetComboBox(field.Name, this.GetValueFromName(field.Name), dataTbls.GetOptionsFromTypeAndName(tableType.Name, field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
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

        public void SetValueFromName(string name, string val)
        {
            FieldInfo field = this.GetType().GetField(name);
            Type type = field.FieldType;
            // If val is null, interpret as empty string for our purposes.  
            val = val ?? "";

            //this.GetType().GetFields().Select(x => x.FieldType).Distinct();

            // I don't wanna use strings.  Probably a better way (?)
            switch (type.ToString())
            {
                case "System.Boolean":
                    field.SetValue(this, Boolean.Parse(val));
                    break;
                case "System.Byte":
                    field.SetValue(this, Byte.Parse(val));
                    break;
                case "System.SByte":
                    field.SetValue(this, SByte.Parse(val));
                    break;
                case "System.UInt16":
                    field.SetValue(this, UInt16.Parse(val));
                    break;
                case "System.Int16":
                    field.SetValue(this, Int16.Parse(val));
                    break;
                case "System.UInt32":
                    field.SetValue(this, UInt32.Parse(val));
                    break;
                case "System.Int32":
                    field.SetValue(this, Int32.Parse(val));
                    break;
                case "System.String":
                    field.SetValue(this, val);
                    break;
                case "System.Single":
                    field.SetValue(this, Single.Parse(val));
                    break;
            }
        }
    }
}
