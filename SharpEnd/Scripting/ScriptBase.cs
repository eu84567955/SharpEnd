using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SharpEnd.Packets;
using SharpEnd.Players;
using System;
using System.IO;

namespace SharpEnd.Scripting
{
    internal abstract class ScriptBase
    {
        protected readonly Player m_player;

        private readonly string m_path;
        private readonly ScriptEngine m_engine;
        private readonly ScriptScope m_scope;

        protected ScriptBase(Player player, string path)
        {
            m_player = player;

            m_path = path;
            m_engine = Python.CreateEngine();
            m_scope = m_engine.CreateScope();

            SetVariable("getMap", new Func<int>(GetMap));
            SetVariable("getPlayerVariable", new Func<string, int>(GetPlayerVariable));
            SetVariable("removePlayerVariable", new Action<string>(RemovePlayerVariable));
            SetVariable("setMap", new Action<int>(SetMap));
            SetVariable("setPlayerVariable", new Action<string, int>(SetPlayerVariable));
            SetVariable("startEvent", new Func<string, int, bool, bool>(StartEvent));
        }

        protected dynamic GetVariable(string name)
        {
            return m_scope.GetVariable(name);
        }

        protected void SetVariable(string name, object value)
        {
            m_scope.SetVariable(name, value);
        }

        public virtual void Execute()
        {
            m_engine.ExecuteFile(m_path, m_scope);
        }

        #region Exports
        private int GetMap()
        {
            return m_player.Map.Identifier;
        }

        private int GetPlayerVariable(string key)
        {
            return int.Parse(m_player.Variables[key]);
        }

        private void RemovePlayerVariable(string key)
        {
            m_player.Variables.Remove(key);
        }

        private void SetMap(int mapIdentifier)
        {
            m_player.SetMap(mapIdentifier);
        }

        private void SetPlayerVariable(string key, int value)
        {
            m_player.Variables.Add(key, value.ToString());
        }

        private bool StartEvent(string name, int time, bool clock)
        {
            if (File.Exists(string.Format("scripts/events/{0}.py", name)))
            {
                EventScript script = new EventScript(m_player, name, time, clock);

                try
                {
                    script.Execute();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
