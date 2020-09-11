using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_spirits_battle_db;
using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static SmashUltimateEditor.Enums;

namespace SmashUltimateEditor
{
    public class Battle : DataTbl
    {
        internal static string XML_NAME = "battle_data_tbl";

        public void BuildEvent(BattleEvent randEvent, int index)
        {
            var eventNum = String.Format("event{0}_", index);
            // event1_type, event1_ label,  event1_ start_time, event1_ range_time, event1_ count, event1_ damage
            SetValueFromName(eventNum + "type", randEvent.event_type);
            SetValueFromName(eventNum + "label", randEvent.event_label);
            SetValueFromName(eventNum + "start_time", randEvent.event_start_time.ToString());
            SetValueFromName(eventNum + "range_time", randEvent.event_range_time.ToString());
            SetValueFromName(eventNum + "count", randEvent.event_count.ToString());
            SetValueFromName(eventNum + "damage", randEvent.event_damage.ToString());
        }

        public Fighter GetNewFighter()
        {
            return new Fighter() { battle_id = battle_id, spirit_name = battle_id};
        }

        // Make an event object dawg.  
        public void Cleanup(ref Random rnd, int fighterCount, DataTbls dataTbls)
        {
            bool isBossType = IsBossType();
            EventSet(ref rnd, dataTbls);
            HazardCheck();
            BossCheck();
            HealthCheck(ref rnd, dataTbls);
            TimerCheck(fighterCount);
            BattlePowerCheck(ref rnd, isBossType);
        }

        public void EventSet(ref Random rnd, DataTbls dataTbls)
        {
            // Post Randomize battle modifiers
            var eventCount = RandomizerHelper.eventDistribution[rnd.Next(RandomizerHelper.eventDistribution.Count)];


            for (int j = 1; j <= eventCount; j++)
            {
                if(RandomizerHelper.ChancePass(dataTbls.config.chaos, ref rnd))
                {
                    var randEvent = GetRandomBattleEvent(ref rnd, dataTbls);
                    BuildEvent(randEvent, j);
                }
            }
        }
        public BattleEvent GetRandomBattleEvent(ref Random rand, DataTbls dataTbls)
        {
            if (dataTbls.eventData.GetCount() == 0)
            {
                return dataTbls.battleData.events[rand.Next(dataTbls.battleData.events.Count)];
            }
            else
            {
                var properties = GetType().GetProperties();
                var event_types = dataTbls.eventData.event_type;
                var randEvent = new BattleEvent();
                randEvent.event_count = Byte.Parse(GetRandomFieldValue(properties.Where(x => x.Name == "event1_count").FirstOrDefault(), ref rand, dataTbls, true));
                randEvent.event_damage = UInt16.Parse(GetRandomFieldValue(properties.Where(x => x.Name == "event1_damage").FirstOrDefault(), ref rand, dataTbls, true));
                randEvent.event_start_time = Int32.Parse(GetRandomFieldValue(properties.Where(x => x.Name == "event1_start_time").FirstOrDefault(), ref rand, dataTbls, true));
                randEvent.event_range_time = Int32.Parse(GetRandomFieldValue(properties.Where(x => x.Name == "event1_range_time").FirstOrDefault(), ref rand, dataTbls, true));
                randEvent.event_type = event_types[rand.Next(event_types.Count)];

                var labels = dataTbls.eventData.GetLabelsOfType(randEvent.event_type);
                randEvent.event_label = labels[rand.Next(labels.Count)];

                return randEvent;
            }
        }

        public void HealthCheck(ref Random rnd, DataTbls dataTbls)
        {
            // Check if init HP is lower than init damage (?)
            if (basic_init_hp < basic_init_damage)
            {
                var hold = basic_init_damage;
                basic_init_damage = basic_init_hp;
                basic_init_hp = hold;
            }

            // If HP battle, and player hass less than 30 hp, and they pass chaos chance, give em extra health.  
            if(battle_type == "hp" || battle_type == "hp_time")
            {
                basic_init_hp += (ushort)(RandomizerHelper.ChancePass(100 - dataTbls.config.chaos, ref rnd) && basic_init_hp < Defs.PLAYER_LOW_HP_CUTOFF ? Defs.PLAYER_LOW_HP_MOD : 0);
            }
        }

        public void HazardCheck()
        {
            // Check for hazards and set skills.
            if (Defs.HAZARD_SKILLS.ContainsKey(stage_attr))
            {
                int i = 1;
                foreach(var hazardRelief in Defs.HAZARD_SKILLS[stage_attr])
                {
                    var skill = String.Format("recommended_skill{0}", i++);
                    SetValueFromName(skill, hazardRelief);
                }
                i += 3; // We want to keep some randomized skills regardless.  
                while(i <= 13)
                {
                    var skill = String.Format("recommended_skill{0}", i++);
                    SetValueFromName(skill, "");
                }
            }
        }

