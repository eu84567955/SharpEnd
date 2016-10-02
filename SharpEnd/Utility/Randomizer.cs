using System;
using System.Collections.Generic;

namespace SharpEnd
{
    public static class Randomizer
    {
        private static readonly Random mRandom = new Random();

        public static T Select<T>(List<T> list)
        {
            return list[mRandom.Next(0, list.Count)];
        }

        public static int Next()
        {
            return mRandom.Next();
        }

        public static int Next(int min, int max)
        {
            return mRandom.Next(min, max);
        }

        public static void NextBytes(byte[] buffer)
        {
            mRandom.NextBytes(buffer);
        }
    }
}
