using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SmashUltimateEditor.DataTables
{
    public class Event : DataTbl, IDataTbl
    {

    }

    public class StandardEvent : Event
    {
        [Order]
        public string label { get; set; }
        [Order]
        public bool is_player_target { get; set; }
        [Order]
        public bool is_enemy_target { get; set; }
    }
    public class ValueEvent : StandardEvent
    {
        [Order]
        public int value { get; set; }
    }
    public class SpawnEvent : Event
    {
        [Order]
        public string label { get; set; }
        [Order]
        public string item_id { get; set; }
    }

    public class PowerupEvent : ValueEvent
    {
        internal const string XML_NAME = "powerup_param";
        [Order]
        public string power_type { get; set; }
    }
    public class ScaleEvent : StandardEvent
    {
        internal const string XML_NAME = "scale_param";
        [Order]
        public bool to_bigger { get; set; }
    }
    public class DamageEvent : ValueEvent
    {
        internal const string XML_NAME = "damage_param";
    }
    public class _0x117db589bd : StandardEvent
    {
        internal const string XML_NAME = "0x117db589bd";
    }
    public class HealEvent : ValueEvent
    {
        internal const string XML_NAME = "heal_param";
    }
    public class QuakeEvent : Event
    {
        internal const string XML_NAME = "quake_param";

        [Order]
        public int before_quake_frame { get; set; }
        [Order]
        public int before_interval_frame { get; set; }
        [Order]
        public int quake_frame { get; set; }
        [Order]
        public int after_interval_frame { get; set; }
        [Order]
        public int after_quake_frame { get; set; }
    }
    public class InvalidCaptureEvent : StandardEvent
    {
        internal const string XML_NAME = "invalid_capture_param";
    }
    public class MetalEvent : StandardEvent
    {
        internal const string XML_NAME = "metal_param";
    }
    public class ShieldWeakenEvent : StandardEvent
    {
        internal const string XML_NAME = "shield_weaken_param";
    }
    public class ChangeGravityEvent : StandardEvent
    {
        internal const string XML_NAME = "change_gravity_param";

        [Order]
        public bool to_high { get; set; }
        [Order]
        public byte level { get; set; }
    }
    public class MagicUpEvent : StandardEvent
    {
        internal const string XML_NAME = "magic_up_param";
    }
    public class EnergyUpEvent : StandardEvent
    {
        internal const string XML_NAME = "energy_up_param";
    }
    public class WeaponUpEvent : StandardEvent
    {
        internal const string XML_NAME = "weapon_up_param";
    }
    public class ElectricUpEvent : StandardEvent
    {
        internal const string XML_NAME = "elec_up_param";
    }
    public class InvincibleEvent : StandardEvent
    {
        internal const string XML_NAME = "invincible_param";
    }
    public class FlowerEvent : StandardEvent
    {
        internal const string XML_NAME = "flower_param";
    }
    public class CurryEvent : StandardEvent
    {
        internal const string XML_NAME = "curry_param";
    }
    public class FinalStandbyEvent : StandardEvent
    {
        internal const string XML_NAME = "final_standby_param";
    }
    public class RunawayEvent : StandardEvent
    {
        internal const string XML_NAME = "runaway_param";
    }
    public class InvisibleEvent : StandardEvent
    {
        internal const string XML_NAME = "invisible_param";
    }
    public class DrownUpEvent : StandardEvent
    {
        internal const string XML_NAME = "drown_up_param";
    }
    public class _0x124c670e8e : StandardEvent
    {
        internal const string XML_NAME = "0x124c670e8e";

        [Order]
        public bool to_up { get; set; }
    }
    public class BalloonEvent : StandardEvent
    {
        internal const string XML_NAME = "ballon_param";
    }
    public class SmashUpEvent : StandardEvent
    {
        internal const string XML_NAME = "smash_up_param";
    }
    public class InvalidHealEvent : StandardEvent
    {
        internal const string XML_NAME = "invalid_heal_param";
    }
    public class InvalidJumpEvent : StandardEvent
    {
        internal const string XML_NAME = "invalid_jump_param";
    }
    public class InvalidNormalAttackEvent : StandardEvent
    {
        internal const string XML_NAME = "invalid_normal_attack_param";
    }
    public class InvalidSpecialAttackEvent : StandardEvent
    {
        internal const string XML_NAME = "invalid_special_attack_param";
    }
    public class InvalidShieldEvent : StandardEvent
    {
        internal const string XML_NAME = "invalid_shield_param";
    }
    public class ReflectUpEvent : StandardEvent
    {
        internal const string XML_NAME = "reflect_up_param";
    }
    public class _0x18bde21b22 : Event
    {
        internal const string XML_NAME = "0x18bde21b22";

        [Order]
        public string label { get; set; }
        [Order]
        public int value { get; set; }
        [Order]
        public bool to_up { get; set; }
        [Order]
        public string attr { get; set; }
    }
    public class ItemEvent : Event
    {
        internal const string XML_NAME = "item_appear_param";

        [Order]
        public string label { get; set; }
        [Order]
        public string item_id { get; set; }
        [Order]
        public uint variation { get; set; }
        [Order]
        public uint num { get; set; }
    }
    public class WarpEvent : StandardEvent
    {
        internal const string XML_NAME = "warp_param";
    }
    public class StormEvent : Event
    {
        internal const string XML_NAME = "storm_param";

        [Order]
        public string label { get; set; }
        [Order]
        public bool is_keft { get; set; }
        [Order]
        public float power { get; set; }
    }
    public class InputReverseEvent : StandardEvent
    {
        internal const string XML_NAME = "input_reverse_param";
    }
    public class SlipperyEvent : StandardEvent
    {
        internal const string XML_NAME = "slippery_param";
    }
    public class SleepEvent : StandardEvent
    {
        internal const string XML_NAME = "sleep_param";

        [Order]
        public uint frame { get; set; }
    }
    public class AssistEvent : SpawnEvent
    {
        internal const string XML_NAME = "assist_param";
    }
    public class PokemonEvent : SpawnEvent
    {
        internal const string XML_NAME = "pokemon_param";
    }
}
