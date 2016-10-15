﻿using SharpEnd.Players;

namespace SharpEnd.Script
{
    internal sealed class ItemScript : ScriptBase
    {
        public ItemScript(Player player, string name)
            : base(player, string.Format("scripts/items/{0}.py", name))
        {
        }
    }
}