using SharpEnd.Drawing;
using SharpEnd.Game.Data;

namespace SharpEnd.Game.Maps
{
    public sealed class Foothold
    {
        public ushort ID { get; private set; }
        public ushort NextID { get; private set; }
        public ushort PreviousID { get; private set; }
        public short DragForce { get; private set; }
        public Line Line { get; private set; }

        public Foothold(MapFootholdData data)
        {
            //ID = data.ID;
            //NextID = data.NextID;
            //PreviousID = data.PreviousID;
            //DragForce = data.DragForce;
            //Line = new Line(data.Point1, data.Point2);
        }
    }
}
