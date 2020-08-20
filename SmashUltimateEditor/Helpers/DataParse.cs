using System;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor
{
    class DataParse
    {
        public static Dictionary<string, string> BattleDataTblDupes = new Dictionary<string, string>() 
        {
            ["event1_type"] = "a_event_type",
            ["event2_type"] = "a_event_type",
            ["event3_type"] = "a_event_type",
            ["event1_label"] = "a_event_label",
            ["event2_label"] = "a_event_label",
            ["event3_label"] = "a_event_label",
            ["event1_start_time"] = "a_event_start_time",
            ["event2_start_time"] = "a_event_start_time",
            ["event3_start_time"] = "a_event_start_time",
            ["event1_range_time"] = "a_event_range_time",
            ["event2_range_time"] = "a_event_range_time",
            ["event3_range_time"] = "a_event_range_time",
            ["event1_count"] = "a_event_count",
            ["event2_count"] = "a_event_count",
            ["event3_count"] = "a_event_count",
            ["event1_damage"] = "a_event_damage",
            ["event2_damage"] = "a_event_damage",
            ["event3_damage"] = "a_event_damage",
            ["recommended_skill1"] = "a_recommended_skill",
            ["recommended_skill2"] = "a_recommended_skill",
            ["recommended_skill3"] = "a_recommended_skill",
            ["recommended_skill4"] = "a_recommended_skill",
            ["recommended_skill5"] = "a_recommended_skill",
            ["recommended_skill6"] = "a_recommended_skill",
            ["recommended_skill7"] = "a_recommended_skill",
            ["recommended_skill8"] = "a_recommended_skill",
            ["recommended_skill9"] = "a_recommended_skill",
            ["recommended_skill10"] = "a_recommended_skill",
            ["recommended_skill11"] = "a_recommended_skill",
            ["recommended_skill12"] = "a_recommended_skill",
            ["recommended_skill13"] = "a_recommended_skill",
            ["un_recommended_skill1"] = "a_un_recommended_skill",
            ["un_recommended_skill2"] = "a_un_recommended_skill",
            ["un_recommended_skill3"] = "a_un_recommended_skill",
            ["un_recommended_skill4"] = "a_un_recommended_skill",
            ["un_recommended_skill5"] = "a_un_recommended_skill",
        };

        public static Dictionary<string, string> FighterDataTblDupes = new Dictionary<string, string>()
        {
            ["ability1"] = "ability",
            ["ability2"] = "ability",
            ["ability3"] = "ability"
        };

        public static Dictionary<string, string> XmlReplacements = new Dictionary<string, string>()
        {
            { "Boolean", "bool" },
            { "Int16", "short" },
            { "UInt16", "ushort" },
            { "Int32", "int"},
            { "UInt32", "uint"},
            { "Single", "float" },
            { "String", "hash40" }
        };

        public static string ReplaceTypes(string xml)
        {
            foreach(KeyValuePair<string, string> type in XmlReplacements)
            {
                xml = xml.Replace(type.Key, type.Value);
            }

            return xml;
        }

        public static string NameFixer(string name)
        {
            return name[0] == '_' ? name.Remove(0, 1) : name;
        }
    }
}
