using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YesweDo;
using YesWeDo.DataTables;

namespace YesWeDo.DataTableCollections
{
    public class DataOptions : BaseDataOptions, IDataOptions//, IXmlType
    {
        //public ParamType TypeKey { get; } = ParamType.list;
        // Try this more?
        public List<IDataTbl> _dataList;
        public List<IDataTbl> dataList { get { return _dataList; } }
        public static Type underlyingType = typeof(IDataTbl);

        public DataOptions()
        {
            _dataList = new List<IDataTbl>();
        }
        public static Type GetUnderlyingType()
        {
            return underlyingType;
        }
        public void AddDataTbl(IDataTbl tbl)
        {
            dataList.Add(tbl);
        }
        public void AddRangeDataTbl(IEnumerable<IDataTbl> tbl)
        {
            dataList.AddRange(tbl);
        }
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
            _dataList = inData.OfType<IDataTbl>().ToList();
        }

        public void SetBattleIdsForAll(string battle_id)
        {
            foreach(var dataTbl in dataList)
            {
                dataTbl.SetValueFromName("battle_id", battle_id);
            }
        }

        public List<IDataTbl> GetOfType(Type type)
        {
            return dataList.Where(x => (x?.GetType()?.IsAssignableFrom(type) ?? false) || (x?.GetType()?.IsSubclassOf(type) ?? false)).ToList();
        }

        public IDataOptions GetDataOptionsFromUnderlyingType(Type underlyingType)
        {
            if (underlyingType is null)
            {
                return null;
            }

            var type = typeof(IDataOptions);

            var child = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass
                                    && type.IsAssignableFrom(t) && t != type
                                    && (Type)t?.GetField("underlyingType", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                                    ?.GetValue(null) == underlyingType) ?? null;
            if (child is null || child.Count() == 0)
            {
                return null;
            }
            else
            {
                var options = (IDataOptions)Activator.CreateInstance(child.First());
                options.SetData(GetOfType(underlyingType));
                return options;
            }
        }

        public IEnumerable<IDataTbl> GetItemsOfType(Type type)
        {
            return _dataList.Where(x => x.GetType() == type);
        }
        public bool ContainsItemsOfType(Type type)
        {
            return _dataList.Exists(x => x.GetType().IsAssignableFrom(type));
        }
        public bool ContainsItemsOfUnderlyingType(Type type)
        {
            return GetDataOptionsFromUnderlyingType(type).GetCount() > 0;
        }
        public List<Type> GetContainerTypes()
        {
            return dataList.Select(x => x.GetType()).Distinct().ToList();
        }
    }
}
