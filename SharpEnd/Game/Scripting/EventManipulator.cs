using SharpEnd.Game.Life;
using SharpEnd.Game.Players;

namespace SharpEnd.Game.Scripting
{
    public sealed class EventManipulator
    {
        private Script m_script;

        public EventManipulator(Script script)
        {
            m_script = script;
        }

        public void PlayerDied(Player player)
        {
            if (m_script.ContainsVariable("playerDied"))
            {
                m_script.GetVariable("playerDied")(new ScriptPlayer(m_script, player));
            }
        }

        public void PlayerDisconnected(Player player)
        {
            if (m_script.ContainsVariable("playerDisconnected"))
            {
                m_script.GetVariable("playerDisconnected")(new ScriptPlayer(m_script, player));
            }
        }

        public void PlayerChangedMap(Player player)
        {
            if (m_script.ContainsVariable("playerChangedMap"))
            {
                m_script.GetVariable("playerChangedMap")(new ScriptPlayer(m_script, player), new ScriptMap(m_script, player.Map));
            }
        }

        public void TimerExpired(string key)
        {
            if (m_script.ContainsVariable("timerExpired"))
            {
                m_script.GetVariable("timerExpired")(key);
            }
        }

        public void MobDied(Mob mob)
        {
            if (m_script.ContainsVariable("mobDied"))
            {
                m_script.GetVariable("mobDied")(new ScriptMob(mob));
            }
        }

        public void MobSpawned(Mob mob)
        {
            if (m_script.ContainsVariable("mobSpawned"))
            {
                m_script.GetVariable("mobSpawned")(new ScriptMob(mob));
            }
        }

        public void Deinitialize()
        {
            if (m_script.ContainsVariable("deinit"))
            {
                m_script.GetVariable("deinit")();
            }
        }
    }
}
