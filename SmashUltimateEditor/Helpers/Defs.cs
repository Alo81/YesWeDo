using System;
using System.Collections.Generic;
using System.Drawing;

namespace YesweDo
{
    public static class Defs
    {
        public const string paramLabelsGitUrl = @"https://github.com/Alo81/param-labels/raw/master/ParamLabels.csv";
        public const string paramLabelsName = @"ParamLabels.csv";

        public const string gitReleasesUrl = "https://api.github.com/repos/Alo81/YesWeDo/releases";

        #region randomizer
        public const int CHAOS = 50;
        public const int BOSS_CHECK = 50;
        public const int EVENT_CHECK = 50;

        public const float BOSS_SCALE_MOD = 1.35F;
        public const float BOSS_ATTACK_MOD = 2;
        public const float BOSS_DEFENSE_MOD = 2;
        public const float BOSS_LOW_HP_MOD = 2;
        public const float BOSS_HIGH_HP_MOD = 50;
        public const float BOSS_HP_CUTOFF = 100;
        public const int BOSS_CPU_LVL_ADD = 20;

        public const ushort PLAYER_LOW_HP_CUTOFF = 30;
        public const ushort PLAYER_LOW_HP_MOD = 30;
        public const ushort ALLY_LOW_HP_CUTOFF = 55;
        public const ushort ALLY_LOW_HP_MOD = 55;
        public const byte ALLY_LOW_STOCK_MOD = 2;

        public const int FIGHTER_COUNT_STOCK_CUTOFF = 2;
        public const int FIGHTER_COUNT_TIMER_CUTOFF = 3;
        public const int FIGHTER_COUNT_TIMER_ADD = 8;   //How many seconds to add?

        public const char CONFIG_DELIMITER = '|';
        #endregion

        #region UI
        public const int LABEL_HEIGHT = 16;
        public const int LABEL_WIDTH = 200 ;
        public const int LABEL_PADDING = 14;
        public const int BOX_HEIGHT = 23;
        public const int BOX_WIDTH = 200;
        public const int BOX_PADDING = -5;
        public const int BUTTON_HEIGHT = 48;
        public const int BUTTON_WIDTH = 200;
        public const int BUTTON_PADDING = 5;

        public static int MAX_WIDTH = Math.Max(Math.Max(BOX_WIDTH, LABEL_WIDTH), BUTTON_WIDTH);
        public static int MAX_PADDING = Math.Max(Math.Max(BOX_PADDING, LABEL_PADDING), BUTTON_PADDING);

        public static int COLUMN_WIDTH = MAX_WIDTH + (MAX_PADDING * 3);
        public static int ROW_HEIGHT = (LABEL_HEIGHT + LABEL_PADDING) * ROWS;

        public const int ROWS = 18;
        public const int COLUMNS = 4;

        public static Color labelBack = Color.FromArgb(255, 205, 205);

        public const string REMOVE_FIGHTER_BUTTON_STRING = "Remove Fighter";
        public const string EDIT_SPIRIT_DETAILS_BUTTON_STRING = "Edit Spirit Details";
        public const string SPIRIT_IMAGE_BUTTON_STRING = "Load Spirit Image #";
        public const string FILE_WILDCARD_PATTERN = ".*";
        #endregion

        #region Files
        public static string dbFileExtension = ".prc";
        public static string textFileExtension = ".msbt";

