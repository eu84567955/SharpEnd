using SharpEnd.Drawing;
using System.Collections.Generic;

namespace SharpEnd.Game.Maps
{
    internal sealed class MapFootholds : List<Foothold>
    {
        public Map Map { get; private set; }

        public MapFootholds(Map map)
            : base()
        {
            Map = map;
        }

        public ushort FindBelow(Point position)
        {
            short x = position.X;
            short y = position.Y;
            short closestValue = short.MaxValue;
            Foothold foundFoothold = null;

            foreach (Foothold foothold in this)
            {
                Line line = foothold.Line;

                if (line.WithinRangeX(x))
                {
                    short yInterpolation = line.InterpolateForY(x);

                    if (yInterpolation <= closestValue && yInterpolation >= y)
                    {
                        closestValue = yInterpolation;
                        foundFoothold = foothold;
                    }
                }
            }

            return foundFoothold.Identifier;
        }

        public Point FindFloor(Point position, short heightModifier, Rectangle searchArea = null)
        {
            short x = position.X;
            short y = (short)(position.Y + heightModifier);
            short closestValue = short.MaxValue;
            Foothold foundFoothold = null;

            foreach (Foothold foothold in this)
            {
                Line line = foothold.Line;

                if (line.WithinRangeX(x))
                {
                    if (searchArea != null)
                    {
                        if (!searchArea.ContainsAnyPartOfLine(line))
                        {
                            continue;
                        }
                    }

                    short yInterpolation = line.InterpolateForY(x);

                    if (yInterpolation <= closestValue && yInterpolation >= y)
                    {
                        closestValue = yInterpolation;
                        foundFoothold = foothold;
                    }
                }
            }

            return new Point(x, closestValue);
        }
    }
}