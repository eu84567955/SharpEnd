using SharpEnd.Players;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Commands
{
    internal sealed class Commands : Dictionary<ECommandType, Dictionary<string, Command>>
    {
        public Commands() : base()
        {
            foreach (ECommandType commandType in (ECommandType[])Enum.GetValues(typeof(ECommandType)))
            {
                Add(commandType, new Dictionary<string, Command>());
            }
        }

        public void Load()
        {
            foreach (ECommandType commandType in this.Keys)
            {
                List<Command> commands = Reflector.FindCommands(commandType);

                foreach (Command command in commands)
                {
                    this[commandType].Add(command.Name, command);
                }
            }
        }

        public void Execute(Player player, string text)
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

            Command command = null;

            if (text.StartsWith("!") && true)
            {
                command = this[ECommandType.Gm].GetOrDefault(commandName, null);
            }
            else if (text.StartsWith("@"))
            {
                command = this[ECommandType.Player].GetOrDefault(commandName, null);
            }

            if (command != null)
            {
                bool execute = true;

                object[] parameters = new object[command.Parameters.Length];

                parameters[0] = player;

                if (args.Length < command.ParameterCount)
                {
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
                            execute = false;

                            break;
                        }
                    }
                }

                if (execute)
                {
                    try
                    {
                        command.MethodInfo.Invoke(null, parameters);
                    }
                    catch (Exception e)
                    {
                        player.Notify("[Command] Unknown error: " + e.Message);
                    }
                }
                else
                {
                    player.Notify(string.Format("[Command] Syntax: {0}", command.Syntax));
                }
            }
            else
            {
                player.Notify("[Command] Invalid command.");
            }
        }
    }
}
