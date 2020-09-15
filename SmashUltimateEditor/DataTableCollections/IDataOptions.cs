using System.Collections.Generic;

namespace SmashUltimateEditor.DataTableCollections
{
    public interface IDataOptions
    {
        public List<IDataTbl> dataList { get;}

        public void SetData(List<IDataTbl> inData);
    }
}
