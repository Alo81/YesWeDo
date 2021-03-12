using System;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public interface IDataOptions
    {
        public List<IDataTbl> dataList { get;}

        public int GetCount();
        public bool HasData();
        public void SetData(List<IDataTbl> inData);
        public IEnumerable<object> GetPropertyValuesFromName(string name);
    }
}
