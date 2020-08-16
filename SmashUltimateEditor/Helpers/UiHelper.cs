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
        public static void SetValueFromName(IDataTbl tbl, string name, object val)
        {
            // use TabIndex - 1?
            tbl.SetValueFromName(name, val);
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

        public static void SetTabs(ref DataTbls dataTbls, ref TabControl tab)
        {
            tab.TabPages.Add(dataTbls.selectedBattle.page);

            for (int i = 0; i < dataTbls.selectedFighters.fighterDataList.Count; i++)
            {
                dataTbls.selectedFighters.fighterDataList[i].BuildPage(dataTbls);
                tab.TabPages.Add(dataTbls.selectedFighters.fighterDataList[i].page);
            }
        }

        public static async Task BuildTabs(DataTbls dataTbls)
        {
            dataTbls.selectedBattle.BuildPage(dataTbls);

            for (int i = 0; i < dataTbls.selectedFighters.fighterDataList.Count; i++)
            {
                dataTbls.selectedFighters.fighterDataList[i].BuildPage(dataTbls);
            }
        }

        public static TabPage BuildPage(DataTbls dataTbls, BattleDataTbl dataTbl)
        {
            return BuildPage(dataTbls, dataTbl, dataTbl.battle_id);
        }
        public static TabPage BuildPage(ref DataTbls dataTbls, FighterDataTbl dataTbl)
        {
            return BuildPage(dataTbls, dataTbl, dataTbl.spirit_name);
        }

        private static Point IncrementPoint(ref Point current, int rowCount)
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

        public static TabPage BuildPage(DataTbls dataTbls, IDataTbl dataTbl, string name)
        {
            TabPage page = GetEmptyTabPage(dataTbls.pageCount);
            Point currentPos = new Point(0, 0);

            page.Name = name;
            page.Text = name;
            LabelBox lb;

            Type tableType = dataTbl.GetType();
            foreach(FieldInfo field in tableType.GetFields().OrderBy(x=>x.Name))
            {
                lb = new LabelBox();

                // Range values?  Use a textbox.
                if(Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetTextBox(dataTbl.GetValueFromName(field.Name), IncrementPoint(ref currentPos, page.Controls.Count));
                }
                else if (Defs.MII_MOVES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, IncrementPoint(ref currentPos, page.Controls.Count));
                    List<String> miiFighterMoves = new List<String>();
                    string fighter = dataTbl.GetValueFromName("fighter_kind");
                    // Is this a mii fighter?  If so, use his moves.  Otherwise, just use default moves.  
                    int mod = Defs.miiFighterMod.ContainsKey(fighter) ? Defs.miiFighterMod[fighter] : 0;
                    
                    for(int i = 0; i < 3; i++)
                    {
                        // THIS IS HARDCODED.  FIX.  (?)
                        miiFighterMoves.Add(Extensions.EnumUtil<Enums.mii_sp_n_opt>.GetByValue(i + mod));
                    }
                    lb.SetComboBox(dataTbl.GetValueFromName(field.Name), miiFighterMoves, IncrementPoint(ref currentPos, page.Controls.Count));
                    //lb.AddComboBoxValue(miiFighterMoves));
                    //lb.SetComboBoxDataSource(miiFighterMoves);
                    //lb.SetComboBoxValue(dataTbl.GetValueFromName(field.Name));
                }
                // If boolean, our range can be true/false. 
                else if(field.FieldType == typeof(bool))
                {
                    lb.SetLabel(field.Name, IncrementPoint(ref currentPos, page.Controls.Count));
                    List<String> boolNames = new List<String>()
                    {
                        "false",
                        "true"
                    };
                    lb.SetComboBox(dataTbl.GetValueFromName(field.Name).Equals("true") ? "true" : "false", boolNames, IncrementPoint(ref currentPos, page.Controls.Count));
                }
                //Else - use a combo box with preset list.  
                else
                {
                    lb.SetLabel(field.Name, IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetComboBox(dataTbl.GetValueFromName(field.Name), dataTbls.GetOptionsFromTypeAndName(tableType.Name, field.Name), IncrementPoint(ref currentPos, page.Controls.Count));
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
