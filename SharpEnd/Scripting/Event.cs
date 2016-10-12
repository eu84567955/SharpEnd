using SharpEnd.Players;
using SharpEnd.Threading;
using System;
using System.Collections.Generic;

namespace SharpEnd.Scripting
{
    internal sealed class Event
    {
        private EventScript m_script;

        private Dictionary<string, Delay> m_timers;

        public Event(Player player, string name, int time, bool clock)
        {
            m_script = new EventScript(this, player, name, time, clock);
            m_script.Execute(); // NOTE: Just for scope.

            m_timers = new Dictionary<string, Delay>();

            player.Event = this;
        }

        public void AddTimer(string key, int delay, Action action)
        {
            m_timers.Add(key, new Delay(delay, action));
        }

        public void Begin()
        {
            m_script.Get("begin");
        }

        public void PlayerChangeMap()
        {
            m_script.Get("playerChangeMap");
        }

        public void PlayerDeath()
        {
            m_script.Get("playerDeath");
        }

        public void PlayerDisconnect(Player player)
        {
            m_script.Get("playerDisconnect")(player);
        }

        public void TimerExpired(string key)
        {
            m_script.Get("timerExpired")(key);
        }
    }
}
