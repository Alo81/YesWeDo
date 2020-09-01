using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Helpers;
using SmashUltimateEditor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SmashUltimateEditor
{

    public class Fighter : DataTbl, IDataTbl
    {
        public const string XML_NAME = "fighter_data_tbl";

        public void Cleanup(ref Random rnd, bool isMain, bool isLoseEscort, List<string> fighters, bool isBoss = false)
        {
            // Post Randomize fighter modifiers
            EntryCheck(isMain, isLoseEscort);
            FighterCheck(fighters, ref rnd);
            HealthCheck();
            BossCheck(isBoss);
        }

        public void FighterCheck(List<string> options, ref Random rnd)
        {
            fighter_kind = options[rnd.Next(options.Count)];
        }

        public void StockCheck(int fighterCount)
        {
            // If there are a lot, makes multiple lives tedius
            if (fighterCount > Defs.FIGHTER_COUNT_STOCK_CUTOFF)
            {
                stock = 1;
            }
        }

        public void HealthCheck()
        {
            // Check if init HP is lower than init damage (?)
            if (hp < init_damage)
            {
                var hold = init_damage;
                init_damage = hp;
                hp = hold;
            }
        }

        public void EntryCheck(bool isMain, bool isLoseEscort)
        {
            if (isLoseEscort)
                entry_type = "friend_type";
            else if (isMain || entry_type == "boss_type")
                entry_type = "main_type";
        }
        public void FirstAppearCheck(bool isMain)
        {
            if (isMain)
                first_appear = true;
        }

        public void BossCheck(bool isBoss)
        {
            if (isBoss)
            {
                scale = scale > 1 ? scale * Defs.BOSS_SCALE_MOD : scale + Defs.BOSS_SCALE_MOD;
                attack = (short)(attack * Defs.BOSS_ATTACK_MOD);
                defense = (short)(defense * Defs.BOSS_DEFENSE_MOD);
                hp = hp < Defs.BOSS_HP_CUTOFF ? (ushort)(hp * Defs.BOSS_LOW_HP_MOD) : (ushort)(hp + Defs.BOSS_HIGH_HP_MOD);
                cpu_lv = (byte)(cpu_lv + Defs.BOSS_CPU_LVL_ADD > Defs.CPU_LV_MAX ? Defs.CPU_LV_MAX : cpu_lv + Defs.BOSS_CPU_LVL_ADD);
            }
        }

        public Fighter Copy()
        {
            Fighter newCopy = new Fighter();
            foreach(PropertyInfo property in GetType().GetProperties())
            {
                newCopy.SetValueFromName(property.Name, GetValueFromName(property.Name));
            }

            return newCopy;
        }

        [Order]
        public string battle_id { get; set; }
        [Order]
        public string entry_type { get; set; }
        [Order]
        public bool first_appear { get; set; }
        [Order]
        public ushort appear_rule_time { get; set; }
        [Order]
        public ushort appear_rule_count { get; set; }
        [Order]
        public string fighter_kind { get; set; }
        [Order]
        public byte color { get; set; }
        [Order]
        public string mii_hat_id { get; set; }
        [Order]
        public string mii_body_id { get; set; }
        [Order]
        public byte mii_color { get; set; }
        [Order]
        public string mii_voice { get; set; }
        [Order]
        public byte mii_sp_n { get; set; }
        [Order]
        public byte mii_sp_s { get; set; }
        [Order]
        public byte mii_sp_hi { get; set; }
        [Order]
        public byte mii_sp_lw { get; set; }
        [Order]
        public byte cpu_lv { get; set; }
        [Order]
        public string cpu_type { get; set; }
        [Order]
        public string cpu_sub_type { get; set; }
        [Order]
        public bool cpu_item_pick_up { get; set; }
        [Order]
        public byte stock { get; set; }
        [Order]
        public bool corps { get; set; }
        [Order]
        public bool _0x0f2077926c { get; set; }
        [Order]
        public ushort hp { get; set; }
        [Order]
        public ushort init_damage { get; set; }
        [Order]
        public string sub_rule { get; set; }
        [Order]
        public float scale { get; set; }
        [Order]
        public float fly_rate { get; set; }
        [Order]
        public bool invalid_drop { get; set; }
        [Order]
        public bool enable_charge_final { get; set; }
        [Order]
        // Primary spirit identifier
        public string spirit_name { get; set; }
        [Order]
        public short attack { get; set; }
        [Order]
        public short defense { get; set; }
        [Order]
        public string attr { get; set; }
        [Order]
        public string ability1 { get; set; }
        [Order]
        public string ability2 { get; set; }
        [Order]
        public string ability3 { get; set; }
        [Order]
        public string ability_personal { get; set; }
    }
}
