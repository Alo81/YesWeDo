using SmashUltimateEditor.DataTables.ui_fighter_spirit_aw_db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmashUltimateEditor.DataTableCollections
{
    public class SpiritFighterDataOptions : BaseDataOptions, IDataOptions
    {
        public List<SpiritFighter> _dataList;
        private List<SpiritFighter> _unlockableFighters;
        internal static Type underlyingType = typeof(SpiritFighter);

        public SpiritFighterDataOptions()
        {
            _dataList = new List<SpiritFighter>();
        }
        public void SetData(List<IDataTbl> inData)
        {
            _dataList = inData.OfType<SpiritFighter>().ToList();
        }
        public static Type GetUnderlyingType()
        {
            return underlyingType;
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
        public List<SpiritFighter> unlockable_fighters
        {
            get
            {
                if (_unlockableFighters is null)
                {
                    BuildUnlockableFighters();
                }
                return _unlockableFighters;
            }
        }
        public List<string> unlockable_fighters_string
        {
            get
            {
                if (_unlockableFighters is null)
                {
                    BuildUnlockableFighters();
                }
                return _unlockableFighters.Select(x => x.chara_id).ToList();
            }
        }

        private void BuildUnlockableFighters()
        {
            _unlockableFighters = _dataList.OrderBy(x => x.chara_id).ToList();
            _unlockableFighters.RemoveAll(x => Defs.EXCLUDED_UNLOCKABLE_FIGHTERS.Contains(x.chara_id));
        }

        public bool IsUnlockableFighter(string ui_spirit_id)
        {
            string fighter = unlockable_fighters.Where(x => x.ui_spirit_id == ui_spirit_id).Select(x => x.chara_id).FirstOrDefault();  // Get the fighter for the spirit.
            
            return fighter == default ?     // If it didn't match, not unlockable.  
                false 
                : 
                _dataList.Where(x => x.chara_id == fighter).First().ui_spirit_id == ui_spirit_id;   // If it matches, its a spirit linked to an unlockable.  Only first instance will be actually unlockable fighter.
        }
    }
}
