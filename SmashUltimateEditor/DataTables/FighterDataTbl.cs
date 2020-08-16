using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SmashUltimateEditor
{

    public class FighterDataTbl : IDataTbl
    {
        internal TabPage page;
        internal int pageCount { get { return page == null ? 0 : 1; } }
        public void BuildPage(DataTbls dataTbls)
        {
            page = UiHelper.BuildPage(dataTbls, this, spirit_name);
        }
        public void BuildFromXml(XmlReader reader)
        {
            string attribute;
            while (reader.Read())
            {
                while (!(reader.NodeType == XmlNodeType.Element))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("struct"))
                    {
                        return;
                    }
                }
                attribute = reader.GetAttribute("hash");
                reader.Read();
                switch (attribute)
                {
                    case "battle_id": battle_id = reader.Value; break;
                    case "entry_type": entry_type = reader.Value; break;
                    case "first_appear": first_appear = Convert.ToBoolean(reader.Value); break;
                    case "appear_rule_time": appear_rule_time = Convert.ToUInt16(reader.Value); break;
                    case "appear_rule_count": appear_rule_count = Convert.ToUInt16(reader.Value); break;
                    case "fighter_kind": fighter_kind = reader.Value; break;
                    case "color": color = Convert.ToByte(reader.Value); break;
                    case "mii_hat_id": mii_hat_id = reader.Value; break;
                    case "mii_body_id": mii_body_id = reader.Value; break;
                    case "mii_color": mii_color = Convert.ToByte(reader.Value); break;
                    case "mii_voice": mii_voice = reader.Value; break;
                    case "mii_sp_n": mii_sp_n = Convert.ToByte(reader.Value); break;
                    case "mii_sp_s": mii_sp_s = Convert.ToByte(reader.Value); break;
                    case "mii_sp_hi": mii_sp_hi = Convert.ToByte(reader.Value); break;
                    case "mii_sp_lw": mii_sp_lw = Convert.ToByte(reader.Value); break;
                    case "cpu_lv": cpu_lv = Convert.ToByte(reader.Value); break;
                    case "cpu_type": cpu_type = reader.Value; break;
                    case "cpu_sub_type": cpu_sub_type = reader.Value; break;
                    case "cpu_item_pick_up": cpu_item_pick_up = Convert.ToBoolean(reader.Value); break;
                    case "stock": stock = Convert.ToByte(reader.Value); break;
                    case "corps": corps = Convert.ToBoolean(reader.Value); break;
                    case "0x0f2077926c": _0x0f2077926c = Convert.ToBoolean(reader.Value); break;
                    case "hp": hp = Convert.ToUInt16(reader.Value); break;
                    case "init_damage": init_damage = Convert.ToUInt16(reader.Value); break;
                    case "sub_rule": sub_rule = reader.Value; break;
                    case "scale": scale = (float)Convert.ToDouble(reader.Value); break;
                    case "fly_rate": fly_rate = (float)Convert.ToDouble(reader.Value); break;
                    case "invalid_drop": invalid_drop = Convert.ToBoolean(reader.Value); break;
                    case "enable_charge_final": enable_charge_final = Convert.ToBoolean(reader.Value); break;
                    case "spirit_name": spirit_name = reader.Value; break;
                    case "attack": attack = Convert.ToInt16(reader.Value); break;
                    case "defense": defense = Convert.ToInt16(reader.Value); break;
                    case "attr": attr = reader.Value; break;
                    case "ability1": ability1 = reader.Value; break;
                    case "ability2": ability2 = reader.Value; break;
                    case "ability3": ability3 = reader.Value; break;
                    case "ability_personal": ability_personal = reader.Value; break;
                }
            }
            return;
        }

        public string GetValueFromName(string name)
        {
            return this.GetType().GetField(name).GetValue(this).ToString();
        }
        public void SetValueFromName(string name, object val)
        {
            this.GetType().GetField(name).SetValue(this, val);
        }

        // No primary key.
        // Battle id is unique identifier.  Main type and sub type fighters.  
        // Battle id should be selectable drop down.  All fighters should be selectable based off battle_id.  
        public string battle_id;
        public string entry_type;
        public bool first_appear;
        public ushort appear_rule_time;
        public ushort appear_rule_count;
        public string fighter_kind;
        public byte color;
        public string mii_hat_id;
        public string mii_body_id;
        public byte mii_color;
        public string mii_voice;
        public byte mii_sp_n;
        public byte mii_sp_s;
        public byte mii_sp_hi;
        public byte mii_sp_lw;
        public byte cpu_lv;
        public string cpu_type;
        public string cpu_sub_type;
        public bool cpu_item_pick_up;
        public byte stock;
        public bool corps;
        public bool _0x0f2077926c;
        public ushort hp;
        public ushort init_damage;
        public string sub_rule;
        public float scale;
        public float fly_rate;
        public bool invalid_drop;
        public bool enable_charge_final;
        // Primary spirit identifier
        public string spirit_name;
        public short attack;
        public short defense;
        public string attr;
        public string ability1;
        public string ability2;
        public string ability3;
        public string ability_personal;
    }
}
