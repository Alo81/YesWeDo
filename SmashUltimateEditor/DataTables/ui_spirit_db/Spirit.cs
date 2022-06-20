using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using YesweDo;

namespace YesWeDo.DataTables
{
    public class Spirit : DataTbl
    {
        // This may end up clashing.... We may need further determination if there are other db_roots in the future.  
        internal static string XML_NAME = "data_version";
        // Use first field if XML_NAME is generic. 
        internal static string XML_FIRST_FIELD = "db_root";


        public static TabPage BuildEmptyPage(DataTbls dataTbls)
        {
            return DataTbl.BuildEmptyPage(dataTbls, typeof(Spirit));
        }
        public void SetAllValuesToDefault()
        {
            var spiritId = this.ui_spirit_id;
            foreach (PropertyInfo field in GetType().GetProperties().Where(x => !(x?.GetCustomAttribute<ExcludedAttribute>()?.Excluded ?? false)))
            {
                SetFieldToDefaultValue(field);
            }
            this.ui_spirit_id = spiritId;
        }
        public Spirit Copy()
        {
            Spirit newCopy = new Spirit();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                newCopy.SetValueFromName(property.Name, this.GetPropertyValueFromName(property.Name));
            }

            return newCopy;
        }

        [Excluded(true)]
        public bool _0x11730b0c1d
        { get { return is_rematch_target; } set { is_rematch_target = value; } }

        [Excluded(true)]
        public string _0x115044521b
        { get { return replace_battle_id; } set { replace_battle_id = value; } }

        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip("The order number of the spirit that appears in Spirits Collection.")]
        public ushort save_no { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string ui_spirit_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)][OriginalType("string")]
        public string name_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public ushort fixed_no { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip("This spirit was part of a downloadable event \r\n" +
            "(Note: This is different from DLC downloads as there is no Title Update required.)")]
        public bool is_bcat { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public bool is_dlc { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip("The ID of the spirit in Spirits Collections.")]
        public ushort directory_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string ui_series_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip("Legend = 4 Star\r\n" +
            "Ace = 3 Stars\r\n" +
            "Hope = 2 Stars\r\n" +
            "Normal = 1 Star")]
        public string rank { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip("The amount of support spirit slots this spirit can have. Only works on Fighter Spirits.")]
        public byte slot_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip("The ability this spirit has. Can only have 1 ID. (Has abilities Super Abilities doesn't have)\r\n" +
            "Can't have super ability if you have normal ability.")]
        public string ability_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string attr { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        [ToolTip("The amount of exp need to reach level 99.")]
        public uint exp_lv_max { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        [ToolTip("Rate this spirit receives XP.  Baseline is 1.")]
        public float exp_up_rate { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        [ToolTip("The attack of the spirit at level 1. Gfx maxes at 10000.\r\n" +
            "Lowest = 1, Highest 30000")]
        public short base_attack { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        [ToolTip("The attack of the spirit at level 99. Gfx maxes at 10000.\r\n" +
            "Lowest = 1, Highest 30000")]
        public short max_attack { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        [ToolTip("The defense of the spirit at level 1. Gfx maxes at 10000.\r\n" +
            "Lowest = 1, Highest 30000")]
        public short base_defense { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        [ToolTip("The defense of the spirit at level 99. Gfx maxes at 10000.\r\n" +
            "Lowest = 1, Highest 30000")]
        public short max_defense { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        public string grow_type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        public byte personality { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        [ToolTip("The spirit this spirit evolves from. (Replaces ability if Spirit evolves.)")]
        public string evolve_src { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)]
        [ToolTip("The ability this spirit has. Can only have 1 ID. (Has abilities Abilities doesn't have.)\r\n" +
            "Can't have normal ability if you have super ability.")]
        public string super_ability { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        [ToolTip("Determines whether spirit appears on Spirit Board.")]
        public bool is_board_appear { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string fighter_conditions { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        [ToolTip("Rarity, Lv1 to Lv4 is least to most rare.")]
        public string appear_conditions { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public uint _0x14ef76fcd7 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public uint _0x1531b0c6f0 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public float _0x16db57210b { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        [ToolTip("SP Multiplier.")]
        public uint reward_capacity { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Stats)][Range(true)]
        public uint battle_exp { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        [ToolTip("The amount of Sp required to summon this spirit.")]
        public uint summon_sp { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        [ToolTip("The spirits required to summon this spirit.")]
        public string summon_item1 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        [ToolTip("The amount of spirits required to summon this spirit.")]
        public byte summon_item1_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item2 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item2_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item3 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item3_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item4 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item4_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        public string summon_item5 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public byte summon_item5_num { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        [ToolTip("The game title this spirit is from. Mostly effects side game spirits.")]
        public string _0x0ccad0f7b2 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)]
        [ToolTip("Limited = This item will appear for limited/rarely times.\r\n" +
            "Not for Sale - This item will never appear in the shop.\r\n" +
            "On Sale - This item will only appear when on sale.\r\n")]
        public string shop_sales_type { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Summon)][Range(true)]
        public uint shop_price { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        [ToolTip(
            "Enable a rematch option with spirits in collections/inventory.\r\n" +
            "Does NOT work with Bosses. Will result in freezing."
            )]
        public bool is_rematch_target { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Unknown)]
        public string _0x13656d7462 { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string check_id { get; set; }
        [Order][Page((int)Enums.Spirit_Page.Basic)]
        public string replace_battle_id { get; set; }
    }
}
