using System;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db
{
    public class SpiritFighter : DataTbl, IDataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "db_root";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "ui_spirit_id";



        [Order]
        public string ui_spirit_id { get; set; }
        [Order]
        public string chara_id { get; set; }
        [Order]
        public byte chara_color { get; set; }
        [Order]
        public bool _0x0fe61a19ad { get; set; }
        [Order]
        public string _0x15e7618b71 { get; set; }
    }
}
