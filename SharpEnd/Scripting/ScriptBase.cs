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

            Expose("setMap", new Action<int>(SetMap));
        }

        protected void Expose(string name, object value)
        {
            m_scope.SetVariable(name, value);
        }

        public void Execute()
        {
            m_engine.ExecuteFile(m_path, m_scope);
        }

        private void SetMap(int mapIdentifier)
        {
            m_player.SetMap(mapIdentifier);
        }
    }
}
