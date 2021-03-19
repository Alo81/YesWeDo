namespace YesWeDo.DataTables
{
    public class Bgm : DataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "db_root";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "ui_bgm_id";
        public static string fieldKey = "ui_bgm_id";

        public string ui_bgm_id { get; set; }
        public string name_id { get; set; }
    }
}
