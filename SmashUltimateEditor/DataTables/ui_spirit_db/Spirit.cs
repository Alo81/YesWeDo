using System.Windows.Forms;
using YesweDo;

namespace YesWeDo.DataTables
{
    public class Spirit : DataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "data_version";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "db_root";


        public static TabPage BuildEmptyPage(DataTbls dataTbls)
        {
            return DataTbl.BuildEmptyPage(dataTbls, typeof(Spirit));
        }

        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public ushort save_no { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string ui_spirit_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)][OriginalType("string")]
        public string name_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public ushort fixed_no { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public bool is_bcat { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public bool is_dlc { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public ushort directory_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string ui_series_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string rank { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public byte slot_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string ability_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string attr { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public uint exp_lv_max { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public float exp_up_rate { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public short base_attack { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public short max_attack { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public short base_defense { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public short max_defense { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        public string grow_type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        public byte personality { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        public string evolve_src { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        public string super_ability { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public bool is_board_appear { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string fighter_conditions { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string appear_conditions { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public uint _0x14ef76fcd7 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public uint _0x1531b0c6f0 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public float _0x16db57210b { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public uint reward_capacity { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public uint battle_exp { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public uint summon_sp { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item1 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item1_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item2 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item2_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item3 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item3_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item4 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item4_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item5 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item5_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public string _0x0ccad0f7b2 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string shop_sales_type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public uint shop_price { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public bool _0x11730b0c1d { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public string _0x13656d7462 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string check_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public string _0x115044521b { get; set; }
    }
}
