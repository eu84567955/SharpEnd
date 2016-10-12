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

            if (text.StartsWith(Application.CommandIndicator))
            {
                commandName = splitted[0].TrimStart('!');
            }
            else if (text.StartsWith(Application.PlayerCommandIndicator))
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

            if (text.StartsWith(Application.CommandIndicator) && player.IsGm)
            {
                command = this[ECommandType.Gm].GetOrDefault(commandName, null);
            }
            else if (text.StartsWith(Application.PlayerCommandIndicator))
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
                    for (int i = 1; i < command.Parameters.Length; i++)
                    {
                        Type requiredType = command.Parameters[i].ParameterType;

                        if (i > args.Length)
                        {
                            parameters[i] = Type.Missing;
                        }
                        else
                        {
                            try
                            {
                                parameters[i] = Convert.ChangeType(args[i - 1], requiredType);
                            }
                            catch
                            {
                                execute = false;

                                break;
                            }
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
