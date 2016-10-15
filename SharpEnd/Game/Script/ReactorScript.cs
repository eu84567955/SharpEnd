using SharpEnd.Players;

namespace SharpEnd.Script
{
    internal sealed class ReactorScript : ScriptBase
    {
        public ReactorScript(Player player, string name)
            : base(player, string.Format("scripts/reactors/{0}.py", name))
        {
        }
    }
}
