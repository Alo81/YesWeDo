using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class FighterDataOptions : DataOptions
    {
        internal int pageCount { get { return dataList.Sum(x=>x.pageCount); } }

        public List<Fighter> dataList;

        public FighterDataOptions()
        {
            dataList = new List<Fighter>();
        }
        public int GetCount()
        {
            return dataList.Count();
        }
        public List<Fighter> GetFightersByBattleId(string battle_id)
        {
            return dataList.Where(x => x.battle_id == battle_id).ToList();
        }
        public Fighter GetFighterAtIndex(int index)
        {
            return dataList[index];
        }
        public int GetFighterIndex(Fighter fighter)
        {
            return dataList.FindIndex(x => x == fighter);
        }
        public List<Fighter> GetFighters()
        {
            return dataList;
        }
        public void RemoveFightersByBattleId(string battle_id)
        {
            dataList.RemoveAll(x => x.battle_id == battle_id);
        }
        public void AddFighter(Fighter fighter)
        {
            dataList.Add(fighter);
        }

        public void AddFighters(List<Fighter> fighters)
        {
            dataList.AddRange(fighters);
        }
        public void SetFighters(List<Fighter> newFighters)
        {
            dataList = newFighters;
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
            dataList[index] = newFighter;
        }
        public void RemoveFighterAtIndex(int index)
        {
            dataList.RemoveAt(index);
        }
        public string GetXmlName()
        {
            return "fighter_data_tbl";
        }
        public Type GetContainerType()
        {
            return dataList[0].GetType();
        }

        public Fighter GetNewBoss(string battle_id)
        {
            return GetFightersByBattleId(battle_id).Single().Copy();
        }

        public List<string> Fighters
        {
            get 
            {
                var fighters = dataList.Select(x => x.fighter_kind).Distinct().OrderBy(x => x).ToList();
                fighters.RemoveAll(x => Defs.EXCLUDED_FIGHTERS.Contains(x));
                return fighters;
            }
        }

        public List<string> spirit_name
        {
            get { return dataList.Select(x => x.spirit_name).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> battle_id
        {
            get { return dataList.Select(x => x.battle_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> entry_type
        {
            get { return dataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> first_appear
        {
            get { return dataList.Select(x => x.first_appear.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> appear_rule_time
        {
            get { return dataList.Select(x => x.appear_rule_time.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> appear_rule_count
        {
            get { return dataList.Select(x => x.appear_rule_count.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> fighter_kind
        {
            get { return dataList.Select(x => x.fighter_kind).Distinct().OrderBy(x => x).ToList(); }
        }
        // THIS WAS ORIGINALLY A BYTE (?)
        public List<string> color
        {
            get { return dataList.Select(x => x.color.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_hat_id
        {
            get { return dataList.Select(x => x.mii_hat_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_body_id
        {
            get { return dataList.Select(x => x.mii_body_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_color
        {
            get { return EnumUtil<mii_color_opt>.GetValuesSorted(); }
        }
        public List<string> mii_voice
        {
            get { return dataList.Select(x => x.mii_voice).Distinct().OrderBy(x => x).ToList(); }
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
            get { return dataList.Select(x => x.cpu_lv.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_type
        {
            get { return dataList.Select(x => x.cpu_type.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_sub_type
        {
            get { return dataList.Select(x => x.cpu_sub_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_item_pick_up
        {
            get { return dataList.Select(x => x.cpu_item_pick_up.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> stock
        {
            get { return dataList.Select(x => x.stock.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> corps
        {
            get { return dataList.Select(x => x.corps.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> _0x0f2077926c
        {
            get { return dataList.Select(x => x._0x0f2077926c.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> hp
        {
            get { return dataList.Select(x => x.hp.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> init_damage
        {
            get { return dataList.Select(x => x.init_damage.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> sub_rule
        {
            get { return dataList.Select(x => x.sub_rule).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> scale
        {
            get { return dataList.Select(x => x.scale.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> fly_rate
        {
            get { return dataList.Select(x => x.fly_rate.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> invalid_drop
        {
            get { return dataList.Select(x => x.invalid_drop.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> enable_charge_final
        {
            get { return dataList.Select(x => x.enable_charge_final.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> attack
        {
            get { return dataList.Select(x => x.attack.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> defense
        {
            get { return dataList.Select(x => x.defense.ToString()).OrderBy(x => x).ToList(); }
        }
        public List<string> attr
        {
            get { return dataList.Select(x => x.attr).Distinct().OrderBy(x => x).ToList(); }
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
            get { return dataList.Select(x => x.ability_personal).Distinct().OrderBy(x => x).ToList(); }
        }
    }
}
