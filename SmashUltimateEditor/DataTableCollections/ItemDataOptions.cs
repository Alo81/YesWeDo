using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.DataTables.ui_item_db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public class ItemDataOptions : BaseDataOptions, IDataOptions
    {
        public List<Item> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }
        internal static Type underlyingType = typeof(Item);

        public ItemDataOptions()
        {
            _dataList = new List<Item>();
        }
        public void SetData(List<IDataTbl> inData)
        {
            _dataList = inData.OfType<Item>().ToList();
        }
        public static Type GetUnderlyingType()
        {
            return underlyingType;
        }
        public int GetCount()
        {
            return _dataList.Count;
        }

        // There could be overlap on name_id.  Worth considering.  
        public Item GetItem(string name)
        {
            return _dataList.Where(x => x.GetFieldValueFromName("name_id") == name).FirstOrDefault();
        }

        public List<Item> GetItemsOfName(string name)
        {
            return _dataList.Where(x => x.GetFieldValueFromName("name_id") == name).ToList();
        }

        public int GetItemIndex(Item item)
        {
            return GetItems().FindIndex(x => x == item);
        }
        public Item GetItemAtIndex(int index)
        {
            return _dataList?[index];
        }
        public List<Item> GetItems()
        {
            return _dataList;
        }

        public void AddItem(Item item)
        {
            _dataList.Add(item);
        }
        public void SetItems(List<Item> items)
        {
            _dataList = items;
        }

        public void ReplaceItems(ItemDataOptions replacement)
        {
            foreach (var replItem in replacement.GetItems())
            {
                _dataList[GetItemIndex(replItem)] = replItem;
            }
        }
        public void ReplaceItemAtIndex(int index, Item item)
        {
            _dataList[index] = item;
        }
        public Type GetContainerType()
        {
            return _dataList[0].GetType();
        }

        public List<ItemEvent> GetAsEvents()
        {
            var itemEvents = new List<ItemEvent>();

            foreach(Item item in GetItems().Where(x => name_id.Contains(x.name_id)))
            {
                itemEvents.Add(
                    new ItemEvent()
                    {
                        label = item.name_id,
                        item_id = item.ui_item_id,
                        variation = 0,
                        num = 5
                    }
                    );
            }

            return itemEvents;
        }

        public List<string> name_id
        {
            get { return _dataList.Where(x => !(pokemon.Contains(x.name_id) || assist.Contains(x.name_id))).Select(x => x.name_id).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> pokemon
        {
            get { return _dataList.Where(x => x.category == "category_pokemon").Select(x => x.name_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> assist
        {
            get { return _dataList.Where(x => x.category == "category_assist").Select(x => x.name_id).Distinct().OrderBy(x => x).ToList(); }
        }
    }
}
