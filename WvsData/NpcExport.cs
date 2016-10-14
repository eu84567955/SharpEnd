using System.IO;
using System.Text;

namespace WvsData
{
    internal static class NpcExport
    {
        public static void Export()
        {
            using (FileStream stream = File.Create("data/Npcs.bin"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                {

                }
            }
        }
    }
}
