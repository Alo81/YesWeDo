namespace SmashUltimateEditor.DataTables
{
    public class Event : DataTbl
    {
        // Default empty XML_NAME where it is not an actual XML element.
        internal static string XML_NAME = "";
        internal static string TYPE_NAME = XML_NAME.Replace("_param", "");

        public string GetTypeName()
        {
            string typeName = this.GetFieldValueFromName("TYPE_NAME");
            return typeName == "" ? (this.GetFieldValueFromName("XML_NAME").Replace("_param", "")) : typeName;
        }
    }

    public class LabelEvent : Event
    {
        [Order]
        public string label { get; set; }
    }
    public class StandardEvent : LabelEvent
    {
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
    public class SpawnEvent : LabelEvent
    {
        [Order]
        public string item_id { get; set; }
    }

    public class PowerUpEvent : ValueEvent
    {
        internal static new string XML_NAME = "powerup_param";
        [Order]
        public string power_type { get; set; }
    }
    public class ScaleEvent : StandardEvent
    {
        internal static string XML_NAME = "scale_param";
        internal static string TYPE_NAME = "change_scale";
        [Order]
        public bool to_bigger { get; set; }
    }
    public class DamageEvent : ValueEvent
    {
        internal static string XML_NAME = "damage_param";
    }
    public class _0x117db589bd : StandardEvent
    {
        internal static string XML_NAME = "0x117db589bd";
    }
    public class HealEvent : ValueEvent
    {
        internal static string XML_NAME = "heal_param";
    }
    public class QuakeEvent : Event
    {
        internal static string XML_NAME = "quake_param";
        internal static string TYPE_NAME = "earthquake";

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
        internal static string XML_NAME = "invalid_capture_param";
    }
    public class MetalEvent : StandardEvent
    {
        internal static string XML_NAME = "metal_param";
    }
    public class ShieldWeakenEvent : StandardEvent
    {
        internal static string XML_NAME = "shield_weaken_param";
    }
    public class ChangeGravityEvent : StandardEvent
    {
        internal static string XML_NAME = "change_gravity_param";

        [Order]
        public bool to_high { get; set; }
        [Order]
        public byte level { get; set; }
    }
    public class MagicUpEvent : StandardEvent
    {
        internal static string XML_NAME = "magic_up_param";
    }
    public class EnergyUpEvent : StandardEvent
    {
        internal static string XML_NAME = "energy_up_param";
    }
    public class WeaponUpEvent : StandardEvent
    {
        internal static string XML_NAME = "weapon_up_param";
    }
    public class ElectricUpEvent : StandardEvent
    {
        internal static string XML_NAME = "elec_up_param";
    }
    public class InvincibleEvent : StandardEvent
    {
        internal static string XML_NAME = "invincible_param";
    }
    public class FlowerEvent : StandardEvent
    {
        internal static string XML_NAME = "flower_param";
    }
    public class CurryEvent : StandardEvent
    {
        internal static string XML_NAME = "curry_param";
    }
    public class FinalStandbyEvent : StandardEvent
    {
        internal static string XML_NAME = "final_standby_param";
    }
    public class RunawayEvent : StandardEvent
    {
        internal static string XML_NAME = "runaway_param";
    }
    public class InvisibleEvent : StandardEvent
    {
        internal static string XML_NAME = "invisible_param";
    }
    public class DrownUpEvent : StandardEvent
    {
        internal static string XML_NAME = "drown_up_param";
    }
    public class _0x124c670e8e : StandardEvent
    {
        internal static string XML_NAME = "0x124c670e8e";

        [Order]
        public bool to_up { get; set; }
    }
    public class BalloonEvent : StandardEvent
    {
        internal static string XML_NAME = "ballon_param";
    }
    public class SmashUpEvent : StandardEvent
    {
        internal static string XML_NAME = "smash_up_param";
    }
    public class InvalidHealEvent : StandardEvent
    {
        internal static string XML_NAME = "invalid_heal_param";
    }
    public class InvalidJumpEvent : StandardEvent
    {
        internal static string XML_NAME = "invalid_jump_param";
    }
    public class InvalidNormalAttackEvent : StandardEvent
    {
        internal static string XML_NAME = "invalid_normal_attack_param";
    }
    public class InvalidSpecialAttackEvent : StandardEvent
    {
        internal static string XML_NAME = "invalid_special_attack_param";
    }
    public class InvalidShieldEvent : StandardEvent
    {
        internal static string XML_NAME = "invalid_shield_param";
    }
    public class ReflectUpEvent : StandardEvent
    {
        internal static string XML_NAME = "reflect_up_param";
    }
    public class _0x18bde21b22 : LabelEvent
    {
        internal static string XML_NAME = "0x18bde21b22";

        [Order]
        public int value { get; set; }
        [Order]
        public bool to_up { get; set; }
        [Order]
        public string attr { get; set; }
    }
    public class ItemEvent : LabelEvent
    {
        internal static string XML_NAME = "item_appear_param";
        internal static string TYPE_NAME = "item";

        [Order]
        public string item_id { get; set; }
        [Order]
        public uint variation { get; set; }
        [Order]
        public uint num { get; set; }
    }
    public class WarpEvent : StandardEvent
    {
        internal static string XML_NAME = "warp_param";
    }
    public class StormEvent : LabelEvent
    {
        internal static string XML_NAME = "storm_param";

        [Order]
        public bool is_left { get; set; }
        [Order]
        public float power { get; set; }
    }
    public class InputReverseEvent : StandardEvent
    {
        internal static string XML_NAME = "input_reverse_param";
    }
    public class SlipperyEvent : StandardEvent
    {
        internal static string XML_NAME = "slippery_param";
    }
    public class SleepEvent : StandardEvent
    {
        internal static string XML_NAME = "sleep_param";

        [Order]
        public uint frame { get; set; }
    }
    public class AssistEvent : SpawnEvent
    {
        internal static string XML_NAME = "assist_param";
        internal static string TYPE_NAME = "assist_appear";
    }
    public class PokemonEvent : SpawnEvent
    {
        internal static string XML_NAME = "pokemon_param";
        internal static string TYPE_NAME = "pokemon_fix";
    }
}
