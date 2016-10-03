using SharpEnd.Players;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Commands
{
    internal sealed class Commands : Dictionary<string, Command>
    {
        public Commands() : base() { }

        public void Load()
        {
            foreach (Command command in Reflector.FindAllGmCommands())
            {
                Add(command.Name, command);
            }

            Console.WriteLine($"Loaded {Count} commands.");
        }

        public bool Execute(Player player, string text)
        {
            string[] splitted = text.Split(' ');
            splitted[0] = splitted[0].ToLower();
            string commandName = "";

            if (text.StartsWith("!"))
            {
                commandName = splitted[0].TrimStart('!');
            }
            else if (text.StartsWith("@"))
            {
                commandName = splitted[0].TrimStart('@');
            }
            else
            {
                commandName = splitted[0];
            }

            string[] args = new string[splitted.Length - 1];

            for (int i = 1; i < splitted.Length; i++)
            {
                args[i - 1] = splitted[i];
            }

            if (ContainsKey(commandName))
            {
                bool execute = true;

                Command command = this[commandName];

                object[] parameters = new object[command.Parameters.Length];

                parameters[0] = player;

                if (args.Length < command.ParameterCount)
                {
                    string syntax = "[Command] Syntax: !" + commandName + " ";

                    foreach (var parameter in command.Parameters)
                    {
                        syntax += "[" + parameter.Name + "] ";
                    }

                    player.Notify(syntax);

                    execute = false;
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        Type requiredType = command.Parameters[i + 1].ParameterType;

                        try
                        {
                            parameters[i + 1] = Convert.ChangeType(args[i], requiredType);
                        }
                        catch
                        {
                            string syntax = "[Command] Syntax: !" + commandName + " ";

                            foreach (var parameter in command.Parameters)
                            {
                                syntax += "[" + parameter.Name + "] ";
                            }

                            player.Notify(syntax);

                            execute = false;

                            break;
                        }
                    }
                }

                if (execute)
                {
                    command.MethodInfo.Invoke(null, parameters);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
