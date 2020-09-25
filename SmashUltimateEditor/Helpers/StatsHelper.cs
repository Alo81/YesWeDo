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
            var fighters = tbl.fighterData.GetFighters();

            var battleStats = battles.GroupBy(x => x.GetPropertyValueFromName(field))
              .Select(g => new
              {
                  Field = g.Key,
                  Count = g.Select(l => l.GetPropertyValueFromName(field)).Count()
              }).OrderByDescending(x => x.Count);

            if(battleStats.Count() <= 1)
            {
                var fighterStats = fighters.GroupBy(x => x.GetPropertyValueFromName(field))
                  .Select(g => new
                  {
                      Field = g.Key,
                      Count = g.Select(l => l.GetPropertyValueFromName(field)).Count()
                  }).OrderByDescending(x => x.Count);
            }
        }
    }
}
