using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor
{
    class FighterDataTbls
    {
        public List<FighterDataTbl> fighterDataList;

        public List<string> spirit_name
        {
            get { return fighterDataList.Select(x => x.spirit_name).OrderBy(x => x).ToList(); }
        }
        public List<string> battle_id
        {
            get { return fighterDataList.Select(x => x.battle_id).OrderBy(x => x).ToList(); }
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
        public List<byte> color
        {
            get { return fighterDataList.Select(x => x.color).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_hat_id
        {
            get { return fighterDataList.Select(x => x.mii_hat_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_body_id
        {
            get { return fighterDataList.Select(x => x.mii_body_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<byte> mii_color
        {
            get { return fighterDataList.Select(x => x.mii_color).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> mii_voice
        {
            get { return fighterDataList.Select(x => x.mii_voice).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<byte> mii_sp_n
        {
            get { return fighterDataList.Select(x => x.mii_sp_n).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<byte> mii_sp_s
        {
            get { return fighterDataList.Select(x => x.mii_sp_s).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<byte> mii_sp_hi
        {
            get { return fighterDataList.Select(x => x.mii_sp_hi).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<byte> mii_sp_lw
        {
            get { return fighterDataList.Select(x => x.mii_sp_lw).Distinct().OrderBy(x => x).ToList(); }
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
            get { return fighterDataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> ability1
        {
            get { return fighterDataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> ability2
        {
            get { return fighterDataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> ability3
        {
            get { return fighterDataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> ability_personal
        {
            get { return fighterDataList.Select(x => x.entry_type).Distinct().OrderBy(x => x).ToList(); }
        }

    }
}
