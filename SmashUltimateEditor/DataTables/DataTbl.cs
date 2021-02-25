using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor.DataTables
{
    // IF YOU'RE GRABBING EMPTY VALUES, JUST SET THEM TO NULL?
    public class DataTbl : IDataTbl //: IXmlType
    {
        //public ParamType TypeKey { get; } = ParamType.@struct;

        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class OrderAttribute : Attribute
        {
            private readonly int _order;
            public OrderAttribute([CallerLineNumber] int order = 0)
            {
                _order = order;
            }

            public int Order { get { return _order; } }
        }

        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class PageAttribute : Attribute
        {
            private readonly int _page;
            public PageAttribute(int page = 0)
            {
                _page = page;
            }

            public int Page { get { return _page; } }
        }
        internal int pageCount { get { return 1; } }

        public static DataTbl GetDataTblFromXmlName(string className)
        {
            if (String.IsNullOrWhiteSpace(className))
            {
                return null;
            }

            var type = typeof(DataTbl);

            var child = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass
                                    && type.IsAssignableFrom(t) && t != type
                                    && (string)t?.GetField("XML_NAME", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                                    ?.GetValue(null) == className) ?? null;
            if (child is null || child.Count() == 0)
            {
                return null;
            }
            else if (child.Count() > 1)
            {
                return new DataTbl();
            }
            else
            {
                return (DataTbl)Activator.CreateInstance(child.First());
            }
        }

        public static IDataTbl GetDataTblFromClassName(string name)
        {
            return (IDataTbl)Activator.CreateInstance(Type.GetType(name));
        }

        public static DataTbl GetDataTblFromFirstField(string attribute)
        {
            if (String.IsNullOrWhiteSpace(attribute))
            {
                return null;
            }

            var type = typeof(DataTbl);

            var child = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass
                                    && type.IsAssignableFrom(t) && t != type
                                    && (string)t?.GetField("XML_FIRST_FIELD", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                                    ?.GetValue(null) == attribute) ?? null;
            if (child is null || child.Count() == 0)
            {
                return null;
            }
            else
            {
                return (DataTbl)Activator.CreateInstance(child.First());
            }
        }

        public void Randomize(ref Random rnd, DataTbls dataTbls)
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
            if (value is null)
            {
                return;
            }
            if ((checkRequired && (Defs.REQUIRED_PARAMS.Contains(field.Name)))
                || RandomizerHelper.ChancePass(dataTbls.config.chaos, ref rnd))
            {
                this.SetValueFromName(field.Name, value);
            }
            else if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
            {
                // Set floats to mode value?
                this.SetValueFromName(field.Name, dataTbls.GetModeFromTypeAndName(GetType(), field.Name));
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
                if (value.Equals(""))
                {
                    value = combo?.Text ?? "";
                }

                value = EnumChecker(value, combo.Name);
                this.SetValueFromName(combo.Name, value);
            }
            foreach (TextBox text in page.Controls.OfType<TextBox>())
            {
                if (!String.IsNullOrEmpty(text.Text))
                    this.SetValueFromName(text.Name, text.Text);
            }
        }

        public void UpdatePageValues(TabPage page)
        {
            foreach (ComboBox combo in page.Controls.OfType<ComboBox>())
            {
                var value = this.GetPropertyValueFromName(combo.Name);
                value = EnumChecker(value, combo.Name);

                combo.SelectedIndex = combo.Items.IndexOf(value);
                combo.Text = combo.SelectedText = value;
            }
            foreach (TextBox text in page.Controls.OfType<TextBox>())
            {
                text.Text = this.GetPropertyValueFromName(text.Name);
            }
        }

        public void UpdateFighterPageValues(TabPage page, int collectionIndex)
        {
            UpdatePageValues(page);

            Button b = UiHelper.GetFighterButton(page);
            if (!(b == default))
            {
                b.Name = collectionIndex.ToString();
            }
        }
        public Tuple<float, float> GetRangeFromName(string name)
        {
            float min = float.Parse(typeof(Defs).GetProperty(name.ToUpper() + "_MIN").GetValue(this).ToString());
            float max = float.Parse(typeof(Defs).GetProperty(name.ToUpper() + "_MAX").GetValue(this).ToString());
            return new Tuple<float, float>(min, max);
        }

        public string ValuableValue(string val)
        {
            return val == "none" ? "" : val;
        }

        public static DataTbl DetermineXmlTypeFromFirstLevel(Stream stream)
        {
            XmlReader reader = XmlReader.Create(stream);

            try
            {
                reader.Read();
                // Read until start of data.  
                XmlHelper.ReadUntilAttribute(reader, attribute: "hash");
                reader.Read();  // Skip first instance, as it is hash of List
                XmlHelper.ReadUntilAttribute(reader, attribute: "hash");
                var attribute = reader.GetAttribute("hash");
                return GetDataTblFromFirstField(attribute);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                this.SetValueFromName(DataParse.ImportNameFixer(attribute), reader.Value);
            }
            return;
        }

        public static IEnumerable<Type> GetChildrenTypes()
        {
            var type = typeof(DataTbl);

            var children = Assembly.GetExecutingAssembly().GetTypes().Where
                (t => t.IsClass && 
                type.IsAssignableFrom(t) && 
                t != type);

            return children;
        }
        public static IEnumerable<Type> GetChildrenTypes(Type type)
        {
            var children = Assembly.GetExecutingAssembly().GetTypes().Where
                (t => t.IsClass &&
                type.IsAssignableFrom(t) &&
                t != type);

            return children;
        }

        public static TabPage BuildEmptyPage(DataTbls dataTbls, Type type)
        {

            TabPage topLevelPage = UiHelper.GetEmptyTabPageFromType(type);
            topLevelPage.TabIndex = dataTbls.tabs.TabPages.Count;

            List<Point> points = new List<Point>();
            TabControl subControl = new TabControl();

            subControl.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));

            topLevelPage.Controls.Add(subControl);

            TabPage page;
            Point currentPos;
            LabelBox lb;

            // Build out subpages.  
            foreach(var pageType in UiHelper.GetSubpagesFromType(type))     //pageType is an Enum
            {
                var newPage = UiHelper.GetEmptyTabPage();

                newPage.Name = newPage.Text = pageType.ToString();
                newPage.TabIndex = (int)pageType;
                subControl.TabPages.Add(newPage);

                points.Add(new Point(0, 0));
            }

            // Set to starting points.  
            currentPos = points[0];
            page = subControl.TabPages[0];

            // If fighter, First pass, add "Remove Fighter" button to first page.  
            if (type == typeof(Fighter))
            {
                Button b = UiHelper.GetEmptyRemoveFighterButton(UiHelper.IncrementPoint(ref currentPos, page.Controls.Count, Ui_Element.Button));

                dataTbls.SetRemoveFighterButtonMethod(ref b);
                page.Controls.Add(b);
                Label spacer = new Label() { Location = UiHelper.IncrementPoint(ref currentPos, page.Controls.Count, Ui_Element.Label) };
                page.Controls.Add(spacer);

                points[0] = currentPos;
            }

            // Gather each property for the DataTbl type, and make a label/data entry point for it.  
            foreach (PropertyInfo field in type.GetProperties().OrderBy(x => x.Name))
            {
                lb = new LabelBox();
                var pageNum = UiHelper.GetUiPageFromProperty(field);
                page = subControl.TabPages[pageNum];
                currentPos = points[pageNum];

                lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count, Ui_Element.Label));

                // Range values?  Use a textbox.
                if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
                {
                    lb.SetTextBox(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1, Ui_Element.Box));
                }
                //Else - use a combo box with preset list.  
                else
                {
                    lb.SetComboBox(field.Name, dataTbls.GetOptionsFromTypeAndName(type, field.Name), UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1, Ui_Element.Box));
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

                points[pageNum] = currentPos;
            }

            // Set method to update event label options when event changes.  
            if (type == typeof(Battle))
            {
                // Add event to set event labels when event type changes.  
                var pageNum = (int)Battle_Page.Events;
                var subPage = subControl.TabPages[pageNum];

                dataTbls.SetEventOnChange(ref subPage);

                // Add Load spirit image buttons to the Battle basics page.  
                pageNum = (int)Battle_Page.Basics;
                subPage = subControl.TabPages[pageNum];
                currentPos = points[pageNum];

                // Force buttons onto new column.  
                UiHelper.MovePointToNewColumn(subPage, ref currentPos);

                for (int i = 0; i < Defs.spiritUiLocations.Count; i++)
                {
                    Button b = UiHelper.GetEmptySpiritImageButton(UiHelper.IncrementPoint(ref currentPos, subPage.Controls.Count, Ui_Element.Button));
                    b.Text = b.Name = b.Text.Replace('#', i.ToString()[0]);
                    dataTbls.SetLoadSpiritImageButtonMethod(ref b);    // Set GetEmprtSpiritImageButtonMethod
                    subPage.Controls.Add(b);

                    points[pageNum] = currentPos;
                }
            }

            return topLevelPage;
        }
    }
}
