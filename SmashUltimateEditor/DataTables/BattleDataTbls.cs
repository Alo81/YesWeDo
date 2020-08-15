﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor
{
    class BattleDataTbls
    {
        public List<BattleDataTbl> battleDataList;

        public List<string> battle_id
        {
            get { return battleDataList.Select(x => x.battle_id).OrderBy(x => x).ToList(); }
        }

        public List<string>battle_type

        {
            get { return battleDataList.Select(x => x.battle_type).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>battle_time_sec
        

        {
            get { return battleDataList.Select(x => x.battle_time_sec).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>basic_init_damage
        

        {
            get { return battleDataList.Select(x => x.basic_init_damage).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>basic_init_hp
        

        {
            get { return battleDataList.Select(x => x.basic_init_hp).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<byte>basic_stock
        

        {
            get { return battleDataList.Select(x => x.basic_stock).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>ui_stage_id
        

        {
            get { return battleDataList.Select(x => x.ui_stage_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_type
        

        {
            get { return battleDataList.Select(x => x.stage_type).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<sbyte>_0x18e536d4f7
        

        {
            get { return battleDataList.Select(x => x._0x18e536d4f7).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_bgm
        

        {
            get { return battleDataList.Select(x => x.stage_bgm).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<bool>stage_gimmick
        

        {
            get { return battleDataList.Select(x => x.stage_gimmick).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>stage_attr
        

        {
            get { return battleDataList.Select(x => x.stage_attr).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>floor_place_id
        

        {
            get { return battleDataList.Select(x => x.floor_place_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>item_table
        

        {
            get { return battleDataList.Select(x => x.item_table).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>item_level
        

        {
            get { return battleDataList.Select(x => x.item_level).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>result_type
        

        {
            get { return battleDataList.Select(x => x.result_type).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>event_type
        

        {
            get { return battleDataList.Select(x => x.event1_type).Distinct().OrderBy(x => x).ToList(); 
            }
        }

        public List<string>event_label
        

        {
            get { return battleDataList.Select(x => x.event1_label).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<int>event_start_time
        

        {
            get { return battleDataList.Select(x => x.event1_start_time).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<int>event_range_time
        

        {
            get { return battleDataList.Select(x => x.event1_range_time).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<byte>event_count
        

        {
            get { return battleDataList.Select(x => x.event1_count).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<ushort>event_damage
        

        {
            get { return battleDataList.Select(x => x.event1_damage).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<bool>_0x0d41ef8328
        

        {
            get { return battleDataList.Select(x => x._0x0d41ef8328).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<bool>aw_flap_delay
        

        {
            get { return battleDataList.Select(x => x.aw_flap_delay).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<bool>_0x0d6f19abae
        

        {
            get { return battleDataList.Select(x => x._0x0d6f19abae).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>_0x18d9441f71
        

        {
            get { return battleDataList.Select(x => x._0x18d9441f71).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>_0x18404d4ecb
        

        {
            get { return battleDataList.Select(x => x._0x18404d4ecb).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>recommended_skill
        

        {
            get { return battleDataList.Select(x => x.recommended_skill1).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>un_recommended_skill
        

        {
            get { return battleDataList.Select(x => x.un_recommended_skill1).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string>_0x0ff8afd14f


        {
            get { return battleDataList.Select(x => x._0x0ff8afd14f).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<uint> battle_power
        {
            get { return battleDataList.Select(x => x.battle_power).Distinct().OrderBy(x => x).ToList(); }
        }


    }
}
