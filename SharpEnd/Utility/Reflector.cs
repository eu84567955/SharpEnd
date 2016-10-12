using SharpEnd.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharpEnd.Utility
{
    internal static class Reflector
    {
        public static List<Tuple<T1, T2>> FindAllMethods<T1, T2>()
            where T1 : Attribute
            where T2 : class
        {
            if (!typeof(T2).IsSubclassOf(typeof(Delegate))) return null;
            List<Tuple<T1, T2>> results = new List<Tuple<T1, T2>>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GlobalAssemblyCache) continue;
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    MethodInfo[] methods = type.GetMethods();
                    foreach (MethodInfo method in methods)
                    {
                        T1 attribute = Attribute.GetCustomAttribute(method, typeof(T1), false) as T1;
                        if (attribute == null) continue;
                        T2 callback = Delegate.CreateDelegate(typeof(T2), method, false) as T2;
                        if (callback == null) continue;
                        results.Add(new Tuple<T1, T2>(attribute, callback));
                    }
                }
            }
            return results;
        }

        public static List<Command> FindCommands(ECommandType commandType)
        {
            List<Command> commands = new List<Command>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GlobalAssemblyCache)
                {
                    continue;
                }

                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    MethodInfo[] methods = type.GetMethods();

                    foreach (var method in methods)
                    {
                        switch (commandType)
                        {
                            case ECommandType.Gm:
                                {
                                    GmCommandAttribute attribute = Attribute.GetCustomAttribute(method, typeof(GmCommandAttribute), false) as GmCommandAttribute;

                                    if (attribute == null)
                                    {
                                        continue;
                                    }

                                    string name = attribute.Name;
                                    string description = attribute.Description;
                                    MethodInfo methodInfo = method;
                                    ParameterInfo[] parameters = method.GetParameters();

                                    commands.Add(new Command(name, description, methodInfo, parameters));
                                }
                                break;

                            case ECommandType.Player:
                                {
                                    PlayerCommandAttribute attribute = Attribute.GetCustomAttribute(method, typeof(PlayerCommandAttribute), false) as PlayerCommandAttribute;

                                    if (attribute == null)
                                    {
                                        continue;
                                    }

                                    string name = attribute.Name;
                                    string description = attribute.Description;
                                    MethodInfo methodInfo = method;
                                    ParameterInfo[] parameters = method.GetParameters();

                                    commands.Add(new Command(name, description, methodInfo, parameters));
                                }
                                break;
                        }
                    }
                }
            }

            return commands;
        }
    }
}