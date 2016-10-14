using System.IO;
using System.Text;

namespace WvsData
{
    internal static class ReactorExport
    {
        public static void Export()
        {
            using (FileStream stream = File.Create("data/Reactors.bin"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                {

                }
            }
        }
    }
}
