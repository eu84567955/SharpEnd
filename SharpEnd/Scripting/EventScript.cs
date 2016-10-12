using SharpEnd.Packets;
using SharpEnd.Players;
using SharpEnd.Threading;
using System;
using System.Collections.Generic;
using static Microsoft.Scripting.Hosting.Shell.ConsoleHostOptions;

namespace SharpEnd.Scripting
{
    internal sealed class EventScript : ScriptBase
    {
        private int m_time;
        private bool m_clock;

        private Dictionary<string, Delay> m_timers;

        public EventScript(Player player, string name, int time, bool clock)
            : base(player, string.Format("scripts/events/{0}.py", name))
        {
            m_timers = new Dictionary<string, Delay>();

            SetVariable("showTimer", new Action<int>(ShowTimer));
            SetVariable("startTimer", new Action<string, int>(StartTimer));
        }

        public override void Execute()
        {
            base.Execute();

            GetVariable("begin")();
        }

        private void ShowTimer(int time)
        {
            m_player.Map.Send(MapPackets.ShowTimer(time));
        }

        private void StartTimer(string key, int time)
        {
            m_timers.Add(key, new Delay(time, () =>
            {
                GetVariable("timerExpired")(key);
            }));
        }
    }
}