        public static List<Tuple<string, string>> spiritUiLocations = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>($"spirits_0_{FILE_WILDCARD_PATTERN}.bntx", @"\ui\replace\spirits\spirits_0\"),
            new Tuple<string, string>($"spirits_1_{FILE_WILDCARD_PATTERN}.bntx", @"\ui\replace\spirits\spirits_1\"),
            new Tuple<string, string>($"spirits_2_{FILE_WILDCARD_PATTERN}.bntx", @"\ui\replace\spirits\spirits_2\")
        };

        public static List<Tuple<string, string>> regExFiles = new List<Tuple<string, string>>(spiritUiLocations);

        public static List<Tuple<string, string>> filesToCopy = new List<Tuple<string, string>>()    // Include Spirit UI locations
        {
            new Tuple<string, string>("ui_spirits_battle_db.prc", @"\ui\param\database\"),
            new Tuple<string, string>("ui_fighter_spirit_aw_db.prc", @"\ui\param\database\"),
            new Tuple<string, string>("ui_chara_db.prc", @"\ui\param\database\"),
            new Tuple<string, string>("spirits_battle_event.prc", @"\ui\param\spirits\"),
            new Tuple<string, string>("spirits_board_special_param.prc", @"\ui\param_patch\spirits_board_special\"),
            new Tuple<string, string>("spirits_battle_items.prc", @"\param\spirits\"),
            new Tuple<string, string>("msg_spirits.msbt", @"\ui\message\"),
            new Tuple<string, string>("ui_spirit_db.prc", @"\ui\param\database\"),
            new Tuple<string, string>("ui_spirits_ability_db.prc", @"\ui\param\database\"),
            new Tuple<string, string>("ui_stage_db.prc", @"\ui\param\database\"),
            new Tuple<string, string>("ui_bgm_db.prc", @"\ui\param\database\")
        };

        public static List<string> msbtFilesToSave = new List<string>()
        {
            "msg_spirits.msbt"
        };

        public static List<string> dbFilesToSave = new List<string>()
        {
            "ui_spirit_db.prc"
        };
        #endregion

        #region MSBT

        public static string msbtSeparator = "\u000e\u0001\n\f\n";
        #endregion

        #region Spirit Battles
        public static Dictionary<string, List<string>> HAZARD_SKILLS = new Dictionary<string, List<string>>()
        {
            { "damage_floor",   new List<string>() { "damage_floor_invalid", "damage_floor_half" } },
            { "elec_floor",     new List<string>() { "damage_floor_invalid", "damage_floor_half" } },
            { "gum_floor",      new List<string>() { "adhesion_floor_invalid" } },
            { "ice_floor",      new List<string>() { "slip_invalid" } },
            { "mist_area",      new List<string>() { "fog_invalid" } },
            { "poison_area",    new List<string>() { "poison_heal", "poison_invalid", "fog_invalid", "poison_half", "fog_invalid" } },
            { "poison_floor",   new List<string>() { "poison_heal", "poison_invalid", "poison_half" } },
            { "sleep_floor",    new List<string>() { "sleep_invalid" } },
            { "wind_area",      new List<string>() { "wind_invalid", "wind_half" } }
        };

        public static Dictionary<string, string> relatedFields = new Dictionary<string, string>()
        {
            { "event#_label",   "event#_type"},
            { "event1_label",   "event1_type"},
            { "event2_label",   "event2_type"},
            { "event3_label",   "event3_type"}
        };

        public static List<string> EVENT_OPTIONS = new List<String>()
        {
            "event1_type",
            "event1_label",
            "event1_start_time",
            "event1_range_time",
            "event1_count",
            "event1_damage",
            "event2_type",
            "event2_label",
            "event2_start_time",
            "event2_range_time",
            "event2_count",
            "event2_damage",
            "event3_type",
            "event3_label",
            "event3_start_time",
            "event3_range_time",
            "event3_count",
            "event3_damage"
        };


        public static List<string> REQUIRED_PARAMS = new List<string>()
        {
            "entry_type",
            "fighter_kind",
            "fly_rate",
            "hp",
            "scale",
            "cpu_lvl",
            "basic_init_hp",
            "battle_type",
            "result_type",
            "ui_stage_id"
        };

        // Exclude event options from randomized, as we'll set it programatically elsewhere from a list of existing events.  
        public static List<string> EXCLUDED_RANDOMIZED = new List<string>(EVENT_OPTIONS)
        {
            "battle_id",
            "appear_rule_time",
            "appear_rule_count"
        };


        // final_stage1 Looks functioning?
        public static List<string> BOSSES = new List<string>()
        {
            "ui_chara_ganonboss",
            "ui_chara_crazyhand",
            "ui_chara_darz", 
            "ui_chara_dracula",
            "ui_chara_galleom",
            "ui_chara_koopag",
            "ui_chara_kiila",
            "ui_chara_kiila_darz",
            "ui_chara_lioleus",
            "ui_chara_marx",
            "ui_chara_masterhand"
        };

        public static List<string> EXCLUDED_FIGHTERS = new List<string>(BOSSES) {"ui_chara_random", ""};
        public static List<string> EXCLUDED_UNLOCKABLE_FIGHTERS = new List<string>() {"ui_chara_kirby"};

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
        public static ushort APPEAR_RULE_TIME_SANE { get; } = 0;

        // Technically, 7 doesn't ever get used (?)
        public static ushort APPEAR_RULE_COUNT_MIN  { get; } = 0;
        public static ushort APPEAR_RULE_COUNT_MAX { get; } = 8;
        public static ushort APPEAR_RULE_COUNT_SANE { get; } = 8;

        // Technically, 7 doesn't ever get used (?)
        public static byte COLOR_MIN { get; } = 0;
        public static byte COLOR_MAX { get; } = 7;
        public static byte COLOR_SANE { get; } = 0;

        public static byte CPU_LV_MIN  { get; } = 1;
        public static byte CPU_LV_MAX { get; } = 101;
        public static byte CPU_LV_SANE { get; } = 101;

        public static byte STOCK_MIN { get; } = 1;
        public static byte STOCK_MAX { get; } = 4;
        public static byte STOCK_SANE { get; } = 4;

        // 0, 500
        public static ushort HP_MIN  { get; } = 35;
        public static ushort HP_MAX { get; } = 300;
        public static ushort HP_SANE { get; } = 300;

        // 0, 300
        public static ushort INIT_DAMAGE_MIN  { get; } = 0;
        public static ushort INIT_DAMAGE_MAX { get; } = 300;
        public static ushort INIT_DAMAGE_SANE { get; } = 300;

        public static float SCALE_MIN { get; } = 0.4f;
        public static float SCALE_MAX { get; } = 2.2f;
        public static float SCALE_SANE { get; } = 4.0f;

        public static float FLY_RATE_MIN  { get; } = 0.4f;
        public static float FLY_RATE_MAX { get; } = 2.2f;
        public static float FLY_RATE_SANE { get; } = 2.5f;

        public static short ATTACK_MIN  { get; } = 0;
        public static short ATTACK_MAX { get; } = 10000;
        public static short ATTACK_SANE { get; } = 10000;

        public static short DEFENSE_MIN  { get; } = 0;
        public static short DEFENSE_MAX { get; } = 10000;
        public static short DEFENSE_SANE { get; } = 10000;
        #endregion

        #region BattleDataTbl
        public static ushort BATTLE_TIME_SEC_MIN  { get; } = 15;
        public static ushort BATTLE_TIME_SEC_MAX { get; } = 240;
        public static ushort BATTLE_TIME_SEC_SANE { get; } = 240;

        public static ushort BASIC_INIT_DAMAGE_MIN  { get; } = 0;
        public static ushort BASIC_INIT_DAMAGE_MAX { get; } = 120;
        public static ushort BASIC_INIT_DAMAGE_SANE { get; } = 120;

        public static ushort BASIC_INIT_HP_MIN  { get; } = 80;
        public static ushort BASIC_INIT_HP_MAX { get; } = 185;
        public static ushort BASIC_INIT_HP_SANE { get; } = 185;

        public static byte BASIC_STOCK_MIN  { get; } = 1;
        public static byte BASIC_STOCK_MAX { get; } = 5;
        public static byte BASIC_STOCK_SANE { get; } = 2;

        public static int EVENT_START_TIME_MIN  { get; } = 0;
        public static int EVENT_START_TIME_MAX { get; } = 45;
        public static int EVENT_START_TIME_SANE { get; } = 80;

        public static int EVENT_RANGE_TIME_MIN  { get; } = -1;
        public static int EVENT_RANGE_TIME_MAX { get; } = 120;
        public static int EVENT_RANGE_TIME_SANE { get; } = 120;

        public static byte EVENT_COUNT_MIN  { get; } = 0;
        public static byte EVENT_COUNT_MAX { get; } = 200;
        public static byte EVENT_COUNT_SANE { get; } = 200;

        public static ushort EVENT_DAMAGE_MIN  { get; } = 0;
        public static ushort EVENT_DAMAGE_MAX { get; } = 120;
        public static ushort EVENT_DAMAGE_SANE { get; } = 150;

        public static uint BATTLE_POWER_MIN  { get; } = 0;
        public static uint BATTLE_POWER_MAX { get; } = 20000;
        public static uint BATTLE_POWER_SANE { get; } = 20000;


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

        #endregion


    }
}
