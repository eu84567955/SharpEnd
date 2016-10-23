using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    internal sealed class BeautyDataProvider
    {
        private static BeautyDataProvider instance;

        public static BeautyDataProvider Instance { get { return instance ?? (instance = new BeautyDataProvider()); } }

        private List<byte> m_skins;
        private List<int> m_faces;
        private List<int> m_hairs;

        private BeautyDataProvider()
        {
            m_skins = new List<byte>();
            m_faces = new List<int>();
            m_hairs = new List<int>();
        }

        public void LoadData()
        {
            using (FileStream stream = File.OpenRead(Path.Combine("data", "beauty.bin")))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                {
                    LoadSkins(reader);
                    LoadFaces(reader);
                    LoadHairs(reader);
                }
            }
        }

        private void LoadSkins(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            while (count-- > 0)
            {
                m_skins.Add(reader.ReadByte());
            }
        }

        private void LoadFaces(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            while (count-- > 0)
            {
                m_faces.Add(reader.ReadInt32());
            }
        }

        private void LoadHairs(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            while (count-- > 0)
            {
                m_hairs.Add(reader.ReadInt32());
            }
        }
    }
}
