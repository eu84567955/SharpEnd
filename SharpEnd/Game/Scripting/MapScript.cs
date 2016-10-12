using SharpEnd.Players;

namespace SharpEnd.Scripting
{
    internal sealed class MapScript : ScriptBase
    {
        public MapScript(Player player, string name)
            : base(player, string.Format("scripts/maps/{0}.py", name))
        {
        }
    }
}
