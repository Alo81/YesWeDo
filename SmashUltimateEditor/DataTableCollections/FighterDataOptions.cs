using System;
using System.Collections.Generic;
using System.Linq;
using YesweDo;
using YesWeDo.DataTables;
using static YesweDo.Enums;
using static YesweDo.Extensions;

namespace YesWeDo.DataTableCollections
{
    public class FighterDataOptions : BaseDataOptions, IDataOptions
    {
        internal int pageCount { get { return _dataList.Sum(x=>x.pageCount); } }

        public List<Fighter> _dataList;
        private List<string> _abilities;
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

        public List<string> abilities
        {
            get
            {
                if(_abilities == null)
                {

                    _abilities.Add("");
                    _abilities.AddRange((_dataList.Select(x => x.ability1).Distinct()));
                    _abilities.AddRange((_dataList.Select(x => x.ability2).Distinct()));
                    _abilities.AddRange((_dataList.Select(x => x.ability3).Distinct()));
                    _abilities.AddRange((_dataList.Select(x => x.ability_personal).Distinct()));
                }
                return _abilities;
            }
            set
            {
                _abilities = value.Distinct().OrderBy(x => x).ToList();
            }
        }

        private IEnumerable<string> basicAbilities
        {
            get
            {
                return abilities.Where(x => !(x.StartsWith("personal_") || x.StartsWith("style_")));
            }
        }
        private IEnumerable<string> personalAbilities
        {
            get
            {
                return abilities.Where(x => x.StartsWith("personal_"));
            }
        }

        public IEnumerable<string> ability1
        {
            get { return basicAbilities; }
        }
        public IEnumerable<string> ability2
        {
            get { return basicAbilities; }
        }
        public IEnumerable<string> ability3
        {
            get { return basicAbilities; }
        }
        public IEnumerable<string> ability_personal
        {
            get { return personalAbilities; }
        }

        public List<string> mii_color
        {
            get { return EnumUtil<mii_color_opt>.GetValuesSorted(); }
        }
        // NOT ORIGINAL TYPE.  Changed from BYTE tp string (?)
        public List<string> mii_sp_n
        {
            get { return EnumUtil<mii_sp_n_opt>.GetValuesSorted(); }
        }
        public List<string> mii_sp_s
        {
            get { return EnumUtil<mii_sp_s_opt>.GetValuesSorted(); }
        }
        public List<string> mii_sp_hi
        {
            get { return EnumUtil<mii_sp_hi_opt>.GetValuesSorted(); }
        }
        public List<string> mii_sp_lw
        {
            get { return EnumUtil<mii_sp_lw_opt>.GetValuesSorted(); }
        }
    }
}
