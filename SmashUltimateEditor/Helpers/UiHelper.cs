using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SmashUltimateEditor.Helpers
{
    class UiHelper
    {
        public static TabPage GetEmptyTabPage(int page = 0)
        {
        
            TabPage tabPage = new TabPage()
            {
                Location = new System.Drawing.Point(4, 24),
                Padding = new System.Windows.Forms.Padding(3),
                Size = new System.Drawing.Size(768, 150),
                TabIndex = page,
                UseVisualStyleBackColor = true
            };
            return tabPage;
        }

        public static List<TabPage> BuildTabs(ref DataTbls dataTbls)
        {
            BattleDataTbl battleDataTbl = dataTbls.selectedBattle;
            FighterDataTbls fightersTbl = dataTbls.selectedFighters;

            List<TabPage> tabPages = new List<TabPage>();
            tabPages.Add(BuildPage(ref dataTbls, dataTbls.selectedBattle));

            foreach (FighterDataTbl fighter in dataTbls.selectedFighters.GetFighters())
            {
                tabPages.Add(BuildPage(ref dataTbls, fighter));
            }
            return tabPages;
        }

        public static TabPage BuildPage(ref DataTbls dataTbls, BattleDataTbl dataTbl)
        {
            return BuildPage(ref dataTbls, dataTbl, dataTbl.battle_id);
        }
        public static TabPage BuildPage(ref DataTbls dataTbls, FighterDataTbl dataTbl)
        {
            return BuildPage(ref dataTbls, dataTbl, dataTbl.spirit_name);
        }


        public static TabPage BuildPage(ref DataTbls dataTbls, IDataTbl dataTbl, string name)
        {
            TabPage page = GetEmptyTabPage();
            page.Name = name;
            page.Text = name;
            LabelBox lb;

            Type tableType = dataTbl.GetType();
            foreach(FieldInfo field in tableType.GetFields())
            {
                lb = new LabelBox();

                // Range values?  Use a textbox.
                if(Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabelValue(String.Format("{0}", field.Name));
                    lb.SetTextBoxValue(dataTbl.GetValueFromName(field.Name));
                }
                else if (Defs.MII_MOVES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabelValue(String.Format("{0})", field.Name));
                    List<String> miiFighterMoves = new List<String>();
                    string fighter = dataTbl.GetValueFromName("fighter_kind");
                    // Is this a mii fighter?  If so, use his moves.  Otherwise, just use default moves.  
                    int mod = Defs.miiFighterMod.ContainsKey(fighter) ? Defs.miiFighterMod[fighter] : 0;
                    
                    for(int i = 0; i < 3; i++)
                    {
                        // THIS IS HARDCODED.  FIX.  (?)
                        miiFighterMoves.Add(Extensions.EnumUtil<Enums.mii_sp_n_opt>.GetByValue(i + mod));
                        lb.AddComboBoxValue(miiFighterMoves.Last());
                    }
                    lb.SetComboBoxDataSource(miiFighterMoves);
                    lb.SetComboBoxValue(dataTbl.GetValueFromName(field.Name));
                }
                // If boolean, our range can be true/false. 
                else if(field.FieldType == typeof(bool))
                {
                    List<String> boolNames = new List<String>()
                    {
                    "false",
                    "true"
                    };

                    lb.SetLabelValue(field.Name);
                    foreach(string val in boolNames)
                    {
                        lb.AddComboBoxValue(val);
                    }
                    lb.SetComboBoxDataSource(boolNames);
                    lb.SetComboBoxValue(dataTbl.GetValueFromName(field.Name).Equals("true") ? true : false);
                }
                //Else - use a combo box with preset list.  
                else
                {
                    lb.SetLabelValue(field.Name);
                    lb.SetComboBoxDataSource(dataTbls.GetOptionsFromTypeAndName(tableType.Name, field.Name));
                    lb.SetComboBoxValue(dataTbl.GetValueFromName(field.Name));
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

            foreach(ComboBox c in page.Controls.OfType<ComboBox>())
            {
                foreach(object x in (List<String>)c.DataSource)
                {
                    //string y = x.ToString();
                }
            }

            return page;
        }
    }
}
