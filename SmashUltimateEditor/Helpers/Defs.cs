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
        public const string FILE_LOCATION = @"F:\Tools\Switch\Smash Ultimate Modding\PRCEditor\files\out_ui_spirits_battle_db.prc";

        public const int LABEL_HEIGHT = 20;
        public const int LABEL_WIDTH = 160;
        public const int PADDING = 5;
        public const int BOX_HEIGHT = 20;
        public const int BOX_WIDTH = 160;
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
        public static ushort APPEAR_RULE_TIME_MIN = 0;
        public static ushort APPEAR_RULE_TIME_MAX = 60;

        // Technically, 7 doesn't ever get used (?)
        public static ushort APPEAR_RULE_COUNT_MIN = 0;
        public static ushort APPEAR_RULE_COUNT_MAX = 8;

        public static byte CPU_LV_MIN = 1;
        public static byte CPU_LV_MAX = 100;

        public static byte STOCK_MAX = 99;
        public static byte STOCK_MIN = 1;
        
        // 0, 500
        public static ushort HP_MIN = 0;
        public static ushort HP_MAX = 999;

        // 0, 300
        public static ushort INIT_DAMAGE_MIN = 0;
        public static ushort INIT_DAMAGE_MAX = 999;

        public static float SCALE_MAX = 2.75f;
        public static float SCALE_MIN = 0.3f;

        public static float FLY_RATE_MIN = 0f;
        public static float FLY_RATE_MAX = 5f;

        public static short ATTACK_MIN = 0;
        public static short ATTACK_MAX = 10000;

        public static short DEFENSE_MIN = 0;
        public static short DEFENSE_MAX = 10000;
        #endregion

        #region BattleDataTbl
        public static ushort BATTLE_TIME_SEC_MIN = 0;
        public static ushort BATTLE_TIME_SEC_MAX = 180;

        public static ushort BASIC_INIT_DAMAGE_MIN = 0;
        public static ushort BASIC_INIT_DAMAGE_MAX = 300;

        public static ushort BASIC_INIT_HP_MIN = 0;
        public static ushort BASIC_INIT_HP_MAX = 150;

        public static byte BASIC_STOCK_MIN = 1;
        public static byte BASIC_STOCK_MAX = 99;

        public static int EVENT_START_TIME_MIN = 0;
        public static int EVENT_START_TIME_MAX = 80;

        public static int EVENT_RANGE_TIME_MIN = -1;
        public static int EVENT_RANGE_TIME_MAX = 120;

        public static byte EVENT_COUNT_MIN = 0;
        public static byte EVENT_COUNT_MAX = 200;

        public static ushort EVENT_DAMAGE_MIN = 0;
        public static ushort EVENT_DAMAGE_MAX = 150;
        
        public static uint BATTLE_POWER_MIN = 0;
        public static uint BATTLE_POWER_MAX = 20000;
        #endregion
    }
}
