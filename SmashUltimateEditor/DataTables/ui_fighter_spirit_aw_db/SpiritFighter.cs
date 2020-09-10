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

    }
}
