using SharpEnd.Network;
using SharpEnd.Network.Servers;

namespace SharpEnd.Game.Scripting
{
    public abstract class PlayerScriptInteraction
    {
        protected Script m_script;
        protected GameClient m_client;

        protected PlayerScriptInteraction(Script script, GameClient client)
        {
            m_script = script;
            m_client = client;
        }

        public ScriptEvent GetEvent(string name)
        {
            return MasterServer.Instance.Worlds[m_client.World][m_client.Channel].EventManager.GetRunningScript(name);
        }

        public ScriptEvent MakeEvent(string name, bool onlyOne, object attachment)
        {
            return MasterServer.Instance.Worlds[m_client.World][m_client.Channel].EventManager.RunScript(name, onlyOne, attachment);
        }
    }
}
