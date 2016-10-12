using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SharpEnd.Players;
using System;

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

            Expose("getMap", new Func<int>(GetMap));
            Expose("getPlayerVariable", new Func<string, int>(GetPlayerVariable));
            Expose("removePlayerVariable", new Action<string>(RemovePlayerVariable));
            Expose("setMap", new Action<int>(SetMap));
            Expose("setPlayerVariable", new Action<string, int>(SetPlayerVariable));
        }

        protected void Expose(string name, object value)
        {
            m_scope.SetVariable(name, value);
        }

        public void Execute()
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
        #endregion
    }
}
