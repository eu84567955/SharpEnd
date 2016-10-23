using SharpEnd.Network.Servers;
using SharpEnd.Utility;
using System.Collections.Generic;
using System.Timers;

namespace SharpEnd.Game.Scripting
{
    public sealed class ScriptEvent
    {
        private Script m_script;
        private string m_name;
        private ChannelServer m_channel;
        private EventManipulator m_hooks;
        private Dictionary<string, object> m_variables;
        private Dictionary<string, Timer> m_timers;

        public ScriptEvent(Script script, string name, ChannelServer channel, EventManipulator hooks)
        {
            m_script = script;
            m_name = name;
            m_channel = channel;
            m_hooks = hooks;
            m_variables = new Dictionary<string, object>();
            m_timers = new Dictionary<string, Timer>();
        }

        public object GetVariable(string key)
        {
            object value = null;

            m_variables.TryGetValue(key, out value);

            return value;
        }

        public void SetVariable(string key, object value)
        {
            if (m_variables.ContainsKey(key))
            {
                m_variables[key] = value;
            }
            else
            {
                m_variables.Add(key, value);
            }
        }

        public ScriptMap GetMap(int mapID)
        {
            return new ScriptMap(m_script, m_channel.MapFactory.GetMap(mapID)); // TODO: Use the channel's map factory.
        }

        // TODO: makeMap - Creates a new instanced map.
        // TODO: destroyMap - Removes an instanced map.

        // TODO: Find a better logic for timer disposal.
        public void StartTimer(string key, double delay)
        {
            Timer timer = new Timer(delay);

            timer.Elapsed += new ElapsedEventHandler((o, e) =>
            {
                timer.Stop();

                m_timers.Remove(key);

                m_hooks.TimerExpired(key);
            });

            timer.Start();

            m_timers.Add(key, timer);
        }

        public void StopTimer(string key)
        {
            Timer timer;

            if (m_timers.TryGetValue(key, out timer))
            {
                timer.Stop();
            }
        }

        public void StopTimers()
        {
            foreach (Timer timer in m_timers.Values)
            {
                timer.Stop();
            }
        }

        public void DestroyEvent()
        {
            // TODO.
        }
    }
}
