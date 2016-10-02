using System;

namespace SharpEnd
{
    internal static class Randomizer
    {
        private static readonly Random mRandom = new Random();

        public static long NextLong()
        {
            byte[] buffer = new byte[8];

            mRandom.NextBytes(buffer);

            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
