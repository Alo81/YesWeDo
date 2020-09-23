using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.Helpers
{
    class StatsHelper
    {
        public static void GetCountPerField(DataTbls tbl, string field)
        {
            var battles = tbl.battleData.GetBattles();

            var results = battles.GroupBy(x => x.event1_type)
              .Select(g => new
              {
                  Field = g.Key,
                  Count = g.Select(l => l.event1_type).Count()
              }).OrderByDescending(x => x.Count);
        }
    }
}
