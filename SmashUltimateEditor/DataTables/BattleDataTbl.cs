using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static SmashUltimateEditor.Extensions;

namespace SmashUltimateEditor
{
    public class BattleDataTbl : IDataTbl
    {
        public void BuildFromXml(XmlReader reader)
        {
            string attribute;
            while (reader.Read())
            {
                while(! (reader.NodeType == XmlNodeType.Element))
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
                    case "battle_type": battle_type = reader.Value; break;
                    case "battle_time_sec": battle_time_sec = Convert.ToUInt16((reader.Value)); break;
                    case "basic_init_damage": basic_init_damage = Convert.ToUInt16(reader.Value); break;
                    case "basic_init_hp": basic_init_hp = Convert.ToUInt16(reader.Value); break;
                    case "basic_stock": basic_stock = Convert.ToByte(reader.Value); break;
                    case "ui_stage_id": ui_stage_id = reader.Value; break;
                    case "stage_type": stage_type = reader.Value; break;
                    case "x018e536d4f7": _0x18e536d4f7 = Convert.ToSByte(reader.Value); break;
                    case "stage_bgm": stage_bgm = reader.Value; break;
                    case "stage_gimmick": stage_gimmick = Convert.ToBoolean(reader.Value); break;
                    case "stage_attr": stage_attr = reader.Value; break;
                    case "floor_place_id": floor_place_id = reader.Value; break;
                    case "item_table": item_table = reader.Value; break;
                    case "item_level": item_level = reader.Value; break;
                    case "result_type": result_type = reader.Value; break;
                    case "event1_type": event1_type = reader.Value; break;
                    case "event1_label": event1_label = reader.Value; break;
                    case "event1_start_time": event1_start_time = Convert.ToInt32(reader.Value); break;
                    case "event1_range_time": event1_range_time = Convert.ToInt32(reader.Value); break;
                    case "event1_count": event1_count = Convert.ToByte(reader.Value); break;
                    case "event1_damage": event1_damage = Convert.ToUInt16(reader.Value); break;
                    case "event2_type": event2_type = reader.Value; break;
                    case "event2_label": event2_label = reader.Value; break;
                    case "event2_start_time": event2_start_time = Convert.ToInt32(reader.Value); break;
                    case "event2_range_time": event2_range_time = Convert.ToInt32(reader.Value); break;
                    case "event2_count": event2_count = Convert.ToByte(reader.Value); break;
                    case "event2_damage": event2_damage = Convert.ToUInt16(reader.Value); break;
                    case "event3_type": event3_type = reader.Value; break;
                    case "event3_label": event3_label = reader.Value; break;
                    case "event3_start_time": event3_start_time = Convert.ToInt32(reader.Value); break;
                    case "event3_range_time": event3_range_time = Convert.ToInt32(reader.Value); break;
                    case "event3_count": event3_count = Convert.ToByte(reader.Value); break;
                    case "event3_damage": event3_damage = Convert.ToUInt16(reader.Value); break;
                    case "x00d41ef8328": _0x0d41ef8328 = Convert.ToBoolean(reader.Value); break;
                    case "aw_flap_delay": aw_flap_delay = Convert.ToBoolean(reader.Value); break;
                    case "x00d6f19abae": _0x0d6f19abae = Convert.ToBoolean(reader.Value); break;
                    case "x018d9441f71": _0x18d9441f71 = reader.Value; break;
                    case "x018404d4ecb": _0x18404d4ecb = reader.Value; break;
                    case "recommended_skill1": recommended_skill1 = reader.Value; break;
                    case "recommended_skill2": recommended_skill2 = reader.Value; break;
                    case "recommended_skill3": recommended_skill3 = reader.Value; break;
                    case "recommended_skill4": recommended_skill4 = reader.Value; break;
                    case "recommended_skill5": recommended_skill5 = reader.Value; break;
                    case "recommended_skill6": recommended_skill6 = reader.Value; break;
                    case "recommended_skill7": recommended_skill7 = reader.Value; break;
                    case "recommended_skill8": recommended_skill8 = reader.Value; break;
                    case "recommended_skill9": recommended_skill9 = reader.Value; break;
                    case "recommended_skill10": recommended_skill10 = reader.Value; break;
                    case "recommended_skill11": recommended_skill11 = reader.Value; break;
                    case "recommended_skill12": recommended_skill12 = reader.Value; break;
                    case "recommended_skill13": recommended_skill13 = reader.Value; break;
                    case "un_recommended_skill1": un_recommended_skill1 = reader.Value; break;
                    case "un_recommended_skill2": un_recommended_skill2 = reader.Value; break;
                    case "un_recommended_skill3": un_recommended_skill3 = reader.Value; break;
                    case "un_recommended_skill4": un_recommended_skill4 = reader.Value; break;
                    case "un_recommended_skill5": un_recommended_skill5 = reader.Value; break;
                    case "x00ff8afd14f": _0x0ff8afd14f = reader.Value; break;
                    case "battle_power": battle_power = Convert.ToUInt32(reader.Value); break;
                }
            }
            return;
        }

        public string GetValueFromName(string name)
        {
            if(name == "_0x18d9441f71")
            {
                return this.GetType().GetField(name).GetValue(this)?.ToString() ?? "";
            }
            return this.GetType().GetField(name).GetValue(this)?.ToString() ?? "";
        }

        public string battle_id;
        public string	battle_type;
        public ushort	battle_time_sec;
        public ushort	basic_init_damage;
        public ushort	basic_init_hp;
        public byte	    basic_stock;
        public string	ui_stage_id;
        public string	stage_type;
        public string	stage_bgm;
        public bool	    stage_gimmick;
        public string	stage_attr;
        public string	floor_place_id;
        public string	item_table;
        public string	item_level;
        public string	result_type;
        public string	event1_type;
        public string	event1_label;
        public int		event1_start_time;
        public int		event1_range_time;
        public byte	    event1_count;
        public ushort	event1_damage;
        public string	event2_type;
        public string	event2_label;
        public int		event2_start_time;
        public int		event2_range_time;
        public byte	    event2_count;
        public ushort	event2_damage;
        public string	event3_type;
        public string	event3_label;
        public int		event3_start_time;
        public int		event3_range_time;
        public byte	    event3_count;
        public ushort	event3_damage;
        public sbyte    _0x18e536d4f7;
        public bool	    _0x0d41ef8328;
        public bool	    aw_flap_delay;
        public bool	    _0x0d6f19abae;
        public string	_0x18d9441f71;
        public string	_0x18404d4ecb;
        public string	recommended_skill1;
        public string	recommended_skill2;
        public string	recommended_skill3;
        public string	recommended_skill4;
        public string	recommended_skill5;
        public string	recommended_skill6;
        public string	recommended_skill7;
        public string	recommended_skill8;
        public string	recommended_skill9;
        public string	recommended_skill10;
        public string	recommended_skill11;
        public string	recommended_skill12;
        public string	recommended_skill13;
        public string	un_recommended_skill1;
        public string	un_recommended_skill2;
        public string	un_recommended_skill3;
        public string	un_recommended_skill4;
        public string	un_recommended_skill5;
        public string	_0x0ff8afd14f;
        public uint	    battle_power;
    }
}
