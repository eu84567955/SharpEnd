using System;

namespace SharpEnd
{
    internal static class Randomizer
    {
        private static readonly Random mRandom = new Random();

        public static sbyte NextSByte(int minValue, int maxValue)
        {
            return Convert.ToSByte(mRandom.Next(minValue, maxValue));
        }

        public static ushort NextUShort(int minValue, int maxValue)
        {
            return Convert.ToUInt16(mRandom.Next(minValue, maxValue));
        }

        public static int NextInt()
        {
            return mRandom.Next();
        }

        public static int NextInt(int minValue, int maxValue)
        {
            return mRandom.Next(minValue, maxValue);
        }

        public static long NextLong()
        {
            byte[] buffer = new byte[8];

            mRandom.NextBytes(buffer);

            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
