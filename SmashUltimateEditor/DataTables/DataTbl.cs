using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor.DataTables
{
    // IF YOU'RE GRABBING EMPTY VALUES, JUST SET THEM TO NULL?
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

        internal int pageIndex = 0;
        internal int pageCount { get { return 1; } }

        public void UpdateTblValues(TabPage page)
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

        public void UpdatePageValues(ref TabPage page, int pageIndex, string tabName, int collectionIndex)
        {
            foreach (ComboBox combo in page.Controls.OfType<ComboBox>())
            {
                var value = GetValueFromName(combo.Name);
                if (combo.Name == "mii_color")
                {
                    value = ((int)EnumUtil<Enums.mii_color_opt>.GetByName(value ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_n")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_n_opt>.GetByName(value ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_s")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_s_opt>.GetByName(value ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_hi")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_hi_opt>.GetByName(value ?? "")).ToString();
                }
                else if (combo.Name == "mii_sp_lw")
                {
                    value = ((int)EnumUtil<Enums.mii_sp_lw_opt>.GetByName(value ?? "")).ToString();
                }
                combo.SelectedIndex = combo.Items.IndexOf(value);
            }
            foreach (TextBox text in page.Controls.OfType<TextBox>())
            {
                text.Text = GetValueFromName(text.Name);
            }
            if(GetType().Name == "Fighter")
            {
                Button b = page.Controls.OfType<Button>().Single();
                b.Name = collectionIndex.ToString();
            }
            this.pageIndex = pageIndex;
            page.Text = String.Format("{0} | [{1}]", tabName, collectionIndex);
        }

        public static TabPage BuildEmptyPage(DataTbls dataTbls, Type type)
        {
            TabPage page = UiHelper.GetEmptyTabPage(dataTbls.pageCount);
            Point currentPos = new Point(0, 0);
            LabelBox lb;

            Button b = UiHelper.GetEmptyRemoveFighterButton(UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));

            /* We need to set the button name for real though.  */
            if (type.Name == "Fighter")
            {
                dataTbls.SetRemoveFighterButtonMethod(ref b);
                page.Controls.Add(b);
                Label spacer = new Label() { Location = UiHelper.IncrementPoint(ref currentPos, page.Controls.Count) };
                page.Controls.Add(spacer);
            }

            // GetBattleIndex
            foreach (PropertyInfo field in type.GetProperties().OrderBy(x => x.Name))
            {
                lb = new LabelBox();

                // Range values?  Use a textbox.
                if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetTextBox(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1));
                }
                else if (Defs.MII_MOVES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    List<String> miiFighterMoves = new List<String>();
                    //string fighter = this.GetValueFromName("fighter_kind");
                    // Is this a mii fighter?  If so, use his moves.  Otherwise, just use default moves.  
                    // int mod = Defs.miiFighterMod.ContainsKey(fighter) ? Defs.miiFighterMod[fighter] : 0;
                    int mod = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        // THIS IS HARDCODED.  FIX.  (?)
                        miiFighterMoves.Add(Extensions.EnumUtil<Enums.mii_sp_n_opt>.GetByValue(i + mod));
                    }
                    lb.SetComboBox(field.Name, miiFighterMoves, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1));
                }
                // If boolean, our range can be true/false. 
                else if (field.PropertyType == typeof(bool))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    List<String> boolNames = new List<String>()
                {
                    "false",
                    "true"
                };
                    lb.SetComboBox(field.Name, boolNames, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1));
                }
                //Else - use a combo box with preset list.  
                else
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetComboBox(field.Name, dataTbls.GetOptionsFromTypeAndName(type.Name, field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1));
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

            return page;
        }
        public string GetValueFromName(string name)
        {
            return this.GetType().GetProperty(name).GetValue(this)?.ToString().ToLower() ?? "";
        }

        public void SetValueFromName(string name, string val)
        {
            PropertyInfo field = this.GetType().GetProperty(name);
            Type type = field.PropertyType;
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

        public string ValuableValue (string val)
        {
            return val == "none" ? "" : val;
        }
    }
}
