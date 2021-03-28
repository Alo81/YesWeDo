using System;
using System.Collections.Generic;
using System.Text;

namespace YesWeDo.DataTables
{
    public class ExportedBattle
    {
        public Battle battle { get; set; }
        public List<Fighter> fighters { get; set; }
        public Spirit spirit { get; set; }
    }
}
