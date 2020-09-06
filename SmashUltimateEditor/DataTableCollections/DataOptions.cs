using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.DataTables
{
    public class DataOptions
    {
        // Try this more?
        public List<IDataTbl> dataList;

        public DataOptions()
        {
            dataList = new List<IDataTbl>();
        }

        public void AddDataTbl(IDataTbl tbl)
        {
            dataList.Add(tbl);
        }

        public List<string> GetOptionsFromName(string name)
        {
            return (List<string>)GetType().GetProperty(name).GetValue(this) ?? new List<string>();
        }

        public int GetCount()
        {
            return dataList.Count;
        }
        public List<Battle> GetBattles()
        {
            return dataList.OfType<Battle>().ToList();
        }
        public List<Fighter> GetFighters()
        {
            return dataList.OfType<Fighter>().ToList();
        }
        public List<Event> GetEvents()
        {
            return dataList.OfType<Event>().ToList();
        }
    }
}
