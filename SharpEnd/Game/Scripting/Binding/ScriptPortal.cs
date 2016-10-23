using SharpEnd.Network;

namespace SharpEnd.Game.Scripting
{
    public sealed class ScriptPortal : PlayerScriptInteraction
    {
        private string m_portalID;
        private bool m_warped;

        public bool Warped
        {
            get
            {
                return m_warped;
            }
        }

        public ScriptPortal(Script script, GameClient client, string portalID)
            : base(script, client)
        {
            m_portalID = portalID;
            m_warped = true;
        }

        public void PlayPortalSoundEffect()
        {
            // TODO: Portal sound effect packet.
        }

        public void Block()
        {
            // TODO: Portal block packet.

            m_warped = false;
        }
    }
}
