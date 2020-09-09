using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class FighterDataOptions : BaseDataOptions, IDataOptions
    {
        internal int pageCount { get { return _dataList.Sum(x=>x.pageCount); } }

        public List<Fighter> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }

        public FighterDataOptions()
        {
            _dataList = new List<Fighter>();
        }
        public int GetCount()
        {
            return _dataList.Count();
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

        public List<string> spirit_name
        {
            get { return _dataList.Select(x => x.spirit_name).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> battle_id
        {
            get { return _dataList.Select(x => x.battle_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> entry_type
        {
            get { return _dataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> first_appear
        {
            get { return _dataList.Select(x => x.first_appear.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> appear_rule_time
        {
            get { return _dataList.Select(x => x.appear_rule_time.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> appear_rule_count
        {
            get { return _dataList.Select(x => x.appear_rule_count.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> fighter_kind
        {
            get { return _dataList.Select(x => x.fighter_kind).Distinct().OrderBy(x => x).ToList(); }
        }
        // THIS WAS ORIGINALLY A BYTE (?)
        public List<string> color
        {
            get { return _dataList.Select(x => x.color.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_hat_id
        {
            get { return _dataList.Select(x => x.mii_hat_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_body_id
        {
            get { return _dataList.Select(x => x.mii_body_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_color
        {
            get { return EnumUtil<mii_color_opt>.GetValuesSorted(); }
        }
        public List<string> mii_voice
        {
            get { return _dataList.Select(x => x.mii_voice).Distinct().OrderBy(x => x).ToList(); }
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
        public List<string> cpu_lv
        {
            get { return _dataList.Select(x => x.cpu_lv.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_type
        {
            get { return _dataList.Select(x => x.cpu_type.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_sub_type
        {
            get { return _dataList.Select(x => x.cpu_sub_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_item_pick_up
        {
            get { return _dataList.Select(x => x.cpu_item_pick_up.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> stock
        {
            get { return _dataList.Select(x => x.stock.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> corps
        {
            get { return _dataList.Select(x => x.corps.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> _0x0f2077926c
        {
            get { return _dataList.Select(x => x._0x0f2077926c.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> hp
        {
            get { return _dataList.Select(x => x.hp.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> init_damage
        {
            get { return _dataList.Select(x => x.init_damage.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> sub_rule
        {
            get { return _dataList.Select(x => x.sub_rule).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> scale
        {
            get { return _dataList.Select(x => x.scale.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> fly_rate
        {
            get { return _dataList.Select(x => x.fly_rate.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> invalid_drop
        {
            get { return _dataList.Select(x => x.invalid_drop.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> enable_charge_final
        {
            get { return _dataList.Select(x => x.enable_charge_final.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> attack
        {
            get { return _dataList.Select(x => x.attack.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> defense
        {
            get { return _dataList.Select(x => x.defense.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> attr
        {
            get { return _dataList.Select(x => x.attr).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> ability1
        {
            get { return EnumUtil<ability_opt>.GetValuesSorted(); }
        }
        public List<string> ability2
        {
            get { return EnumUtil<ability_opt>.GetValuesSorted(); }
        }
        public List<string> ability3
        {
            get { return EnumUtil<ability_opt>.GetValuesSorted(); }
        }
        public List<string> ability_personal
        {
            get { return _dataList.Select(x => x.ability_personal).Distinct().OrderBy(x => x).ToList(); }
        }
    }
}
