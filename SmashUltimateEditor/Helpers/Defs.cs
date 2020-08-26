using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashUltimateEditor
{
    public static class Defs
    {
        public const string SPIRIT_BATTLE_DATA_XML = "battle_data_tbl";
        public const string FIGHTER_DATA_XML = "fighter_data_tbl";
        public const string FILE_LOCATION = @"F:\Tools\Switch\Smash Ultimate Modding\PRCEditor\files\ui_spirits_battle_db_mod_unencr.prc";

        public const int LABEL_HEIGHT = 20;
        public const int LABEL_WIDTH = 200 ;
        public const int PADDING = 5;
        public const int BOX_HEIGHT = 20 ;
        public const int BOX_WIDTH = 200 ;
        public const int ROWS = 26;
        public const int COLUMNS = 4;

        public static List<string> RANGE_VALUES = new List<string>()
        {
            "APPEAR_RULE_TIME",
            "APPEAR_RULE_COUNT",
            "CPU_LV",
            "STOCK",
            "HP",
            "INIT_DAMAGE",
            "SCALE",
            "FLY_RATE",
            "ATTACK",
            "DEFENSE",
            "BATTLE_TIME_SEC",
            "BASIC_INIT_DAMAGE",
            "BASIC_INIT_HP",
            "BASIC_STOCK",
            "EVENT1_START_TIME",
            "EVENT1_RANGE_TIME",
            "EVENT1_COUNT",
            "EVENT1_DAMAGE",
            "EVENT2_START_TIME",
            "EVENT2_RANGE_TIME",
            "EVENT2_COUNT",
            "EVENT2_DAMAGE",
            "EVENT3_START_TIME",
            "EVENT3_RANGE_TIME",
            "EVENT3_COUNT",
            "EVENT3_DAMAGE",
            "BATTLE_POWER"
        };

        public static Dictionary<string, List<string>> HAZARD_SKILLS = new Dictionary<string, List<string>>()
        {
            { "damage_floor",   new List<string>() { "damage_floor_invalid", "damage_floor_half" } },
            { "elec_floor",     new List<string>() { "damage_floor_invalid", "damage_floor_half" } },
            { "gum_floor",      new List<string>() { "adhesion_floor_invalid" } },
            { "ice_floor",      new List<string>() { "slip_invalid" } },
            { "mist_area",      new List<string>() { "fog_invalid" } },
            { "poison_area",    new List<string>() { "poison_heal", "poison_invalid", "fog_invalid", "poison_half" } },
            { "poison_floor",   new List<string>() { "poison_heal", "poison_invalid", "poison_half" } },
            { "sleep_floor",    new List<string>() { "sleep_invalid" } },
            { "wind_area",      new List<string>() { "wind_invalid", "wind_half" } }
        };

        public static List<string> FLOAT_VALUES = new List<string>()
        {   "SCALE",
            "FLY_RATE",
        };

        public static List<string> EXCLUDED_RANDOMIZED = new List<string>()
        {
            "aw_flap_delay",
            "battle_id",
            "spirit_name",
            "appear_rule_time",
            "appear_rule_count"
        };

        public static List<string> BOSSES = new List<string>()
        {
            "ui_chara_ganonboss",
            "ui_chara_crazyhand",
            "ui_chara_darz", 
            "ui_chara_dracula",
            "ui_chara_galleom",
            "ui_chara_kiila",
            "ui_chara_kiila_darz",
            "ui_chara_lioleus",
            "ui_chara_marx",
            "ui_chara_masterhand"
        };

        public static List<string> EXCLUDED_FIGHTERS = new List<string>(BOSSES) {"ui_chara_random", ""};

        public static List<string> UNLOCKABLE_FIGHTERS = new List<string>() 
        { 
        };

        public static List<string> MII_MOVES = new List<string>()
        {
            "MII_SP_N",
            "MII_SP_S",
            "MII_SP_HI",
            "MII_SP_LW"
        };

        public static Dictionary<string, int> miiFighterMod = new Dictionary<string, int>()
        {
            { "ui_chara_miifighter", (int)Enums.mii_brawler_mod.Fighter },
            { "ui_chara_miiswordsman", (int)Enums.mii_brawler_mod.SwordFighter },
            { "ui_chara_miigunner", (int)Enums.mii_brawler_mod.Gunner },
        };

        #region FighterDataTbl
        public static ushort APPEAR_RULE_TIME_MIN  { get; } = 0;
        public static ushort APPEAR_RULE_TIME_MAX  { get; } = 60;

        // Technically, 7 doesn't ever get used (?)
        public static ushort APPEAR_RULE_COUNT_MIN  { get; } = 0;
        public static ushort APPEAR_RULE_COUNT_MAX  { get; } = 8;

        public static byte CPU_LV_MIN  { get; } = 1;
        public static byte CPU_LV_MAX  { get; } = 100;

        public static byte STOCK_MIN { get; } = 1;
        public static byte STOCK_MAX  { get; } = 3;
        
        // 0, 500
        public static ushort HP_MIN  { get; } = 0;
        public static ushort HP_MAX  { get; } = 300;

        // 0, 300
        public static ushort INIT_DAMAGE_MIN  { get; } = 0;
        public static ushort INIT_DAMAGE_MAX  { get; } = 300;

        public static float SCALE_MIN { get; } = 0.2f;
        public static float SCALE_MAX  { get; } = 4.0f;

        public static float FLY_RATE_MIN  { get; } = 0f;
        public static float FLY_RATE_MAX  { get; } = 5f;

        public static short ATTACK_MIN  { get; } = 0;
        public static short ATTACK_MAX  { get; } = 10000;

        public static short DEFENSE_MIN  { get; } = 0;
        public static short DEFENSE_MAX  { get; } = 10000;
        #endregion

        #region BattleDataTbl
        public static ushort BATTLE_TIME_SEC_MIN  { get; } = 15;
        public static ushort BATTLE_TIME_SEC_MAX  { get; } = 180;

        public static ushort BASIC_INIT_DAMAGE_MIN  { get; } = 0;
        public static ushort BASIC_INIT_DAMAGE_MAX  { get; } = 300;

        public static ushort BASIC_INIT_HP_MIN  { get; } = 80;
        public static ushort BASIC_INIT_HP_MAX  { get; } = 150;

        public static byte BASIC_STOCK_MIN  { get; } = 1;
        public static byte BASIC_STOCK_MAX  { get; } = 5;

        public static int EVENT_START_TIME_MIN  { get; } = 0;
        public static int EVENT_START_TIME_MAX  { get; } = 80;

        public static int EVENT_RANGE_TIME_MIN  { get; } = -1;
        public static int EVENT_RANGE_TIME_MAX  { get; } = 120;

        public static byte EVENT_COUNT_MIN  { get; } = 0;
        public static byte EVENT_COUNT_MAX  { get; } = 200;

        public static ushort EVENT_DAMAGE_MIN  { get; } = 0;
        public static ushort EVENT_DAMAGE_MAX  { get; } = 150;
        
        public static uint BATTLE_POWER_MIN  { get; } = 0;
        public static uint BATTLE_POWER_MAX  { get; } = 20000;

        
        public static int  EVENT1_START_TIME_MIN { get { return EVENT_START_TIME_MIN; } }
        public static int  EVENT2_START_TIME_MIN { get { return EVENT_START_TIME_MIN; } }
        public static int  EVENT3_START_TIME_MIN { get { return EVENT_START_TIME_MIN; } }
        public static int  EVENT1_RANGE_TIME_MIN { get { return EVENT_RANGE_TIME_MIN; } }
        public static int  EVENT2_RANGE_TIME_MIN { get { return EVENT_RANGE_TIME_MIN; } }
        public static int  EVENT3_RANGE_TIME_MIN { get { return EVENT_RANGE_TIME_MIN; } }
        public static byte  EVENT1_COUNT_MIN { get { return EVENT_COUNT_MIN; } }
        public static byte  EVENT2_COUNT_MIN { get { return EVENT_COUNT_MIN; } }
        public static byte  EVENT3_COUNT_MIN { get { return EVENT_COUNT_MIN; } }
        public static ushort EVENT1_DAMAGE_MIN { get { return EVENT_DAMAGE_MIN; } }
        public static ushort EVENT2_DAMAGE_MIN { get { return EVENT_DAMAGE_MIN; } }
        public static ushort EVENT3_DAMAGE_MIN { get { return EVENT_DAMAGE_MIN; } }
        public static int  EVENT1_START_TIME_MAX { get { return EVENT_START_TIME_MAX; } }
        public static int  EVENT2_START_TIME_MAX { get { return EVENT_START_TIME_MAX; } }
        public static int  EVENT3_START_TIME_MAX { get { return EVENT_START_TIME_MAX; } }
        public static int  EVENT1_RANGE_TIME_MAX { get { return EVENT_RANGE_TIME_MAX; } }
        public static int  EVENT2_RANGE_TIME_MAX { get { return EVENT_RANGE_TIME_MAX; } }
        public static int  EVENT3_RANGE_TIME_MAX { get { return EVENT_RANGE_TIME_MAX; } }
        public static byte  EVENT1_COUNT_MAX { get { return EVENT_COUNT_MAX; } }
        public static byte  EVENT2_COUNT_MAX { get { return EVENT_COUNT_MAX; } }
        public static byte  EVENT3_COUNT_MAX { get { return EVENT_COUNT_MAX; } }
        public static ushort EVENT1_DAMAGE_MAX { get { return EVENT_DAMAGE_MAX; } }
        public static ushort EVENT2_DAMAGE_MAX { get { return EVENT_DAMAGE_MAX; } }
        public static ushort EVENT3_DAMAGE_MAX { get { return EVENT_DAMAGE_MAX; } }
        #endregion
    }
}
