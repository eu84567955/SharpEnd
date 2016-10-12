using System.Reflection;
using System.Text;

namespace SharpEnd.Commands
{
    internal sealed class Command
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }

        public Command(string name, string description, MethodInfo methodInfo, ParameterInfo[] parameters)
        {
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

                for (int i=1; i< Parameters.Length; i++)
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

                sb.Append('!'); // TODO: Either '!' or '@".
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
