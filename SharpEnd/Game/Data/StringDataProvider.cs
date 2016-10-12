using reNX;
using reNX.NXProperties;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class StringDataProvider
    {
        private Dictionary<string, int> m_maps;

        public StringDataProvider()
        {
            m_maps = new Dictionary<string, int>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "String.nx")))
            {
                LoadMaps(file.BaseNode["Map.img"]);
            }
        }

        private void LoadMaps(NXNode mapNode)
        {
            foreach (var category in mapNode)
            {
                foreach (var node in category)
                {
                    int identifier = node.GetIdentifier<int>();
                    string mapName = node.GetString("mapName");
                    string streetName = node.GetString("streetName");

                    string name = null;

                    if (!string.IsNullOrEmpty(streetName))
                    {
                        name = string.Format("{0} - {1}", streetName, mapName);
                    }
                    else
                    {
                        name = mapName;
                    }

                    if (m_maps.ContainsKey(name))
                    {
                        // NOTE: Some map strings are duplicated.

                        continue;
                    }

                    m_maps.Add(name, identifier);
                }
            }
        }

        public Dictionary<string, int> GetMaps(string keyword)
        {
            Dictionary<string, int> maps = new Dictionary<string, int>();

            foreach (var map in m_maps)
            {
                if (map.Key.ToLower().Contains(keyword))
                {
                    maps.Add(map.Key, map.Value);
                }
            }

            return maps;
        }
    }
}
