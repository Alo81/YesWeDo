namespace YesWeDo.DataTables
{
    public class Stage : DataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "db_root";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "ui_stage_id";
        public static string fieldKey = "ui_stage_id";

        public string ui_stage_id { get; set; }
        public string name_id { get; set; }
    }
}
