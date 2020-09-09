using paracobNET;
using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.Interfaces;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor.DataTables
{
    // IF YOU'RE GRABBING EMPTY VALUES, JUST SET THEM TO NULL?
    public class DataTbl //: IXmlType
    {
        //public ParamType TypeKey { get; } = ParamType.@struct;

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

        public static DataTbl GetDataTblFromName(string className)
        {
            if (String.IsNullOrWhiteSpace(className))
            {
                return null;
            }

            var type = typeof(DataTbl);

            var child = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass
                                    && type.IsAssignableFrom(t) && t != type
                                    && (string)t?.GetField("XML_NAME", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                                    ?.GetValue(null) == className)?.FirstOrDefault() ?? null;
            if (child is null)
            {
                return null;
            }
            else
            {
                return (DataTbl)Activator.CreateInstance(child);
            }

        }

        public void Randomize(ref Random rnd, DataTbls dataTbls, bool checkRequired = true)
        {
            Type type = GetType();
            foreach (PropertyInfo field in type.GetProperties())
            {
                var val = GetRandomFieldValue(field, ref rnd, dataTbls);
                SaveRandomizedField(ref rnd, field, val, dataTbls);
            }
        }

        public string GetRandomFieldValue(PropertyInfo field, ref Random rnd, DataTbls dataTbls, bool overrideExclusion = false)
        {
            Type type = GetType();
            string value;
            if (!overrideExclusion && Defs.EXCLUDED_RANDOMIZED.Contains(field.Name))
            {
                return null;
            }

            // Range values?  Set a random value. 
            if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()) && !Defs.EVENT_OPTIONS.Contains(field.Name.ToLower()))
            {
                value = GetRangeValue(ref rnd, field);
            }
            else
            {
                var options = dataTbls.GetOptionsFromTypeAndName(type, field.Name);
                value = options[rnd.Next(options.Count)].ToString();
            }

            value = EnumChecker(value, field.Name);

            return value;
        }

        public void SaveRandomizedField(ref Random rnd, PropertyInfo field, string value, DataTbls dataTbls, bool checkRequired = true)
        {
            if(value is null)
            {
                return;
            }
            if ((checkRequired && (Defs.REQUIRED_PARAMS.Contains(field.Name)))
                || RandomizerHelper.ChancePass(Defs.CHAOS, ref rnd))
            {
                SetValueFromName(field.Name, value);
            }
            else if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
            {
                // Set floats to mode value?
                SetValueFromName(field.Name, dataTbls.GetModeFromTypeAndName(GetType(), field.Name));
            }
        }

        public string GetRangeValue(ref Random rnd, PropertyInfo field)
        {
            var range = GetRangeFromName(field.Name.ToUpper());
            string fieldType = field.PropertyType.Name;
            switch (fieldType)
            {
                case "Single":
                    return RandomizerHelper.GetRandomFloatInRange(range.Item1, range.Item2, rnd).ToString();
                default:
                    return rnd.Next((int)range.Item1, (int)range.Item2).ToString();
            }
        }

        public string EnumChecker(string value, string name)
        {
            if (name == "mii_color")
            {
                value = ((int)EnumUtil<Enums.mii_color_opt>.GetByName(value ?? "")).ToString();
            }
            else if (name == "mii_sp_n")
            {
                value = ((int)EnumUtil<Enums.mii_sp_n_opt>.GetByName(value ?? "")).ToString();
            }
            else if (name == "mii_sp_s")
            {
                value = ((int)EnumUtil<Enums.mii_sp_s_opt>.GetByName(value ?? "")).ToString();
            }
            else if (name == "mii_sp_hi")
            {
                value = ((int)EnumUtil<Enums.mii_sp_hi_opt>.GetByName(value ?? "")).ToString();
            }
            else if (name == "mii_sp_lw")
            {
                value = ((int)EnumUtil<Enums.mii_sp_lw_opt>.GetByName(value ?? "")).ToString();
            }
            return value;
        }

        public void UpdateTblValues(TabPage page)
        {
            foreach (ComboBox combo in page.Controls.OfType<ComboBox>())
            {
                var value = combo?.SelectedItem?.ToString() ?? "";
                
                value = EnumChecker(value, combo.Name);
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
                var value = GetPropertyValueFromName(combo.Name);
                value = EnumChecker(value, combo.Name);

                combo.SelectedIndex = combo.Items.IndexOf(value);
                combo.Text = value;
            }
            foreach (TextBox text in page.Controls.OfType<TextBox>())
            {
                text.Text = GetPropertyValueFromName(text.Name);
            }
            if(GetType().Name == "Fighter")
            {
                Button b = page.Controls.OfType<Button>().Single();
                b.Name = collectionIndex.ToString();
            }
            this.pageIndex = pageIndex;
            page.Text = String.Format("{0} | [{1}]", tabName, collectionIndex);
        }

        public void CorrectEventLabels(ref TabPage page, DataTbls dataTbls)
        {
            foreach (ComboBox combo in page.Controls.OfType<ComboBox>().Where(x => Regex.IsMatch(x.Name, "event.*type")))
            {
                var value = GetPropertyValueFromName(combo.Name);
                value = EnumChecker(value, combo.Name);

                dataTbls.SetEventLabelOptions(combo, page);

                combo.SelectedIndex = combo.Items.IndexOf(value);
                combo.Text = value;
            }
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
                    lb.SetComboBox(field.Name, dataTbls.GetOptionsFromTypeAndName(type, field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1));
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
        public string GetPropertyValueFromName(string name)
        {
            return this.GetType().GetProperty(name).GetValue(this)?.ToString().ToLower() ?? "";
        }
        public string GetFieldValueFromName(string name)
        {
            return this?.GetType()?.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(this)?.ToString()?.ToLower() ?? "";
        }
        public Tuple<float, float> GetRangeFromName(string name)
        {
            float min = float.Parse(typeof(Defs).GetProperty(name.ToUpper()+"_MIN").GetValue(this).ToString());
            float max = float.Parse(typeof(Defs).GetProperty(name.ToUpper() + "_MAX").GetValue(this).ToString());
            return new Tuple<float, float>(min, max);
        }

        public void SetValueFromName(string name, string val)
        {
            PropertyInfo field = this.GetType().GetProperty(name);
            // If val is null, interpret as empty string for our purposes.  
            val ??= "";

            try
            {
                field.SetValue(this, Convert.ChangeType(val, field.PropertyType));
            }
            catch(Exception ex)
            {
                MessageBox.Show(String.Format("Could not set value: {0} for {1}.\r\n\r\n{2}", val, name, ex.Message));
                throw ex;
            }
        }

        public string ValuableValue (string val)
        {
            return val == "none" ? "" : val;
        }

        public XElement GetAsXElement(int index)
        {
            return new XElement("struct",
            new XAttribute("index", index),
                //<hash40 hash="battle_id">default</hash40>	// <*DataListItem.Type* hash="*DataListItem.FieldName*">*DataListItem.FieldValue*</>
                this.GetType().GetProperties().OrderBy(x => ((OrderAttribute)x.GetCustomAttributes(typeof(OrderAttribute), false).Single()).Order).Select(property =>
               new XElement(DataParse.ReplaceTypes(property.PropertyType.Name.ToLower()),
               new XAttribute("hash", DataParse.ExportNameFixer(property.Name)), DataParse.ExportNameFixer(this.GetPropertyValueFromName(property.Name)))
                    )
                );
        }

        public void BuildFromXml(XmlReader reader)
        {
            string attribute;
            while (reader.Read())
            {
                while (!(reader.NodeType == XmlNodeType.Element))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("struct"))
                    {
                        return;
                    }
                }
                attribute = reader.GetAttribute("hash");
                reader.Read();
                SetValueFromName(DataParse.ImportNameFixer(attribute), reader.Value);
            }
            return;
        }
    }
}
