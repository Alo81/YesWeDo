namespace YesWeDo.DataTables
{
    public class Item : DataTbl
    {

        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "db_root";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "ui_item_id";

        [Order]
        public string ui_item_id { get; set; }
        [Order]
        public string name_id { get; set; }
        [Order]
        public string category { get; set; }
        [Order]
        public string item_kind { get; set; }
        [Order]
        public bool master_ball { get; set; }
        [Order]
        public short switch_disp_order { get; set; }
        [Order]
        public bool use_switch { get; set; }
        [Order]
        public short training_disp_order { get; set; }
        [Order]
        public bool use_training { get; set; }
        [Order]
        public short save_no { get; set; }
    }
}
