using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor
{
    class BattleDataTbls
    {
        public List<BattleDataTbl> battleDataList;

        public List<string> battle_ids
        {
            get { return battleDataList.Select(x => x.battle_id).OrderBy(x => x).ToList(); }
        }
        public string battle_type;
        public ushort battle_time_sec;
        public ushort basic_init_damage;
        public ushort basic_init_hp;
        public byte basic_stock;
        public string ui_stage_id;
        public string stage_type;
        public sbyte _0x18e536d4f7;
        public string stage_bgm;
        public bool stage_gimmick;
        public string stage_attr;
        public string floor_place_id;
        public string item_table;
        public string item_level;
        public string result_type;
        public string event1_type;
        public string event1_label;
        public int event1_start_time;
        public int event1_range_time;
        public byte event1_count;
        public ushort event1_damage;
        public string event2_type;
        public string event2_label;
        public int event2_start_time;
        public int event2_range_time;
        public byte event2_count;
        public ushort event2_damage;
        public string event3_type;
        public string event3_label;
        public int event3_start_time;
        public int event3_range_time;
        public byte event3_count;
        public ushort event3_damage;
        public bool _0x0d41ef8328;
        public bool aw_flap_delay;
        public bool _0x0d6f19abae;
        public string _0x18d9441f71;
        public string _0x18404d4ecb;
        public string recommended_skill1;
        public string recommended_skill2;
        public string recommended_skill3;
        public string recommended_skill4;
        public string recommended_skill5;
        public string recommended_skill6;
        public string recommended_skill7;
        public string recommended_skill8;
        public string recommended_skill9;
        public string recommended_skill10;
        public string recommended_skill11;
        public string recommended_skill12;
        public string recommended_skill13;
        public string un_recommended_skill1;
        public string un_recommended_skill2;
        public string un_recommended_skill3;
        public string un_recommended_skill4;
        public string un_recommended_skill5;
        public string _0x0ff8afd14f;
        public uint battle_power;
    }
}
