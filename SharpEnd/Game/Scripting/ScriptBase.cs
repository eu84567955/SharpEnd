using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SharpEnd.Players;
using SharpEnd.Servers;
using System;

namespace SharpEnd.Scripting
{
    internal abstract class ScriptBase
    {
        private enum VariableType : int
        {
            Boolean,
            String,
            Number,
            Integer
        }

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

            SetEnvironmentVariables();

            // NOTE: Map exports.
            Expose("getMap", new Func<int>(() => m_player.MapIdentifier));
            Expose("setMap", new Action<int, string>((mapIdentifier, portalIdentifier) =>
            {
                m_player.SetMap(mapIdentifier, MasterServer.Instance.GetMap(mapIdentifier).Portals.GetPortal(portalIdentifier));
            }));
            Expose("getMapPlayerCount", new Func<int>(() => m_player.Map.Players.Count));

            // NOTE: Inventory exports.
            Expose("giveItem", new Func<int, ushort, bool>((itemIdentifier, quantity) =>
            {
                if (quantity < 0)
                {
                    return true;
                }
                else
                {
                    m_player.Items.Add(new PlayerItem(itemIdentifier, quantity));

                    return true;
                }
            }));

            // NOTE: Player exports.
            Expose("getPlayerVariable", new Func<string, string>((key) =>
            {
                string value = null;

                m_player.Variables.TryGetValue(key, out value);

                return value;
            }));
            Expose("removePlayerVariable", new Action<string>((key) => m_player.Variables.Remove(key)));
            Expose("setPlayerVariable", new Action<string, object>((key, value) =>
            {
                if (m_player.Variables.ContainsKey(key))
                {
                    m_player.Variables[key] = value.ToString();
                }
                else
                {
                    m_player.Variables.Add(key, value.ToString());
                }
            }));
        }

        private void SetEnvironmentVariables()
        {
            Expose("type_bool", VariableType.Boolean);
            Expose("type_int", VariableType.Integer);
            Expose("type_num", VariableType.Number);
            Expose("type_str", VariableType.String);
        }

        public dynamic Get(string name)
        {
            return m_scope.GetVariable(name);
        }

        protected void Expose(string name, object value)
        {
            m_scope.SetVariable(name, value);
        }

        public virtual void Execute()
        {
            m_engine.ExecuteFile(m_path, m_scope);
        }
    }
}
