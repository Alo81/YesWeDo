using SmashUltimateEditor.DataTables;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SmashUltimateEditor
{
    public class Battle : DataTbl, IDataTbl
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
                    case "0x18e536d4f7": _0x18e536d4f7 = Convert.ToSByte(reader.Value); break;
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
                    case "0x0d41ef8328": _0x0d41ef8328 = Convert.ToBoolean(reader.Value); break;
                    case "aw_flap_delay": aw_flap_delay = Convert.ToBoolean(reader.Value); break;
                    case "0x0d6f19abae": _0x0d6f19abae = Convert.ToBoolean(reader.Value); break;
                    case "0x18d9441f71": _0x18d9441f71 = reader.Value; break;
                    case "0x18404d4ecb": _0x18404d4ecb = reader.Value; break;
                    case "recommended_skill1": recommended_skill1 = ValuableValue(reader.Value); break;
                    case "recommended_skill2": recommended_skill2 = ValuableValue(reader.Value); break;
                    case "recommended_skill3": recommended_skill3 = ValuableValue(reader.Value); break;
                    case "recommended_skill4": recommended_skill4 = ValuableValue(reader.Value); break;
                    case "recommended_skill5": recommended_skill5 = ValuableValue(reader.Value); break;
                    case "recommended_skill6": recommended_skill6 = ValuableValue(reader.Value); break;
                    case "recommended_skill7": recommended_skill7 = ValuableValue(reader.Value); break;
                    case "recommended_skill8": recommended_skill8 = ValuableValue(reader.Value); break;
                    case "recommended_skill9": recommended_skill9 = ValuableValue(reader.Value); break;
                    case "recommended_skill10": recommended_skill10 = ValuableValue(reader.Value); break;
                    case "recommended_skill11": recommended_skill11 = ValuableValue(reader.Value); break;
                    case "recommended_skill12": recommended_skill12 = ValuableValue(reader.Value); break;
                    case "recommended_skill13": recommended_skill13 = ValuableValue(reader.Value); break;
                    case "un_recommended_skill1": un_recommended_skill1 = ValuableValue(reader.Value); break;
                    case "un_recommended_skill2": un_recommended_skill2 = ValuableValue(reader.Value); break;
                    case "un_recommended_skill3": un_recommended_skill3 = ValuableValue(reader.Value); break;
                    case "un_recommended_skill4": un_recommended_skill4 = ValuableValue(reader.Value); break;
                    case "un_recommended_skill5": un_recommended_skill5 = ValuableValue(reader.Value); break;
                    case "0x0ff8afd14f": _0x0ff8afd14f = reader.Value; break;
                    case "battle_power": battle_power = Convert.ToUInt32(reader.Value); break;
                }
            }
            return;
        }

        [Order]
        public string   battle_id { get; set; }
        [Order]
        public string	battle_type { get; set; }
        [Order]
        public ushort	battle_time_sec { get; set; }
        [Order]
        public ushort	basic_init_damage { get; set; }
        [Order]
        public ushort	basic_init_hp { get; set; }
        [Order]
        public byte	    basic_stock { get; set; }
        [Order]
        public string	ui_stage_id { get; set; }
        [Order]
        public string	stage_type { get; set; }
        [Order]
        public sbyte _0x18e536d4f7 { get; set; }
        [Order]
        public string	stage_bgm { get; set; }
        [Order]
        public bool	    stage_gimmick { get; set; }
        [Order]
        public string	stage_attr { get; set; }
        [Order]
        public string	floor_place_id { get; set; }
        [Order]
        public string	item_table { get; set; }
        [Order]
        public string	item_level { get; set; }
        [Order]
        public string	result_type { get; set; }
        [Order]
        public string	event1_type { get; set; }
        [Order]
        public string	event1_label { get; set; }
        [Order]
        public int		event1_start_time { get; set; }
        [Order]
        public int		event1_range_time { get; set; }
        [Order]
        public byte	    event1_count { get; set; }
        [Order]
        public ushort	event1_damage { get; set; }
        [Order]
        public string	event2_type { get; set; }
        [Order]
        public string	event2_label { get; set; }
        [Order]
        public int		event2_start_time { get; set; }
        [Order]
        public int		event2_range_time { get; set; }
        [Order]
        public byte	    event2_count { get; set; }
        [Order]
        public ushort	event2_damage { get; set; }
        [Order]
        public string	event3_type { get; set; }
        [Order]
        public string	event3_label { get; set; }
        [Order]
        public int		event3_start_time { get; set; }
        [Order]
        public int		event3_range_time { get; set; }
        [Order]
        public byte	    event3_count { get; set; }
        [Order]
        public ushort	event3_damage { get; set; }
        [Order]
        public bool	    _0x0d41ef8328 { get; set; }
        [Order]
        public bool	    aw_flap_delay { get; set; }
        [Order]
        public bool	    _0x0d6f19abae { get; set; }
        [Order]
        public string	_0x18d9441f71 { get; set; }
        [Order]
        public string	_0x18404d4ecb { get; set; }
        [Order]
        public string	recommended_skill1 { get; set; }
        [Order]
        public string	recommended_skill2 { get; set; }
        [Order]
        public string	recommended_skill3 { get; set; }
        [Order]
        public string	recommended_skill4 { get; set; }
        [Order]
        public string	recommended_skill5 { get; set; }
        [Order]
        public string	recommended_skill6 { get; set; }
        [Order]
        public string	recommended_skill7 { get; set; }
        [Order]
        public string	recommended_skill8 { get; set; }
        [Order]
        public string	recommended_skill9 { get; set; }
        [Order]
        public string	recommended_skill10 { get; set; }
        [Order]
        public string	recommended_skill11 { get; set; }
        [Order]
        public string	recommended_skill12 { get; set; }
        [Order]
        public string	recommended_skill13 { get; set; }
        [Order]
        public string	un_recommended_skill1 { get; set; }
        [Order]
        public string	un_recommended_skill2 { get; set; }
        [Order]
        public string	un_recommended_skill3 { get; set; }
        [Order]
        public string	un_recommended_skill4 { get; set; }
        [Order]
        public string	un_recommended_skill5 { get; set; }
        [Order]
        public string	_0x0ff8afd14f { get; set; }
        [Order]
        public uint	    battle_power { get; set; }
    }
}
