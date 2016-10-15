namespace SharpEnd.Drawing
{
    internal sealed class Line
    {
        public Point Point1 { get; private set; }
        public Point Point2 { get; private set; }

        public Line(Point pt1, Point pt2)
        {
            Point1 = pt1;
            Point2 = pt2;
        }

        public Ratio Slope()
        {
            short rise = (short)(Point2.Y - Point1.Y);
            short run = (short)(Point2.X - Point1.X);

            return new Ratio(rise, run);
        }

        public bool Contains(Point position)
        {
            return SlopeContains(position) && (Point1.X != Point2.X) ?
               WithinRangeX(position.X) :
               WithinRangeY(position.Y);
        }

        public bool SlopeContains(Point position)
        {
            var slope1 = (Point2.X - Point1.X) * (position.Y - Point1.Y);
            var slope2 = (position.X - Point1.X) * (Point2.Y - Point1.Y);

            return slope1 == slope2;
        }

        public bool WithinRangeX(short x)
        {
            return (Point1.X < x && x <= Point2.X) ||
                   (Point2.X < x && x <= Point1.X);
        }

        public bool WithinRangeY(short y)
        {
            return (Point1.Y <= y && y <= Point2.Y) ||
                   (Point2.Y <= y && y <= Point1.Y);
        }

        public short InterpolateForX(short y)
        {
            var lineSlope = Slope();

            if (!lineSlope.IsDefined)
            {
                if (WithinRangeY(y))
                {
                    return Point1.X;
                }

                return 0;
            }

            int difference = y - Point1.Y;
            difference *= lineSlope.Bottom;
            difference /= lineSlope.Top;

            if (lineSlope.IsNegative) difference *= -1;

            return (short)(difference + Point1.X);
        }

        public short InterpolateForY(short x)
        {
            var lineSlope = Slope();

            if (lineSlope.IsZero())
            {
                if (WithinRangeX(x))
                {
                    return Point1.Y;
                }
            }

            int difference = x - Point1.X;
            difference *= lineSlope.Top;
            difference /= lineSlope.Bottom;

            if (lineSlope.IsNegative) difference *= -1;

            return (short)(difference + Point1.Y);
        }
    }
}
