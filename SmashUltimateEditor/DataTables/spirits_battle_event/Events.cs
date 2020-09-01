using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SmashUltimateEditor.DataTables
{
    public class Events : DataTbl, IDataTbl
    {
        [Order]
        public string label { get; set; }
        [Order]
        public bool is_player_target { get; set; }
        [Order]
        public bool is_enemy_target { get; set; }
        [Order]
        public string power_type { get; set; }
        [Order]
        public int value { get; set; }
    }
}
