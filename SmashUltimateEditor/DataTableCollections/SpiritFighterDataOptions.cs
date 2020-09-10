using SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public class SpiritFighterDataOptions
    {
        public List<SpiritFighter> _dataList;

        public SpiritFighterDataOptions()
        {
            _dataList = new List<SpiritFighter>();
        }

        public void SetSpiritFighters(List<SpiritFighter> spiritFighters)
        {
            _dataList = spiritFighters;
        }

        public int GetCount()
        {
            return _dataList.Count;
        }

        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }

        public List<string> chara_id
        {
            get { return _dataList.Select(x => x.chara_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public List<string> ui_spirit_id
        {
            get { return _dataList.Select(x => x.ui_spirit_id).Distinct().OrderBy(x => x).ToList(); }
        }
        public bool IsUnlockableFighter(string ui_spirit_id)
        {
            string fighter = _dataList.Where(x => x.ui_spirit_id == ui_spirit_id).Select(x => x.chara_id).FirstOrDefault();  // Get the fighter for the spirit.
            
            return fighter == default(string) ?     // If it didn't match, not unlockable.  
                false 
                : 
                _dataList.Where(x => x.chara_id == fighter).First().ui_spirit_id == ui_spirit_id;   // If it matches, its a spirit linked to an unlockable.  Only first instance will be actually unlockable fighter.
        }
    }
}
