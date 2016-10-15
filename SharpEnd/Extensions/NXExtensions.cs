using reNX.NXProperties;
using System;

namespace SharpEnd.Game.Data
{
    public static class NXExtensions
    {
        public static T GetIdentifier<T>(this NXNode node)
        {
            return (T)Convert.ChangeType(node.Name.Replace(".img", ""), typeof(T));
        }

        public static bool GetBoolean(this NXNode node, string childName, bool def = false)
        {
            return GetByte(node, childName) == 1;
        }

        public static double GetDouble(this NXNode node, string childName, double def = 0.0d)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return node[childName].ValueOrDie<double>();
        }

        public static byte GetByte(this NXNode node, string childName, byte def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return byte.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToByte(node[childName].ValueOrDie<long>());
            }
        }

        public static sbyte GetSByte(this NXNode node, string childName, sbyte def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return sbyte.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToSByte(node[childName].ValueOrDie<long>());
            }
        }

        public static short GetShort(this NXNode node, string childName, short def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return short.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToInt16(node[childName].ValueOrDie<long>());
            }
        }

        public static ushort GetUShort(this NXNode node, string childName, ushort def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return ushort.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToUInt16(node[childName].ValueOrDie<long>());
            }
        }

        public static int GetInt(this NXNode node, string childName, int def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return int.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToInt32(node[childName].ValueOrDie<long>());
            }
        }

        public static uint GetUInt(this NXNode node, string childName, uint def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return uint.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToUInt32(node[childName].ValueOrDie<long>());
            }
        }

        public static long GetLong(this NXNode node, string childName, long def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return long.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return node[childName].ValueOrDie<long>();
            }
        }

        public static ulong GetULong(this NXNode node, string childName, ulong def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            if (node[childName] is NXValuedNode<string>)
            {
                return ulong.Parse(node[childName].ValueOrDie<string>());
            }
            else
            {
                return Convert.ToUInt64(node[childName].ValueOrDie<long>());
            }
        }

        public static string GetString(this NXNode node, string childName, string def = "")
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return node[childName].ValueOrDie<string>();
        }
    }
}
