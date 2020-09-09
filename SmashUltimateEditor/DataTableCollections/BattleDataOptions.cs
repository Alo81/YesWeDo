using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_spirits_battle_db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class BattleDataOptions : BaseDataOptions, IDataOptions
    {
        public List<Battle> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }

        private List<BattleEvent> _events;

        public BattleDataOptions()
        {
            _dataList = new List<Battle>();
        }
        public int GetCount()
        {
            return _dataList.Count();
        }

        public Battle GetBattle(string battle_id)
        {

            return _dataList.FirstOrDefault(x => x.battle_id == battle_id);
        }

        public int GetBattleIndex(string battle_id)
        {
            return _dataList.FindIndex(x => x.battle_id == battle_id);
        }
        public int GetBattleIndex(Battle battle)
        {
            return _dataList.FindIndex(x => x == battle);
        }
        public Battle GetBattleAtIndex(int index)
        {
            return _dataList?[index];
        }
        public List<Battle> GetBattles()
        {
            return _dataList;
        }

        public void AddBattle(Battle newBattle)
        {
            _dataList.Add(newBattle);
        }
        public void SetBattles(List<Battle> newBattles)
        {
            _dataList = newBattles;
        }

        public void ReplaceBattles(BattleDataOptions replacement)
        {
            foreach(var replBattle in replacement.GetBattles())
            {
                _dataList[GetBattleIndex(replBattle.battle_id)] = replBattle;
            }
        }
        public void ReplaceBattleAtIndex(int index, Battle newBattle)
        {
            _dataList[index] = newBattle;
        }
        public string GetXmlName()
        {
            return "battle_data_tbl";
        }
        public Type GetContainerType()
        {
            return _dataList[0].GetType();
        }

        public List<string> battle_id
        {
            get { return _dataList.Select(x => x.battle_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>battle_type{
            get { return _dataList.Select(x => x.battle_type).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> battle_time_sec
        {
            get { return _dataList.Select(x => x.battle_time_sec.ToString()).OrderBy(x => x).ToList(); }
        }

        public List<string> basic_init_damage
        {
            get { return _dataList.Select(x => x.basic_init_damage.ToString()).OrderBy(x => x).ToList(); }
        }

        public List<string> basic_init_hp
        {
            get { return _dataList.Select(x => x.basic_init_hp.ToString()).OrderBy(x => x).ToList(); }
        }

        public List<string> basic_stock
        {
            get { return _dataList.Select(x => x.basic_stock.ToString()).OrderBy(x => x).ToList(); }
        }

        public List<string>ui_stage_id
        {
            get { return _dataList.Select(x => x.ui_stage_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_type
        {
            get { return _dataList.Select(x => x.stage_type).Distinct().OrderBy(x => x).ToList(); }
        }

        // ORIGINALLY AN SBYTE.  (?)
        public List<string>_0x18e536d4f7
        {
            get { return _dataList.Select(x => x._0x18e536d4f7.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_bgm
        {
            get { return _dataList.Select(x => x.stage_bgm).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_gimmick
        {
            get { return _dataList.Select(x => x.stage_gimmick.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_attr
        {
            get { return _dataList.Select(x => x.stage_attr).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>floor_place_id
        {
            get { return _dataList.Select(x => x.floor_place_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>item_table
        {
            get { return _dataList.Select(x => x.item_table).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>item_level
        {
            get { return _dataList.Select(x => x.item_level).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>result_type
        {
            get { return _dataList.Select(x => x.result_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> event_type
        {
            get { return EnumUtil<event_type_opt>.GetValuesSorted(); }
        }

        public List<string> event_label
        {
            get { return EnumUtil<event_label_opt>.GetValuesSorted(); }
        }

        public List<string> event_start_time
        {
            get { return _dataList.Select(x => x.event1_start_time.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> event_range_time
        {
            get { return _dataList.Select(x => x.event1_range_time.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<BattleEvent> events
        {
            get { 
                if(_events is null)
                {
                    _events = new List<BattleEvent>();

                    _events.AddRange((_dataList.Select(x => new BattleEvent
                    (
                        x.event1_type, 
                        x.event1_label, 
                        x.event1_start_time, 
                        x.event1_range_time, 
                        x.event1_count, 
                        x.event1_damage
                    )).Distinct()));

                    _events.AddRange((_dataList.Select(x => new BattleEvent
                    (
                        x.event2_type,
                        x.event2_label,
                        x.event2_start_time,
                        x.event2_range_time,
                        x.event2_count,
                        x.event2_damage
                    )).Distinct()));

                    _events.AddRange((_dataList.Select(x => new BattleEvent
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
            get { return _dataList.Select(x => x.event1_count).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort> event_damage
        {
            get { return _dataList.Select(x => x.event1_damage).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> event1_type
        {
            get { return event_type; }
        }

        public List<string> event1_label
        {
            get { return event_label;  }
        }

        public List<string> event1_start_time
        {
            get { return event_start_time;  }
        }

        public List<string> event1_range_time
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

        public List<string> event2_start_time
        {
            get { return event_start_time; }
        }

        public List<string> event2_range_time
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

        public List<string> event3_start_time
        {
            get { return event_start_time; }
        }

        public List<string> event3_range_time
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
            get { return _dataList.Select(x => x.aw_flap_delay.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> _0x0d41ef8328
        {
            get { return _dataList.Select(x => x._0x0d41ef8328.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> _0x0d6f19abae
        {
            get { return _dataList.Select(x => x._0x0d6f19abae.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>_0x18d9441f71
        {
            get { return _dataList.Select(x => x._0x18d9441f71).Distinct().OrderBy(x => x).ToList();  }
        }

        public List<string> _0x18404d4ecb
        {
            get { return _dataList.Select(x => x._0x18404d4ecb).Distinct().OrderBy(x => x).ToList(); }
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
            get { return _dataList.Select(x => x._0x0ff8afd14f).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> battle_power
        {
            get { return _dataList.Select(x => x.battle_power.ToString()).OrderBy(x => x).ToList(); }
        }
		
    }
}