        public void BossCheck()
        {
            if (IsBossType())
            {
                battle_type = "hp";
            }
        }

        public void TimerCheck(int count)
        {
            if (count > Defs.FIGHTER_COUNT_TIMER_CUTOFF)
            {
                battle_time_sec += (ushort)(count * Defs.FIGHTER_COUNT_TIMER_ADD);
            }
        }

        public void BattlePowerCheck(ref Random rnd, bool isBoss)
        {
            int power = rnd.Next((int)Defs.BATTLE_POWER_MIN, (int)Defs.BATTLE_POWER_MAX);

            battle_power = isBoss ? (uint)(power * 1.25) : (uint)power;
        }

        public bool IsLoseEscort()
        {
            return result_type == "lose_escort";
        }
        public bool IsBossType()
        {
            return battle_type == "boss";
        }

        public Battle Copy()
        {
            Battle newCopy = new Battle();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                newCopy.SetValueFromName(property.Name, GetPropertyValueFromName(property.Name));
            }

            return newCopy;
        }

        public static TabPage BuildEmptyPage(DataTbls dataTbls)
        {
            Type type = typeof(Battle);
            TabPage topLevelPage = UiHelper.GetEmptyTabPage(dataTbls.pageCount);
            List<Point> points = new List<Point>();
            TabControl subControl = new TabControl();

            subControl.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));

            topLevelPage.Controls.Add(subControl);

            TabPage page;
            Point currentPos;
            LabelBox lb;

            for (int i = 0; i < Enum.GetNames(typeof(Battle_Page)).Length; i++)
            {
                subControl.TabPages.Add(UiHelper.GetEmptyTabPage(i));
                subControl.TabPages[i].Name = subControl.TabPages[i].Text = ((Battle_Page)i).ToString();
                points.Add(new Point(0, 0));
            }


            // GetBattleIndex
            foreach (PropertyInfo field in type.GetProperties().OrderBy(x => x.Name))
            {
                lb = new LabelBox();
                var pageNum = field.GetCustomAttributes(true).OfType<PageAttribute>().First().Page;
                page = subControl.TabPages[pageNum];
                currentPos = points[pageNum];

                // Range values?  Use a textbox.
                if (Defs.RANGE_VALUES.Contains(field.Name.ToUpper()))
                {
                    lb.SetLabel(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count));
                    lb.SetTextBox(field.Name, UiHelper.IncrementPoint(ref currentPos, page.Controls.Count + 1));
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

                points[pageNum] = currentPos;
            }

            for (int i = 0; i < subControl.TabPages.Count; i++)
            {
                var subPage = subControl.TabPages[i];
                dataTbls.SetEventOnChange(ref subPage);
                subControl.TabPages[i] = subPage;
            }

            return topLevelPage;
        }

        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string   battle_id { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	battle_type { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public ushort	battle_time_sec { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public ushort	basic_init_damage { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public ushort	basic_init_hp { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public byte	    basic_stock { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	ui_stage_id { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	stage_type { get; set; }
        [Order][Page((int)Enums.Battle_Page.Unknowns)]
        public sbyte _0x18e536d4f7 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	stage_bgm { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public bool	    stage_gimmick { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	stage_attr { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	floor_place_id { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	item_table { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	item_level { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public string	result_type { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public string	event1_type { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public string	event1_label { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public int		event1_start_time { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public int		event1_range_time { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public byte	    event1_count { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public ushort	event1_damage { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public string	event2_type { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public string	event2_label { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public int		event2_start_time { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public int		event2_range_time { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public byte	    event2_count { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public ushort	event2_damage { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public string	event3_type { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public string	event3_label { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public int		event3_start_time { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public int		event3_range_time { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public byte	    event3_count { get; set; }
        [Order][Page((int)Enums.Battle_Page.Events)]
        public ushort	event3_damage { get; set; }
        [Order][Page((int)Enums.Battle_Page.Unknowns)]
        public bool	    _0x0d41ef8328 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public bool	    aw_flap_delay { get; set; }
        [Order][Page((int)Enums.Battle_Page.Unknowns)]
        public bool	    _0x0d6f19abae { get; set; }
        [Order][Page((int)Enums.Battle_Page.Unknowns)]
        public string	_0x18d9441f71 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Unknowns)]
        public string	_0x18404d4ecb { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill1 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill2 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill3 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill4 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill5 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill6 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill7 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill8 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill9 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill10 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill11 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill12 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	recommended_skill13 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	un_recommended_skill1 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	un_recommended_skill2 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	un_recommended_skill3 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	un_recommended_skill4 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Skills)]
        public string	un_recommended_skill5 { get; set; }
        [Order][Page((int)Enums.Battle_Page.Unknowns)]
        public string	_0x0ff8afd14f { get; set; }
        [Order][Page((int)Enums.Battle_Page.Basics)]
        public uint	    battle_power { get; set; }
    }
}
