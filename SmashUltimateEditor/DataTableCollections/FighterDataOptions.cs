using SmashUltimateEditor.DataTableCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class FighterDataOptions : BaseDataOptions, IDataOptions
    {
        internal int pageCount { get { return _dataList.Sum(x=>x.pageCount); } }

        public List<Fighter> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }
        internal static Type underlyingType = typeof(Fighter);

        public FighterDataOptions()
        {
            _dataList = new List<Fighter>();
        }
        public void SetData(List<IDataTbl> inEvents)
        {
            _dataList = inEvents.OfType<Fighter>().ToList();
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
        public List<Fighter> GetFightersByBattleId(string battle_id)
        {
            return _dataList.Where(x => x.battle_id == battle_id).ToList();
        }
        public Fighter GetFighterAtIndex(int index)
        {
            return _dataList[index];
        }
        public int GetFighterIndex(Fighter fighter)
        {
            return _dataList.FindIndex(x => x == fighter);
        }
        public List<Fighter> GetFighters()
        {
            return _dataList;
        }
        public void RemoveFightersByBattleId(string battle_id)
        {
            _dataList.RemoveAll(x => x.battle_id == battle_id);
        }
        public void AddFighter(Fighter fighter)
        {
            _dataList.Add(fighter);
        }

        public void AddFighters(List<Fighter> fighters)
        {
            _dataList.AddRange(fighters);
        }
        public void SetFighters(List<Fighter> newFighters)
        {
            _dataList = newFighters;
        }

        public void ReplaceFighters(FighterDataOptions replacement)
        {
            foreach (var replBattle in replacement.GetFighters())
            {
                RemoveFightersByBattleId(replBattle.battle_id);
            }
            AddFighters(replacement.GetFighters());
        }

        public void ReplaceFighterAtIndex(int index, Fighter newFighter)
        {
            _dataList[index] = newFighter;
        }
        public void RemoveFighterAtIndex(int index)
        {
            _dataList.RemoveAt(index);
        }
        public string GetXmlName()
        {
            return "fighter_data_tbl";
        }
        public Type GetContainerType()
        {
            return _dataList[0].GetType();
        }

        public Fighter GetNewBoss(string battle_id)
        {
            return GetFightersByBattleId(battle_id).Single().Copy();
        }

        public List<string> Fighters
        {
            get 
            {
                var fighters = _dataList.Select(x => x.fighter_kind).Distinct().OrderBy(x => x).ToList();
                fighters.RemoveAll(x => Defs.EXCLUDED_FIGHTERS.Contains(x));
                return fighters;
            }
        }
    }
}
