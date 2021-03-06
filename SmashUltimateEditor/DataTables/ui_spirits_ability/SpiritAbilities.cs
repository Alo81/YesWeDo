﻿namespace YesWeDo.DataTables
{
    public class SpiritAbilities : DataTbl
    {
        internal static string XML_NAME = "db_root";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "ui_spirits_ability_id";
        public static string fieldKey = "ui_spirits_ability_id";

        [Order]
        public string ui_spirits_ability_id { get; set; }
        [Order][OriginalType("string")]
        public string ability_name { get; set; }
    }
}
