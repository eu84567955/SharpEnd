using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Threading;
using System;
using System.Collections.Generic;

namespace SharpEnd.Scripting
{
    internal sealed class EventScript : ScriptBase
    {
        private Event m_event;

        private int m_time;
        private bool m_clock;

        private Dictionary<string, Delay> m_timers;

        public EventScript(Event evt, Player player, string name, int time, bool clock)
            : base(player, string.Format("scripts/events/{0}.py", name))
        {
            m_event = evt;

            m_time = time;
            m_clock = clock;

            Expose("startTimer", new Action<string, int, bool>(StartTimer));
        }

        private void StartTimer(string key, int delay, bool show)
        {
            m_event.AddTimer(key, delay, () =>
            {
                m_event.TimerExpired(key);
            });

            if (show)
            {
                m_player.Map.Send(MapPackets.ShowTimer(delay / 1000));
            }
        }
    }
}
