using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class BattleDataOptions : DataOptions
    {
        public List<Battle> battleDataList;

        private List<Tuple<string, string, int, int, byte, ushort>> _events;

        public BattleDataOptions()
        {
            battleDataList = new List<Battle>();
        }
        public int GetBattleCount()
        {
            return battleDataList.Count();
        }

        public Battle GetBattle(string battle_id)
        {

            return battleDataList.FirstOrDefault(x => x.battle_id == battle_id);
        }

        public int GetBattleIndex(string battle_id)
        {
            return battleDataList.FindIndex(x => x.battle_id == battle_id);
        }
        public int GetBattleIndex(Battle battle)
        {
            return battleDataList.FindIndex(x => x == battle);
        }
        public Battle GetBattleAtIndex(int index)
        {
            return battleDataList?[index];
        }
        public List<Battle> GetBattles()
        {
            return battleDataList;
        }

        public void AddBattle(Battle newBattle)
        {
            battleDataList.Add(newBattle);
        }

        public void ReplaceBattles(BattleDataOptions replacement)
        {
            foreach(var replBattle in replacement.GetBattles())
            {
                battleDataList[GetBattleIndex(replBattle.battle_id)] = replBattle;
            }
        }
        public void ReplaceBattleAtIndex(int index, Battle newBattle)
        {
            battleDataList[index] = newBattle;
        }
        public string GetXmlName()
        {
            return "battle_data_tbl";
        }
        public Type GetContainerType()
        {
            return battleDataList[0].GetType();
        }

        public List<string> battle_id
        {
            get { return battleDataList.Select(x => x.battle_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>battle_type{
            get { return battleDataList.Select(x => x.battle_type).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>battle_time_sec
        {
            get { return battleDataList.Select(x => x.battle_time_sec).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>basic_init_damage
        {
            get { return battleDataList.Select(x => x.basic_init_damage).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>basic_init_hp
        {
            get { return battleDataList.Select(x => x.basic_init_hp).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<byte>basic_stock
        {
            get { return battleDataList.Select(x => x.basic_stock).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>ui_stage_id
        {
            get { return battleDataList.Select(x => x.ui_stage_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_type
        {
            get { return battleDataList.Select(x => x.stage_type).Distinct().OrderBy(x => x).ToList(); }
        }

        // ORIGINALLY AN SBYTE.  (?)
        public List<string>_0x18e536d4f7
        {
            get { return battleDataList.Select(x => x._0x18e536d4f7.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_bgm
        {
            get { return battleDataList.Select(x => x.stage_bgm).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_gimmick
        {
            get { return battleDataList.Select(x => x.stage_gimmick.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_attr
        {
            get { return battleDataList.Select(x => x.stage_attr).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>floor_place_id
        {
            get { return battleDataList.Select(x => x.floor_place_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>item_table
        {
            get { return battleDataList.Select(x => x.item_table).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>item_level
        {
            get { return battleDataList.Select(x => x.item_level).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>result_type
        {
            get { return battleDataList.Select(x => x.result_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> event_type
        {
            get { return EnumUtil<event_type_opt>.GetValuesSorted(); }
        }

        public List<string> event_label
        {
            get { return EnumUtil<event_label_opt>.GetValuesSorted(); }
        }

        public List<int> event_start_time
        {
            get { return battleDataList.Select(x => x.event1_start_time).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<int> event_range_time
        {
            get { return battleDataList.Select(x => x.event1_range_time).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<Tuple<string, string, int, int, byte, ushort>> events
        {
            get { 
                if(_events is null)
                {
                    _events = new List<Tuple<string, string, int, int, byte, ushort>>();

                    _events.AddRange((battleDataList.Select(x => new Tuple<string, string, int, int, byte, ushort>
                    (
                        x.event1_type, 
                        x.event1_label, 
                        x.event1_start_time, 
                        x.event1_range_time, 
                        x.event1_count, 
                        x.event1_damage
                    )).Distinct()));

                    _events.AddRange((battleDataList.Select(x => new Tuple<string, string, int, int, byte, ushort>
                    (
                        x.event2_type,
                        x.event2_label,
                        x.event2_start_time,
                        x.event2_range_time,
                        x.event2_count,
                        x.event2_damage
                    )).Distinct()));

                    _events.AddRange((battleDataList.Select(x => new Tuple<string, string, int, int, byte, ushort>
                    (
                        x.event3_type,
                        x.event3_label,
                        x.event3_start_time,
                        x.event3_range_time,
                        x.event3_count,
                        x.event3_damage
                    )).Distinct()));

                    _events = _events.Distinct().ToList();
                    //
                }
                return _events; 
                }
        }

        public List<byte> event_count
        {
            get { return battleDataList.Select(x => x.event1_count).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort> event_damage
        {
            get { return battleDataList.Select(x => x.event1_damage).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> event1_type
        {
            get { return event_type; }
        }

        public List<string> event1_label
        {
            get { return event_label;  }
        }

        public List<int> event1_start_time
        {
            get { return event_start_time;  }
        }

        public List<int> event1_range_time
        {
            get { return event_range_time; }
        }

        public List<byte> event1_count
        {
            get { return event_count; }
        }

        public List<ushort> event1_damage
        {
            get { return event_damage; }
        }
        public List<string> event2_type
        {
            get { return event_type; }
        }

        public List<string> event2_label
        {
            get { return event_label;  }
        }

        public List<int> event2_start_time
        {
            get { return event_start_time; }
        }

        public List<int> event2_range_time
        {
            get { return event_range_time; }
        }

        public List<byte> event2_count
        {
            get { return event_count; }
        }

        public List<ushort> event2_damage
        {
            get { return event_damage; }
        }
        public List<string> event3_type
        {
            get { return event_type; }
        }

        public List<string> event3_label
        {
            get { return event_label; }
        }

        public List<int> event3_start_time
        {
            get { return event_start_time; }
        }

        public List<int> event3_range_time
        {
            get { return event_range_time; }
        }

        public List<byte> event3_count
        {
            get { return event_count; }
        }

        public List<ushort> event3_damage
        {
            get { return event_damage; }
        }

        public List<string>aw_flap_delay
        {
            get { return battleDataList.Select(x => x.aw_flap_delay.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> _0x0d41ef8328
        {
            get { return battleDataList.Select(x => x._0x0d41ef8328.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> _0x0d6f19abae
        {
            get { return battleDataList.Select(x => x._0x0d6f19abae.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>_0x18d9441f71
        {
            get { return battleDataList.Select(x => x._0x18d9441f71).Distinct().OrderBy(x => x).ToList();  }
        }

        public List<string> _0x18404d4ecb
        {
            get { return battleDataList.Select(x => x._0x18404d4ecb).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>recommended_skill
        {
            get { return EnumUtil<ability_opt>.GetValuesSorted(); }
        }
        public List<string> recommended_skill1
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill2
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill3
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill4
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill5
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill6
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill7
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill8
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill9
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill10
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill11
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill12
        {
            get { return recommended_skill; }
        }
        public List<string> recommended_skill13
        {
            get { return recommended_skill; }
        }

        public List<string> un_recommended_skill
        {
            get { return EnumUtil<ability_opt>.GetValuesSorted(); }
        }
        public List<string> un_recommended_skill1
        {
            get { return un_recommended_skill; }
        }
        public List<string> un_recommended_skill2
        {
            get { return un_recommended_skill; }
        }
        public List<string> un_recommended_skill3
        {
            get { return un_recommended_skill; }
        }
        public List<string> un_recommended_skill4
        {
            get { return un_recommended_skill; }
        }
        public List<string> un_recommended_skill5
        {
            get { return un_recommended_skill; }
        }

        public List<string>_0x0ff8afd14f
{
            get { return battleDataList.Select(x => x._0x0ff8afd14f).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<uint> battle_power
        {
            get { return battleDataList.Select(x => x.battle_power).Distinct().OrderBy(x => x).ToList(); }
        }
		
    }
}
