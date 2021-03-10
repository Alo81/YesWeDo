using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_spirits_battle_db;
using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static SmashUltimateEditor.Enums;

namespace SmashUltimateEditor
{
    public class Battle : DataTbl
    {
        internal static string XML_NAME = "battle_data_tbl";
        internal string msbtTitle;
        internal string msbtSort;
        internal string msbtSeparator;

        // Make an event object dawg.  
        public void Cleanup(ref Random rnd, int fighterCount, DataTbls dataTbls, bool isUnlockableFighterType = false)
        {
            bool isBossType = IsBossType();
            EventSet(ref rnd, dataTbls);
            HazardCheck();
            BossCheck();
            HealthCheck(ref rnd, dataTbls);
            TimerCheck(fighterCount, isUnlockableFighterType, dataTbls.config.minimum_battle_time);
            BattlePowerCheck(ref rnd, isBossType);
        }

        public void BuildEvent(BattleEvent randEvent, int index)
        {
            var eventNum = String.Format("event{0}_", index);
            // event1_type, event1_ label,  event1_ start_time, event1_ range_time, event1_ count, event1_ damage
            this.SetValueFromName(eventNum + "type", randEvent.event_type);
            this.SetValueFromName(eventNum + "label", randEvent.event_label);
            this.SetValueFromName(eventNum + "start_time", randEvent.event_start_time.ToString());
            this.SetValueFromName(eventNum + "range_time", randEvent.event_range_time.ToString());
            this.SetValueFromName(eventNum + "count", randEvent.event_count.ToString());
            this.SetValueFromName(eventNum + "damage", randEvent.event_damage.ToString());
        }

        public Fighter GetNewFighter()
        {
            return new Fighter() { battle_id = battle_id, spirit_name = battle_id};
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
                IDataTbl pulledEvent = dataTbls.eventData.GetRandomEvent(ref rand);

                var randEvent = new BattleEvent();

                // Chance to get a random event from label, vs random event from event type.  Should help spread things out hopefully. 
                if (RandomizerHelper.ChancePass(Defs.EVENT_CHECK, ref rand))
                {
                    randEvent.event_type = ((Event)pulledEvent).GetTypeName();
                    randEvent.event_label = pulledEvent.GetPropertyValueFromName("label");
                }
                else
                {
                    var event_types = dataTbls.eventData.event_type;
                    randEvent.event_type = event_types[rand.Next(event_types.Count)];

                    var labels = dataTbls.eventData.GetLabelsOfType(randEvent.event_type);
                    randEvent.event_label = labels[rand.Next(labels.Count)];
                }

                randEvent.event_count = Byte.Parse(GetRandomFieldValue(properties.FirstOrDefault(x => x.Name == "event1_count"), ref rand, dataTbls, true));
                randEvent.event_damage = UInt16.Parse(GetRandomFieldValue(properties.FirstOrDefault(x => x.Name == "event1_damage"), ref rand, dataTbls, true));
                randEvent.event_start_time = Int32.Parse(GetRandomFieldValue(properties.FirstOrDefault(x => x.Name == "event1_start_time"), ref rand, dataTbls, true));
                randEvent.event_range_time = Int32.Parse(GetRandomFieldValue(properties.FirstOrDefault(x => x.Name == "event1_range_time"), ref rand, dataTbls, true));

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
                    this.SetValueFromName(skill, hazardRelief);
                }
                i += 3; // We want to keep some randomized skills regardless.  
                while(i <= 13)
                {
                    var skill = String.Format("recommended_skill{0}", i++);
                    this.SetValueFromName(skill, "");
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

        public void TimerCheck(int count, bool isUnlockableFighterType, int minimumTimer)
        {
            if (isUnlockableFighterType)
            {
                battle_type = battle_type.Replace("_time", "");
            }
            if (count > Defs.FIGHTER_COUNT_TIMER_CUTOFF)
            {
                battle_time_sec += (ushort)(count * Defs.FIGHTER_COUNT_TIMER_ADD);
            }
            battle_time_sec = (ushort)Math.Max(battle_time_sec, minimumTimer);  //Always use at least minimum timer
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
                newCopy.SetValueFromName(property.Name, this.GetPropertyValueFromName(property.Name));
            }

            return newCopy;
        }

        public string GetCombinedMsbtTitle()
        {
            return string.Concat(msbtTitle, msbtSeparator, msbtSort);
        }

        public void SetSpiritTitleParameters(string title)
        {

            try
            {
                // Find boundaries of string separator.  
                var sepStart = title.LastIndexOf('\u000e'); //For strange titles, there can be multiple instances of this, so we want the last one we see.  
                var sepEnd = sepStart + title.Substring(sepStart).IndexOf('\0');   //Search the first instance of \0 that shows up after the parsed display title, add that index amount to the already found SepStart size.  

                msbtTitle = title.Substring(0, sepStart);
                msbtSeparator = title.Substring(sepStart, sepEnd - sepStart);
                msbtSort = title.Substring(sepEnd, title.Length - sepEnd);
            }
            catch
            {
                return; // It doesn't have a title.  Thats fine, just return.  
            }
        }

        public static TabPage BuildEmptyPage(DataTbls dataTbls)
        {
            return BuildEmptyPage(dataTbls, typeof(Battle));
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
        [Order][Page((int)Enums.Battle_Page.Basics)][Excluded(true)]
        public string spiritTitle 
        { 
            get { return msbtTitle; }
            set { msbtTitle = value; } 
        }
        [Order][Page((int)Enums.Battle_Page.Basics)][Excluded(true)]
        public string spiritSortTitle 
        {
            get 
            {
                return msbtSort?.Replace(Convert.ToString('\0'), "") ?? String.Empty;  
            }
            set 
            {
                var totalLength = msbtSort.Length;
                StringBuilder newString = new StringBuilder();  
                newString.Append('\0');     //Start off with \0 character since Sort string is sandwiched on both ends.  
                foreach (var ch in value)
                {
                    newString.Append(ch);
                    newString.Append('\0');
                }
                while(newString.Length < totalLength)
                {
                    newString.Append(" ");
                    newString.Append('\0');
                }

                msbtSort = newString.ToString(); 
            } 
        }
    }
}
