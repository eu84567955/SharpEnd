using reNX;
using reNX.NXProperties;
using SharpEnd.Drawing;
using SharpEnd.Maps;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class MapDataProvider
    {
        private Dictionary<int, Map> m_maps;

        public MapDataProvider()
        {
            m_maps = new Dictionary<int, Map>();
        }

        private static List<string> skippedCategories = new List<string>()
        {
            "AdditionalInfo_JP.img", "AreaCode.img", "FieldGenerator.img", "Graph.img"
        };

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "Map.nx")))
            {
                foreach (var category in file.BaseNode["Map"])
                {
                    if (skippedCategories.Contains(category.Name))
                    {
                        continue;
                    }

                    foreach (var node in category)
                    {
                        int identifier = node.GetIdentifier();

                        Map map = new Map(identifier);

                        var infoNode = node["info"];

                        // TODO: Fetch info from infoNode

                        if (node.ContainsChild("portal"))
                        {
                            LoadPortals(map, node["portal"]);
                        }

                        m_maps.Add(identifier, map);
                    }
                }
            }

            Console.WriteLine($"Loaded {m_maps.Count} maps.");
        }

        private void LoadPortals(Map map, NXNode portalNode)
        {
            byte identifier = 0;

            foreach (var node in portalNode)
            {
                PortalData portal = new PortalData();

                portal.Identifier = identifier++;
                portal.Label = node.GetString("pn");
                portal.DestinationMap = node.GetInt("tm");
                portal.DestinationLabel = node.GetString("tn");
                portal.Position = new Point(node.GetShort("x"), node.GetShort("y"));

                map.Portals.Add(portal);
            }
        }

        public Map this[int identifier]
        {
            get
            {
                return m_maps.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class PortalData
    {
        public byte Identifier { get; set; }
        public string Label { get; set; }
        public int DestinationMap { get; set; }
        public string DestinationLabel { get; set; }
        public Point Position { get; set; }
    }
}