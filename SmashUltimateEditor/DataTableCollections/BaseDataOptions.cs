using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public class BaseDataOptions
    {
        public List<string> GetOptionsFromName(string name)
        {
            // TURN EVERYTHING TO STRINGS
            var list = GetType().GetProperty(name).GetValue(this);
            if (list.GetType() != typeof(List<string>))
            {
                var newList = new List<string>();
                foreach (var item in (IEnumerable)list)
                {
                    newList.Add((string)Convert.ChangeType(item, typeof(string)));
                }
                list = newList;
            }
            return (List<string>)list;
        }
    }
}
