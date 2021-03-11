using SmashUltimateEditor;
using SmashUltimateEditor.DataTableCollections;
using SmashUltimateEditor.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YesWeDo.DataTableCollections
{
    public class SpiritBoardDataOptions : BaseDataOptions, IDataOptions
    {
        private List<SpiritBoard> _dataList;

        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }
        internal static Type underlyingType = typeof(SpiritBoard);

        public static Type GetUnderlyingType()
        {
            return underlyingType;
        }
        public void SetData(List<IDataTbl> inSpiritBoard)
        {
            _dataList = inSpiritBoard.OfType<SpiritBoard>().ToList();
        }

        public int GetCount()
        {
            return _dataList.Count;
        }
        public bool HasData()
        {
            return GetCount() > 0;
        }

        public void AddSpiritToBoard(string label, string battle_id)
        {
            var result = _dataList.FirstOrDefault(x => x.label == label);

            if(result != default(SpiritBoard))
            {
                result.spirit_list.Add(battle_id);
            }
        }

        public List<string> spirit_name
        {
            get { return _dataList.Select(x => x.label).Distinct().OrderBy(x => x).ToList(); }
        }

        public List<string> spirit_list_battles     //All spirits that are already added to a spirit board
        {
            get 
            {
                var results = new List<string>();
                foreach(var spirits in _dataList.Select(x => x.spirit_list))
                {
                    results.AddRange(spirits);
                }
                return results.Distinct().OrderBy(x => x).ToList(); 
            }
        }
    }
}
