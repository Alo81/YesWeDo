using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesWeDo.DataTables
{
    public class SpiritLayout : DataTbl
    {
        internal static string XML_NAME = "db_root";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "ui_spirit_layout_id";

        public string ui_spirit_layout_id { get; set; }
        [Order]
        public float ui_art_center_px_x { get; set; }
        [Order]
        public float ui_art_center_px_y { get; set; }
        [Order]
        public float ui_art_scale { get; set; }
        [Order]
        public float ui_art_on_stand_center_px_x { get; set; }
        [Order]
        public float ui_art_on_stand_center_px_y { get; set; }
        [Order]
        public float ui_art_on_stand_scale { get; set; }
        [Order]
        public float ui_art_type_under_center_px_x { get; set; }
        [Order]
        public float ui_art_type_under_center_px_y { get; set; }
        [Order]
        public float ui_art_type_under_scale { get; set; }
        [Order]
        public float ui_art_chara_sel_center_px_x { get; set; }
        [Order]
        public float ui_art_chara_sel_center_px_y { get; set; }
        [Order]
        public float ui_art_chara_sel_scale { get; set; }
        [Order]
        public uint effect_num { get; set; }
        [Order]
        public int effect_pos_0_x { get; set; }
        [Order]
        public int effect_pos_0_y { get; set; }
        [Order]
        public int effect_pos_1_x { get; set; }
        [Order]
        public int effect_pos_1_y { get; set; }
        [Order]
        public int effect_pos_2_x { get; set; }
        [Order]
        public int effect_pos_2_y { get; set; }
        [Order]
        public int effect_pos_3_x { get; set; }
        [Order]
        public int effect_pos_3_y { get; set; }
        [Order]
        public int effect_pos_4_x { get; set; }
        [Order]
        public int effect_pos_4_y { get; set; }
        [Order]
        public int effect_pos_5_x { get; set; }
        [Order]
        public int effect_pos_5_y { get; set; }
        [Order]
        public int effect_pos_6_x { get; set; }
        [Order]
        public int effect_pos_6_y { get; set; }
        [Order]
        public int effect_pos_7_x { get; set; }
        [Order]
        public int effect_pos_7_y { get; set; }
        [Order]
        public int effect_pos_8_x { get; set; }
        [Order]
        public int effect_pos_8_y { get; set; }
        [Order]
        public int effect_pos_9_x { get; set; }
        [Order]
        public int effect_pos_9_y { get; set; }
        [Order]
        public int effect_pos_10_x { get; set; }
        [Order]
        public int effect_pos_10_y { get; set; }
        [Order]
        public int effect_pos_11_x { get; set; }
        [Order]
        public int effect_pos_11_y { get; set; }
        [Order]
        public int effect_pos_12_x { get; set; }
        [Order]
        public int effect_pos_12_y { get; set; }
        [Order]
        public int effect_pos_13_x { get; set; }
        [Order]
        public int effect_pos_13_y { get; set; }
        [Order]
        public int effect_pos_14_x { get; set; }
        [Order]
        public int effect_pos_14_y { get; set; }
    }
}
