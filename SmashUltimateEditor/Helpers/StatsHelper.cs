using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static IEnumerable<string> GetBattlesWhereFieldIsValue(IDataOptions tbl, string field, string val)
        {
            var results = tbl.dataList.Where(x => x.GetPropertyValueFromName(field) == val);
            var battleIdStrings = results.Select(x => x.GetPropertyValueFromName("battle_id"));

            return battleIdStrings;
        }
        public static void GetNamesPerType(Type type)
        {
            var dataTblTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass
                                    && type.IsSubclassOf(typeof(DataTbl)));

            var dataTblPropertiesMatchingType = dataTblTypes.Select(x => x.GetProperties().Where(y => y.PropertyType == type));

            var results = dataTblPropertiesMatchingType.GroupBy(x => x.Select(y => y.Name));
        }
    }
}
