using System.Collections.Generic;
using YesWeDo.DataTables;

namespace YesWeDo.DataTableCollections
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
