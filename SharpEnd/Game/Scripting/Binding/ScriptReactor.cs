using SharpEnd.Network;
using SharpEnd.Game.Life;
using SharpEnd.Utility;

namespace SharpEnd.Game.Scripting
{
    public sealed class ScriptReactor : PlayerScriptInteraction
    {
        private Reactor m_reactor;

        public ScriptReactor(Script script, GameClient client, Reactor reactor)
            : base(script, client)
        {
            m_reactor = reactor;
        }

        public void DropItems(int mesoMin, int mesoMax, int mesoChance, params int[] itemsAndChances)
        {

        }
    }
}
