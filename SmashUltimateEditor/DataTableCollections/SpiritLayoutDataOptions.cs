using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesWeDo.DataTables;

namespace YesWeDo.DataTableCollections
{
    public class SpiritLayoutDataOptions : BaseDataOptions, IDataOptions
    {
        public List<SpiritLayout> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }
        internal static Type underlyingType = typeof(SpiritLayout);

        public SpiritLayoutDataOptions()
        {
            _dataList = new List<SpiritLayout>();
        }
        public void SetData(List<IDataTbl> inData)
        {
            _dataList = inData.OfType<SpiritLayout>().ToList();
        }
        public static Type GetUnderlyingType()
        {
            return underlyingType;
        }
        public int GetCount()
        {
            return _dataList.Count;
        }
        public bool HasData()
        {
            return GetCount() > 0;
        }
    }
}
