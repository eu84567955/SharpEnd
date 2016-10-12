using SharpEnd.Data;
using SharpEnd.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace SharpEnd.Maps
{
    internal sealed class MapFootholds : List<FootholdData>
    {
        public Map Map { get; private set; }

        public MapFootholds(Map map)
            : base()
        {
            Map = map;
        }

        public Point FindBelow(Point point, short heightModifier)
        {
            short x = point.X;
            short y = (short)(point.Y + heightModifier);

            var validFootholds = this.Where(f => x >= f.Point1.X && x <= f.Point2.X && f.Point1.Y >= y && f.Point2.Y >= y);

            if (validFootholds.Any())
            {
                foreach (FootholdData foothold in validFootholds.OrderBy(f => f.Point1.Y < f.Point2.Y ? f.Point1.Y : f.Point2.Y))
                {
                    if (foothold.Point1.Y != foothold.Point2.Y)
                    {
                        // TODO: Diagonal footholds.
                    }
                    else
                    {
                        return new Point(x, foothold.Point1.Y);
                    }
                }
            }

            return null;
        }
    }
}
