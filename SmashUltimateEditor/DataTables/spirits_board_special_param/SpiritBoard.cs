using System;
using System.Collections.Generic;
using System.Xml;
using YesWeDo.DataTables;

namespace SmashUltimateEditor.DataTables
{
    class SpiritBoard : DataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "0x1700f80264";
        // Use first field if XML_NAME is generic. 
        //internal static string XML_FIRST_FIELD = "ui_spirit_id";

        public static string spiritListHash = "spirit_id";

        public override void BuildFromXml(XmlReader reader)
        {
            bool listEnded = false;
            while (reader.Read())
            {
                var dbVal = GetNextXmlValue(reader);

                if (dbVal == null)
                {
                    return;
                }
                else if (dbVal.hash == "spirit_list")
                {
                    spirit_list = new List<string>();
                    while (reader.Read() && !listEnded)
                    {
                        var result = GetNextXmlListItems(reader);
                        listEnded = result == null;

                        if(!listEnded)
                        {
                            spirit_list.Add(result);
                        }
                    }
                }
                else
                {
                    this.SetValueFromName(DataParse.ImportNameFixer(dbVal.hash), dbVal.value);
                }
            }
            return;
        }

        [Order]
        public uint save_no { get; set; }
        [Order]
        public string label { get; set; }
        [Order]
        public List<string> spirit_list { get; set; }
        [Order]
        public string prize_id { get; set; }
        [Order]
        public uint value { get; set; }
        [Order]
        public string bgm_id { get; set; }
        [Order]
        public uint layout_no { get; set; }
        [Order]
        public string shop_item_tag { get; set; }
    }
}
