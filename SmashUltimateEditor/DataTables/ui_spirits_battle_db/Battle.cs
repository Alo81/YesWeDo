using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace SmashUltimateEditor
{
    public class Battle : DataTbl, IDataTbl
    {
        public const string XML_NAME = "battle_data_tbl";

        public void BuildEvent(Tuple<string, string, int, int, byte, ushort> randEvent, int index)
        {
            var eventNum = String.Format("event{0}_", index);
            // event1_type, event1_ label,  event1_ start_time, event1_ range_time, event1_ count, event1_ damage
            SetValueFromName(eventNum + "type", randEvent.Item1);
            SetValueFromName(eventNum + "label", randEvent.Item2);
            SetValueFromName(eventNum + "start_time", randEvent.Item3.ToString());
            SetValueFromName(eventNum + "range_time", randEvent.Item4.ToString());
            SetValueFromName(eventNum + "count", randEvent.Item5.ToString());
            SetValueFromName(eventNum + "damage", randEvent.Item6.ToString());
        }

        public Fighter GetNewFighter()
        {
            return new Fighter() { battle_id = battle_id, spirit_name = battle_id};
        }

        // Make an event object dawg.  
        public void Cleanup(ref Random rnd, List<Tuple<string, string, int, int, byte, ushort>> events, int fighterCount)
        {
            bool isBossType = IsBossType();
            EventSet(rnd, events);
            HazardCheck();
            BossCheck();
            HealthCheck();
            TimerCheck(fighterCount);
            BattlePowerCheck(rnd, isBossType);
        }

        public void EventSet(Random rnd, List<Tuple<string, string, int, int, byte, ushort>> events)
        {
            // Post Randomize battle modifiers
            var eventCount = RandomizerHelper.eventDistribution[rnd.Next(RandomizerHelper.eventDistribution.Count)];

            for (int j = 1; j <= eventCount; j++)
            {
                var randEvent = events[rnd.Next(events.Count)];
                if(RandomizerHelper.ChancePass(Defs.CHAOS, rnd))
                    BuildEvent(randEvent, j);
            }
        }

        public void HealthCheck()
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
                basic_init_hp += (ushort)(RandomizerHelper.ChancePass(Defs.CHAOS) && basic_init_hp < 30 ? 0 : (ushort)Defs.PLAYER_LOW_HP_MOD);
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

        public void BattlePowerCheck(Random rnd, bool isBoss)
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
                newCopy.SetValueFromName(property.Name, GetValueFromName(property.Name));
            }

            return newCopy;
        }

        [Order]
        public string   battle_id { get; set; }
        [Order]
        public string	battle_type { get; set; }
        [Order]
        public ushort	battle_time_sec { get; set; }
        [Order]
        public ushort	basic_init_damage { get; set; }
        [Order]
        public ushort	basic_init_hp { get; set; }
        [Order]
        public byte	    basic_stock { get; set; }
        [Order]
        public string	ui_stage_id { get; set; }
        [Order]
        public string	stage_type { get; set; }
        [Order]
        public sbyte _0x18e536d4f7 { get; set; }
        [Order]
        public string	stage_bgm { get; set; }
        [Order]
        public bool	    stage_gimmick { get; set; }
        [Order]
        public string	stage_attr { get; set; }
        [Order]
        public string	floor_place_id { get; set; }
        [Order]
        public string	item_table { get; set; }
        [Order]
        public string	item_level { get; set; }
        [Order]
        public string	result_type { get; set; }
        [Order]
        public string	event1_type { get; set; }
        [Order]
        public string	event1_label { get; set; }
        [Order]
        public int		event1_start_time { get; set; }
        [Order]
        public int		event1_range_time { get; set; }
        [Order]
        public byte	    event1_count { get; set; }
        [Order]
        public ushort	event1_damage { get; set; }
        [Order]
        public string	event2_type { get; set; }
        [Order]
        public string	event2_label { get; set; }
        [Order]
        public int		event2_start_time { get; set; }
        [Order]
        public int		event2_range_time { get; set; }
        [Order]
        public byte	    event2_count { get; set; }
        [Order]
        public ushort	event2_damage { get; set; }
        [Order]
        public string	event3_type { get; set; }
        [Order]
        public string	event3_label { get; set; }
        [Order]
        public int		event3_start_time { get; set; }
        [Order]
        public int		event3_range_time { get; set; }
        [Order]
        public byte	    event3_count { get; set; }
        [Order]
        public ushort	event3_damage { get; set; }
        [Order]
        public bool	    _0x0d41ef8328 { get; set; }
        [Order]
        public bool	    aw_flap_delay { get; set; }
        [Order]
        public bool	    _0x0d6f19abae { get; set; }
        [Order]
        public string	_0x18d9441f71 { get; set; }
        [Order]
        public string	_0x18404d4ecb { get; set; }
        [Order]
        public string	recommended_skill1 { get; set; }
        [Order]
        public string	recommended_skill2 { get; set; }
        [Order]
        public string	recommended_skill3 { get; set; }
        [Order]
        public string	recommended_skill4 { get; set; }
        [Order]
        public string	recommended_skill5 { get; set; }
        [Order]
        public string	recommended_skill6 { get; set; }
        [Order]
        public string	recommended_skill7 { get; set; }
        [Order]
        public string	recommended_skill8 { get; set; }
        [Order]
        public string	recommended_skill9 { get; set; }
        [Order]
        public string	recommended_skill10 { get; set; }
        [Order]
        public string	recommended_skill11 { get; set; }
        [Order]
        public string	recommended_skill12 { get; set; }
        [Order]
        public string	recommended_skill13 { get; set; }
        [Order]
        public string	un_recommended_skill1 { get; set; }
        [Order]
        public string	un_recommended_skill2 { get; set; }
        [Order]
        public string	un_recommended_skill3 { get; set; }
        [Order]
        public string	un_recommended_skill4 { get; set; }
        [Order]
        public string	un_recommended_skill5 { get; set; }
        [Order]
        public string	_0x0ff8afd14f { get; set; }
        [Order]
        public uint	    battle_power { get; set; }
    }
}
