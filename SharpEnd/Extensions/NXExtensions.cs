using reNX.NXProperties;
using System;

namespace SharpEnd.Data
{
    internal static class NXExtensions
    {
        public static int GetIdentifier(this NXNode node)
        {
            return int.Parse(node.Name.Replace(".img", ""));
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

            return Convert.ToByte(node[childName].ValueOrDie<long>());
        }

        public static sbyte GetSByte(this NXNode node, string childName, sbyte def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return Convert.ToSByte(node[childName].ValueOrDie<long>());
        }

        public static short GetShort(this NXNode node, string childName, short def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return Convert.ToInt16(node[childName].ValueOrDie<long>());
        }

        public static ushort GetUShort(this NXNode node, string childName, ushort def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return Convert.ToUInt16(node[childName].ValueOrDie<long>());
        }

        public static int GetInt(this NXNode node, string childName, int def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return Convert.ToInt32(node[childName].ValueOrDie<long>());
        }

        public static uint GetUInt(this NXNode node, string childName, uint def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return Convert.ToUInt32(node[childName].ValueOrDie<long>());
        }

        public static long GetLong(this NXNode node, string childName, long def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return node[childName].ValueOrDie<long>();
        }

        public static ulong GetULong(this NXNode node, string childName, ulong def = 0)
        {
            if (!node.ContainsChild(childName))
            {
                return def;
            }

            return Convert.ToUInt64(node[childName].ValueOrDie<long>());
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
