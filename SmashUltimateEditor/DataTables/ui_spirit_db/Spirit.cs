using SmashUltimateEditor;
using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace YesWeDo.DataTables.ui_spirit_db
{
    public class Spirit : DataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "data_version";


        public static TabPage BuildEmptyPage(DataTbls dataTbls)
        {
            return DataTbl.BuildEmptyPage(dataTbls, typeof(Spirit));
        }

        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public ushort save_no { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string ui_spirit_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string name_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public ushort fixed_no { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public bool is_bcat { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public bool is_dlc { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public ushort directory_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string ui_series_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string rank { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte slot_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string ability_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string attr { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint exp_lv_max { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public float exp_up_rate { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public short base_attack { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public short max_attack { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public short base_defense { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public short max_defense { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string grow_type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte personality { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string evolve_src { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string super_ability { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public bool is_board_appear { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string fighter_conditions { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string appear_conditions { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint _0x14ef76fcd7 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint _0x1531b0c6f0 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public float _0x16db57210b { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint reward_capacity { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint battle_exp { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint summon_sp { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string summon_item1 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte summon_item1_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string summon_item2 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte summon_item2_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string summon_item3 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte summon_item3_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string summon_item4 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte summon_item4_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string summon_item5 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public byte summon_item5_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string _0x0ccad0f7b2 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string shop_sales_type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public uint shop_price { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public bool _0x11730b0c1d { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string _0x13656d7462 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string check_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.SpiritDetails)]
        public string _0x115044521b { get; set; }
    }
}
