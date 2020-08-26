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
        public XElement GetAsXElement(int index)
        {
            return new XElement("struct",
            new XAttribute("index", index),
                //<hash40 hash="battle_id">default</hash40>	// <*DataListItem.Type* hash="*DataListItem.FieldName*">*DataListItem.FieldValue*</>
                this.GetType().GetProperties().OrderBy(x => ((OrderAttribute)x.GetCustomAttributes(typeof(OrderAttribute), false).Single()).Order).Select(property =>
               new XElement(property.PropertyType.Name,
               new XAttribute("hash", DataParse.NameFixer(property.Name)), this.GetValueFromName(property.Name))
                    )
                );
        }

        public void FighterCheck(List<string> options, ref Random rnd)
        {
            options.RemoveAll(x => Defs.EXCLUDED_FIGHTERS.Contains(x));
            while (Defs.EXCLUDED_FIGHTERS.Contains(fighter_kind))
            {
                fighter_kind = options[rnd.Next(options.Count)];
            }
        }

        public void StockCheck(int fighterCount)
        {
            // If there are a lot, makes multiple lives tedius
            if (fighterCount > 3)
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

        public void EntryCheck(bool isMain)
        {
            if(isMain)
                entry_type = "main_type";
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
                    case "ability1": ability1 = ValuableValue(reader.Value); break;
                    case "ability2": ability2 = ValuableValue(reader.Value); break;
                    case "ability3": ability3 = ValuableValue(reader.Value); break;
                    case "ability_personal": ability_personal = ValuableValue(reader.Value); break;
                }
            }
            return;
        }

        public Fighter ShallowCopy()
        {
            return (Fighter)this.MemberwiseClone();
        }

        // No primary key.
        // Battle id is unique identifier.  Main type and sub type fighters.  
        // Battle id should be selectable drop down.  All fighters should be selectable based off battle_id.  
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
