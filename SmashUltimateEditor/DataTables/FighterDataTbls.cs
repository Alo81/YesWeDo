using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmashUltimateEditor.Enums;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class FighterDataTbls
    {
        public List<FighterDataTbl> GetFightersByBattleId(string battle_id)
        {
            return fighterDataList.Where(x => x.battle_id == battle_id).ToList();
        }
        public List<FighterDataTbl> GetFighters()
        {
            return fighterDataList;
        }

        public List<FighterDataTbl> fighterDataList;

        public List<string> spirit_name
        {
            get { return fighterDataList.Select(x => x.spirit_name).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> battle_id
        {
            get { return fighterDataList.Select(x => x.battle_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> entry_type
        {
            get { return fighterDataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<bool> first_appear
        {
            get { return fighterDataList.Select(x => x.first_appear).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<ushort> appear_rule_time
        {
            get { return fighterDataList.Select(x => x.appear_rule_time).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<ushort> appear_rule_count
        {
            get { return fighterDataList.Select(x => x.appear_rule_count).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> fighter_kind
        {
            get { return fighterDataList.Select(x => x.fighter_kind).Distinct().OrderBy(x => x).ToList(); }
        }
        // THIS WAS ORIGINALLY A BYTE (?)
        public List<string> color
        {
            get { return fighterDataList.Select(x => x.color.ToString()).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_hat_id
        {
            get { return fighterDataList.Select(x => x.mii_hat_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_body_id
        {
            get { return fighterDataList.Select(x => x.mii_body_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_color
        {
            get { return EnumUtil<mii_color_opt>.GetValuesSorted(); }
        }
        public List<string> mii_voice
        {
            get { return fighterDataList.Select(x => x.mii_voice).Distinct().OrderBy(x => x).ToList(); }
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
        public List<byte> cpu_lv
        {
            get { return fighterDataList.Select(x => x.cpu_lv).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_type
        {
            get { return fighterDataList.Select(x => x.cpu_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> cpu_sub_type
        {
            get { return fighterDataList.Select(x => x.cpu_sub_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<bool> cpu_item_pick_up
        {
            get { return fighterDataList.Select(x => x.cpu_item_pick_up).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<byte> stock
        {
            get { return fighterDataList.Select(x => x.stock).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<bool> corps
        {
            get { return fighterDataList.Select(x => x.corps).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<bool> _0x0f2077926c
        {
            get { return fighterDataList.Select(x => x._0x0f2077926c).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<ushort> hp
        {
            get { return fighterDataList.Select(x => x.hp).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<ushort> init_damage
        {
            get { return fighterDataList.Select(x => x.init_damage).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> sub_rule
        {
            get { return fighterDataList.Select(x => x.sub_rule).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<float> scale
        {
            get { return fighterDataList.Select(x => x.scale).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<float> fly_rate
        {
            get { return fighterDataList.Select(x => x.fly_rate).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<bool> invalid_drop
        {
            get { return fighterDataList.Select(x => x.invalid_drop).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<bool> enable_charge_final
        {
            get { return fighterDataList.Select(x => x.enable_charge_final).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<short> attack
        {
            get { return fighterDataList.Select(x => x.attack).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<short> defense
        {
            get { return fighterDataList.Select(x => x.defense).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> attr
        {
            get { return fighterDataList.Select(x => x.attr).Distinct().OrderBy(x => x).ToList(); }
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
            get { return fighterDataList.Select(x => x.ability_personal).Distinct().OrderBy(x => x).ToList(); }
        }
        
        

    }
}
