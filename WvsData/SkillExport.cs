using System.IO;
using System.Text;

namespace WvsData
{
    internal static class SkillExport
    {
        public static void Export()
        {
            using (FileStream stream = File.Create("data/Skills.bin"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                {

                }
            }
        }
    }
}
