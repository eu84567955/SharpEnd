using System;

namespace SharpEnd.Drawing
{
    internal sealed class Ratio
    {
        public bool IsDefined { get; private set; }
        public bool IsNegative { get; private set; }

        public short Top { get; private set; }
        public short Bottom { get; private set; }

        public Ratio(short top, short bottom)
        {
            IsDefined = top != 0 && bottom != 0;
            IsNegative = top < 0 != bottom < 0;

            Top = Math.Abs(top);
            Bottom = Math.Abs(bottom);

            if (Top > 0 && Bottom > 0)
            {
                bool simplified = false;

                do
                {
                    // TODO: Simplify.
                } while (simplified);
            }
        }

        public bool IsUnit()
        {
            return IsDefined && Top == Bottom;
        }

        public bool IsZero()
        {
            return Top == 0 && Bottom == 0;
        }

        public Ratio Reciprocal()
        {
            return IsNegative ?
                new Ratio((short)-Bottom, Top) :
                new Ratio(Bottom, Top);
        }

        public Ratio InvertSign()
        {
            return IsNegative ?
                new Ratio(Top, Bottom) :
                new Ratio((short)-Top, Bottom);
        }

        public Ratio NegativeReciprocal()
        {
            return IsNegative ?
                new Ratio(Bottom, Top) :
                new Ratio((short)-Bottom, Top);
        }
    }
}
