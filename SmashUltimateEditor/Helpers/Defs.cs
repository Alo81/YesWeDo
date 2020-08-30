﻿using System;
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

        public const string FILE_NAME = "ui_spirits_battle_db_mod_unencr.prc";
        public const string FILE_NAME_ENCR = "ui_spirits_battle_db.prc";

        public const string FILE_DIRECTORY = @"F:\Tools\Switch\Smash Ultimate Modding\PRCEditor\files\SpiritDbs\";
        public const string FILE_DIRECTORY_CUSTOM_BATTLES = FILE_DIRECTORY + @"CustomBattles\";
        public const string FILE_DIRECTORY_ENCR = FILE_DIRECTORY + @"Encrypted\";
        public const string FILE_DIRECTORY_RANDOMIZED = FILE_DIRECTORY + @"Randomized\";

        public const string FILE_LOCATION = FILE_DIRECTORY + FILE_NAME;
        public const string FILE_LOCATION_CUSTOM_BATTLES = FILE_DIRECTORY_CUSTOM_BATTLES + FILE_NAME;
        public const string FILE_LOCATION_ENCR = FILE_DIRECTORY_ENCR + FILE_NAME_ENCR;

        public const string LABELS_DIRECTORY = @"F:\Tools\Switch\Smash Ultimate Modding\PRCEditor\files\SpiritDbs\";
        public const string LABELS_FILE_NAME = @"ParamLabels.csv";
        public const string LABELS_FILE_LOCATION = LABELS_DIRECTORY + LABELS_FILE_NAME;

        public const int LABEL_HEIGHT = 20;
        public const int LABEL_WIDTH = 200 ;
        public const int PADDING = 5;
        public const int BOX_HEIGHT = 20 ;
        public const int BOX_WIDTH = 200 ;
        public const int ROWS = 26;
        public const int COLUMNS = 4;

        public const int CHAOS = 50;

        public const float BOSS_SCALE_MOD = 1.35F;
        public const float BOSS_ATTACK_MOD = 2;
        public const float BOSS_DEFENSE_MOD = 2;
        public const float BOSS_LOW_HP_MOD = 2;
        public const float BOSS_HIGH_HP_MOD = 50;
        public const float BOSS_HP_CUTOFF = 100;
        public const int BOSS_CPU_LVL_ADD = 20;

        public const ushort PLAYER_LOW_HP_MOD = 30;

        public const int FIGHTER_COUNT_STOCK_CUTOFF = 3;
        public const int FIGHTER_COUNT_TIMER_CUTOFF = 3;
        public const int FIGHTER_COUNT_TIMER_ADD = 8;   //How many seconds to add?

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
            "aw_flap_delay",
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
            "ui_chara_kiila",
            "ui_chara_kiila_darz",
            "ui_chara_lioleus",
            "ui_chara_marx",
            "ui_chara_masterhand"
        };

        public static List<string> EXCLUDED_FIGHTERS = new List<string>(BOSSES) {"ui_chara_random", ""};

        public static List<string> UNLOCKABLE_FIGHTERS = new List<string>() 
        {
            "bayonetta","brave","buddy","captain","chrom","cloud","daisy","dedede",
            "diddy","dolly","donkey","falco","fox","gamewatch","ganon","gaogaen",
            "gekkouga","ice_climber","ike","inkling","jack","kamui","ken","kirby",
            "koopa","koopajr","krool","link","littlemac","lucario","lucas","lucina",
            "luigi","mario","mariod","marth","master","metaknight","mewtwo","miifighter",
            "miigunner","miiswordsman","murabito","ness","packun","pacman","palutena","peach",
            "pfushigisou","pichu","pikachu","pikmin","pit","pitb","plizardon","ptrainer",
            "purin","pzenigame","reflet","richter","ridley","robot","rockman","rosetta",
            "roy","ryu","samus","samusd","sheik","shizue","shulk","simon",
            "snake","sonic","szerosuit","tantan","toonlink","wario","wiifit","wolf",
            "yoshi","younglink","zelda"
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
        public static ushort APPEAR_RULE_TIME_SANE { get; } = 0;

        // Technically, 7 doesn't ever get used (?)
        public static ushort APPEAR_RULE_COUNT_MIN  { get; } = 0;
        public static ushort APPEAR_RULE_COUNT_MAX { get; } = 8;
        public static ushort APPEAR_RULE_COUNT_SANE { get; } = 8;

        public static byte CPU_LV_MIN  { get; } = 1;
        public static byte CPU_LV_MAX { get; } = 100;
        public static byte CPU_LV_SANE { get; } = 100;

        public static byte STOCK_MIN { get; } = 1;
        public static byte STOCK_MAX { get; } = 3;
        public static byte STOCK_SANE { get; } = 3;

        // 0, 500
        public static ushort HP_MIN  { get; } = 35;
        public static ushort HP_MAX { get; } = 300;
        public static ushort HP_SANE { get; } = 300;

        // 0, 300
        public static ushort INIT_DAMAGE_MIN  { get; } = 0;
        public static ushort INIT_DAMAGE_MAX { get; } = 300;
        public static ushort INIT_DAMAGE_SANE { get; } = 300;

        public static float SCALE_MIN { get; } = 0.4f;
        public static float SCALE_MAX { get; } = 3.0f;
        public static float SCALE_SANE { get; } = 4.0f;

        public static float FLY_RATE_MIN  { get; } = 0.4f;
        public static float FLY_RATE_MAX { get; } = 2.5f;
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
        public static byte BASIC_STOCK_SANE { get; } = 5;

        public static int EVENT_START_TIME_MIN  { get; } = 0;
        public static int EVENT_START_TIME_MAX { get; } = 80;
        public static int EVENT_START_TIME_SANE { get; } = 80;

        public static int EVENT_RANGE_TIME_MIN  { get; } = -1;
        public static int EVENT_RANGE_TIME_MAX { get; } = 120;
        public static int EVENT_RANGE_TIME_SANE { get; } = 120;

        public static byte EVENT_COUNT_MIN  { get; } = 0;
        public static byte EVENT_COUNT_MAX { get; } = 200;
        public static byte EVENT_COUNT_SANE { get; } = 200;

        public static ushort EVENT_DAMAGE_MIN  { get; } = 0;
        public static ushort EVENT_DAMAGE_MAX { get; } = 150;
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
    }
}
