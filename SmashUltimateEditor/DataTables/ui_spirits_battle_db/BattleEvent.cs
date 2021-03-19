using System;
using System.Collections.Generic;
using System.Text;

namespace YesWeDo.DataTables
{
    public class BattleEvent
    {
        public string  event_type;
        public string  event_label;
        public int     event_start_time;
        public int     event_range_time;
        public byte    event_count;
        public ushort  event_damage;

        public BattleEvent() { }

        public BattleEvent(string event_type, string event_label, int event_start_time, int event_range_time, byte event_count, ushort event_damage)
        {
            this.event_type         = event_type;
            this.event_label        = event_label;
            this.event_start_time   = event_start_time;
            this.event_range_time   = event_range_time;
            this.event_count        = event_count;
            this.event_damage       = event_damage;
        }
    }
}
