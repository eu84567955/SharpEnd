using System;

namespace SharpEnd.Game.Commands
{
    public sealed class PlayerCommandAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public PlayerCommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
