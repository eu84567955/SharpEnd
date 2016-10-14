using System.Reflection;
using System.Text;

namespace SharpEnd.Game.Commands
{
    internal sealed class Command
    {
        public ECommandType Type { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }

        public Command(ECommandType type, string name, string description, MethodInfo methodInfo, ParameterInfo[] parameters)
        {
            Type = type;
            Name = name;
            Description = description;
            MethodInfo = methodInfo;
            Parameters = parameters;
        }

        public int ParameterCount
        {
            get
            {
                int count = 0;

                for (int i = 1; i < Parameters.Length; i++)
                {
                    if (!Parameters[i].HasDefaultValue)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public string Syntax
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                switch (Type)
                {
                    case ECommandType.Gm: sb.Append('!'); break;
                    case ECommandType.Player: sb.Append('@'); break;
                }

                sb.Append(Name);

                for (int i = 1; i < Parameters.Length; i++)
                {
                    if (Parameters[i].HasDefaultValue)
                    {
                        sb.Append(" {" + Parameters[i].Name + "}");
                    }
                    else
                    {
                        sb.Append(" [" + Parameters[i].Name + "]");
                    }
                }

                return sb.ToString();
            }
        }
    }
}
