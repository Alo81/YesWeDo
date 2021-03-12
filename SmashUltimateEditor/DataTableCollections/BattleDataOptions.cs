using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables.ui_spirits_battle_db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmashUltimateEditor
{
    public class BattleDataOptions : BaseDataOptions, IDataOptions
    {
        public List<Battle> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }
        internal static Type underlyingType = typeof(Battle);

        private List<BattleEvent> _events;
        private List<string> _recommended_skill;

        public BattleDataOptions()
        {
            _dataList = new List<Battle>();
        }
        public BattleDataOptions(List<Battle> battles)
        {
            _dataList = battles;
        }
        public void SetData(List<IDataTbl> inData)
        {
            _dataList = inData.OfType<Battle>().ToList();
        }
        public static Type GetUnderlyingType()
        {
            return underlyingType;
        }
        public int GetCount()
        {
            return _dataList.Count();
        }
        public bool HasData()
        {
            return GetCount() > 0;
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
        public List<Battle> GetBattlesCopy()
        {
            List<Battle> battlesCopy = new List<Battle>();
            foreach (Battle battle in _dataList)
            {
                battlesCopy.Add(battle.Copy());
            }

            return battlesCopy;
        }
        public BattleDataOptions Copy()
        {
            var copy = new BattleDataOptions(GetBattlesCopy());
            copy._events = events;
            copy._recommended_skill = _recommended_skill;

            return copy;
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
                var ogBattleSpiritTitle = GetBattle(replBattle.battle_id).GetCombinedMsbtTitle();

                _dataList[GetBattleIndex(replBattle.battle_id)] = replBattle;

                _dataList[GetBattleIndex(replBattle.battle_id)].SetSpiritTitleParameters(ogBattleSpiritTitle);
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
        public List<string> event_type
        {
            get 
            {
                return events.Select(x => x.event_type).Distinct().OrderBy(x => x).ToList();
            }
        }

        public List<string> event_label
        {
            get
            {
                return events.Select(x => x.event_label).Distinct().OrderBy(x => x).ToList();
            }
        }

        public List<string> event_start_time
        {
            get
            {
                return events.Select(x => x.event_start_time.ToString()).Distinct().OrderBy(x => x).ToList();
            }
        }

        public List<string> event_range_time
        {
            get
            {
                return events.Select(x => x.event_range_time.ToString()).Distinct().OrderBy(x => x).ToList();
            }
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

        public List<string> recommended_skill
        {
            get
            {
                if (_recommended_skill is null)
                {
                    _recommended_skill = new List<string>();

                    _recommended_skill.Add("");
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill1).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill2).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill3).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill4).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill5).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill6).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill7).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill8).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill9).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill10).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill11).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill12).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.recommended_skill13).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.un_recommended_skill1).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.un_recommended_skill2).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.un_recommended_skill3).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.un_recommended_skill4).Distinct()));
                    _recommended_skill.AddRange((_dataList.Select(x => x.un_recommended_skill5).Distinct()));

                    _recommended_skill = _recommended_skill.Distinct().OrderBy(x => x).ToList();
                    //
                }
                return _recommended_skill;
            }
        }
    }
}
