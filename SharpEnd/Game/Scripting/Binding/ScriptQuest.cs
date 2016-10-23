using SharpEnd.Network;
using SharpEnd.Utility;

namespace SharpEnd.Game.Scripting
{
    public sealed class ScriptQuest : ScriptNpc
    {
        private ushort m_questID;

        public ScriptQuest(Script script, GameClient client, int npcID, ushort questID)
            : base(script, client, npcID)
        {
            m_questID = questID;
        }

        public void StartQuest()
        {

        }

        public void CompleteQuest()
        {

        }
    }
}
