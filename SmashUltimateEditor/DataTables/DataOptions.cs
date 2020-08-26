using System;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor.DataTables
{
    public class DataOptions
    {
        // Try this more?
        public List<DataTbl> dataList;
        public List<string> GetOptionsFromName(string name)
        {
            return (List<string>)GetType().GetProperty(name).GetValue(this) ?? new List<string>();
        }
    }
}
