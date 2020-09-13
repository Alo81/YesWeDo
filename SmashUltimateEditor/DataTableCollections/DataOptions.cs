using paracobNET;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db;
using SmashUltimateEditor.DataTables.ui_item_db;
using SmashUltimateEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static SmashUltimateEditor.DataTables.DataTbl;

namespace SmashUltimateEditor.DataTables
{
    public class DataOptions : BaseDataOptions, IDataOptions//, IXmlType
    {
        //public ParamType TypeKey { get; } = ParamType.list;
        // Try this more?
        public List<IDataTbl> _dataList;
        public List<IDataTbl> dataList { get { return _dataList; } }

        public DataOptions()
        {
            _dataList = new List<IDataTbl>();
        }
        public void AddDataTbl(IDataTbl tbl)
        {
            dataList.Add(tbl);
        }
        public int GetCount()
        {
            return dataList.Count;
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
            return dataList.Where(x => x.GetType() == type).ToList();
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
        public List<Item> GetItems()
        {
            return dataList.OfType<Item>().ToList();
        }
        public List<SpiritFighter> GetSpiritFighters()
        {
            return dataList.OfType<SpiritFighter>().ToList();
        }
        public List<Type> GetContainerTypes()
        {
            return dataList.Select(x => x.GetType()).Distinct().ToList();
        }
    }
}
