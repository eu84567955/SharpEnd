using System.Reflection;

namespace SharpEnd.Commands
{
    internal sealed class Command
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }

        public int ParameterCount => Parameters.Length - 1;

        public Command(string name, string description, MethodInfo methodInfo, ParameterInfo[] parameters)
        {
            Name = name;
            Description = description;
            MethodInfo = methodInfo;
            Parameters = parameters;
        }
    }
}
