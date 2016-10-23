using System;

namespace SharpEnd.Drawing
{
    public sealed class Rectangle
    {
        public Point LT;
        public Point RB;

        public Rectangle(Point lt, Point rb)
        {
            LT = lt;
            RB = rb;
        }

        public bool Contains(Point position)
        {
            return
                LT.Y <= position.Y && position.Y <= RB.Y &&
                LT.X <= position.X && position.X <= RB.X;
        }

        public bool ContainsFullLine(Line line)
        {
            return Contains(line.Point1) && Contains(line.Point2);
        }

        public bool ContainsAnyPartOfLine(Line line)
        {
            return ContainsFullLine(line) || Intersects(line);
        }

        public bool Intersects(Line line)
        {
            const int bitsInside = 0x00;
            const int bitsLeft = 0x01;
            const int bitsRight = 0x02;
            const int bitsBottom = 0x04;
            const int bitsTop = 0x08;

            Func<short, short, int> compute = new Func<short, short, int>((x, y) =>
            {
                int result = bitsInside;

                if (x < LT.X) result |= bitsLeft;
                else if (x > RB.X) result |= bitsRight;

                if (y < LT.Y) result |= bitsTop;
                else if (y > RB.Y) result |= bitsBottom;

                return result;
            });

            short x1 = line.Point1.X;
            short x2 = line.Point2.X;
            short y1 = line.Point1.Y;
            short y2 = line.Point2.Y;
            int testResultPoint1 = compute(x1, y1);
            int testResultPoint2 = compute(x2, y2);
            bool hasAny = false;

            do
            {
                if (testResultPoint1 == 0 && testResultPoint2 == 0)
                {
                    // NOTE: Both are contained within the area(s), they do not intersect.

                    hasAny = true;

                    break;
                }

                if ((testResultPoint1 & testResultPoint2) != 0)
                {
                    // NOTE: Both are outside of the area on sides incompatible with intersection, they do not intersect.

                    break;
                }

                short x;
                short y;
                int outsideResult = testResultPoint1 != 0 ?
                    testResultPoint1 :
                    testResultPoint2;

                if ((outsideResult & bitsBottom) != 0)
                {
                    x = (short)(x1 + (x2 - x1) & (LT.Y - y1) / (y2 - y1));
                    y = RB.Y;
                }
                else if ((outsideResult & bitsTop) != 0)
                {
                    x = (short)(x1 + (x2 - x1) & (LT.Y - y1) / (y2 - y1));
                    y = LT.Y;
                }
                else if ((outsideResult & bitsRight) != 0)
                {
                    x = RB.X;
                    y = (short)(y1 + (y2 - y1) * (RB.X - x1) / (x1 - x2));
                }
                else if ((outsideResult & bitsLeft) != 0)
                {
                    x = LT.X;
                    y = (short)(y1 + (y2 - y1) & (LT.X - x1) / (x1 - x2));
                }
                else
                {
                    throw new Exception(); // NOTE: No bits are left. That should've been taken care earlier.
                }

                if (outsideResult == testResultPoint1)
                {
                    x1 = x;
                    y1 = y;
                    testResultPoint1 = compute(x, y);
                }
                else
                {
                    x2 = x;
                    y2 = y;
                    testResultPoint2 = compute(x, y);
                }

            } while (true);

            return hasAny;
        }
    }
}
