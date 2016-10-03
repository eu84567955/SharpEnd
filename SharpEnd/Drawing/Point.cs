namespace SharpEnd.Drawing
{
    internal sealed class Point
    {
        public short X { get; set; }
        public short Y { get; set; }

        public Point(short x, short y)
        {
            X = x;
            Y = y;
        }
    }
}
