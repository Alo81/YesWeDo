using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.Helpers
{
    class StatsHelper
    {
        public static void GetCountPerField(IDataOptions tbl, string field)
        {
            var results = tbl.dataList.GroupBy(x => x.GetPropertyValueFromName(field))
              .Select(g => new
              {
                  Field = g.Key,
                  Count = g.Select(l => l.GetPropertyValueFromName(field)).Count()
              }).OrderByDescending(x => x.Count);
        }
    }
}
