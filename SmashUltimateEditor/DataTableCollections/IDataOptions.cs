using System;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public interface IDataOptions
    {
        public List<IDataTbl> dataList { get;}

        public void SetData(List<IDataTbl> inData);
    }
}
