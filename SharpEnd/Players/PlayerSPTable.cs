using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal sealed class PlayerSPTable : Dictionary<byte, int>
    {
        public Player Parent { get; private set; }

        public PlayerSPTable(Player parent, DatabaseQuery query)
            : base()
        {
            Parent = parent;

            while (query.NextRow())
            {
                byte advancement = query.Get<byte>("advancement");
                int points = query.Get<int>("points");

                Add(advancement, points);
            }
        }

        public void Save()
        {
            foreach (KeyValuePair<byte, int> entry in this)
            {
                // TODO: Save.
            }
        }

        public void WriteGeneral(OutPacket outPacket)
        {
            outPacket.WriteByte((byte)Count);

            foreach (KeyValuePair<byte, int> entry in this)
            {
                outPacket
                    .WriteByte(entry.Key)
                    .WriteInt(entry.Value);
            }
        }

        public void SetSkillPoints(byte advancement, int value)
        {
            this[advancement] = value;

            Parent.Send(PlayerPackets.PlayerUpdate(Parent, EPlayerUpdate.SkillPoints));
        }
    }
}
