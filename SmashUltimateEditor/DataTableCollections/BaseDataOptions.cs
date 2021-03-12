using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static SmashUltimateEditor.DataTables.DataTbl;

namespace SmashUltimateEditor.DataTableCollections
{
    public class BaseDataOptions : IDataOptions
    {
        public IList<IDataTbl> _dataList
        {
            get
            {
                return (IList<IDataTbl>)GetType().GetProperty("dataList").GetValue(this);
            }
        }
        public List<IDataTbl> dataList { get; set; }

        public int GetCount()
        {
            return dataList.Count;
        }

        public bool HasData()
        {
            return GetCount() > 0;
        }

        public void SetData(List<IDataTbl> inData)
        {
            dataList = inData;
        }

        public IList<IDataTbl> GetData()
        {
            return _dataList;
        }

        public int GetItemIndex(IDataTbl item)
        {
            return _dataList.IndexOf(item);
        }
        public void ReplaceItemAtIndex(IDataTbl item, int index)
        {
            _dataList[index] = item;
        }
        public IEnumerable<object> GetPropertyValuesFromName(string name)
        {
            return _dataList.Select(x => x.GetPropertyValueFromName(name)).Distinct().OrderBy(x => x);
        }

        public List<string> GetOptionsFromName(string name)
        {
            // TURN EVERYTHING TO STRINGS
            var list = GetPropertyValuesFromName(name);
            if (list.GetType() != typeof(List<string>))
            {
                var newList = new List<string>();
                foreach (var item in (IEnumerable)list)
                {
                    newList.Add(    (   (string) Convert.ChangeType(item, typeof(string)    )).ToLower()   );
                }
                list = newList;
            }
            return (List<string>)list;
        }
    }
}
