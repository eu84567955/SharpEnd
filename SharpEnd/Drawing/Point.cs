using System;

namespace SharpEnd.Drawing
{
    public sealed class Point
    {
        private short m_x;
        private short m_y;

        public Point(short x, short y)
        {
            m_x = x;
            m_y = y;
        }

        public Point(int x, int y)
        {
            m_x = (short)x;
            m_y = (short)y;
        }

        public Point(Point point)
        {
            m_x = point.X;
            m_y = point.Y;
        }

        public double DistanceFrom(Point point)
        {
            return Math.Sqrt((Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2)));
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static implicit operator short(Point v)
        {
            throw new NotImplementedException();
        }

        public short X { get { return m_x; } set { m_x = value; } }
        public short Y { get { return m_y; } set { m_y = value; } }
    }
}
