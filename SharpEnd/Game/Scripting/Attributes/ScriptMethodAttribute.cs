using System;

namespace SharpEnd.Game.Scripting
{
    public class ScriptMethodAttribute : Attribute
    {
        public string Name { get; private set; }

        public override object TypeId
        {
            get
            {
                return Name;
            }
        }

        public ScriptMethodAttribute(string name)
        {
            Name = name;
        }
    }
}