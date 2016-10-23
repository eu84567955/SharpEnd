using System;

namespace SharpEnd.Game.Commands
{
    public sealed class GmCommandAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public GmCommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
