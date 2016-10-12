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
        private static List<string> skippedCategories = new List<string>()
        {
            "AdditionalInfo_JP.img", "AreaCode.img", "FieldGenerator.img", "Graph.img"
        };

        private Dictionary<int, Map> m_maps;

        public MapDataProvider()
        {
            m_maps = new Dictionary<int, Map>();
        }

        public bool Contains(int identifier)
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "Map.nx")))
            {
                return file.BaseNode["Map"][string.Format("Map{0}", identifier / 100000000)].ContainsChild(string.Format("{0}.img", identifier));
            }
        }

        public Map this[int identifier]
        {
            get
            {
                if (!m_maps.ContainsKey(identifier))
                {
                    LoadMap(identifier);

                    foreach (SpawnPoint spawnPoint in m_maps[identifier].SpawnPoints)
                    {
                        spawnPoint.Spawn();
                    }
                }

                return m_maps[identifier];
            }
        }

        private void LoadMap(int identifier)
        {
            Map map = new Map(identifier);

            using (NXFile file = new NXFile(Path.Combine("nx", "Map.nx")))
            {
                var node = file.BaseNode["Map"][string.Format("Map{0}", identifier / 100000000)][string.Format("{0}.img", identifier)];

                var infoNode = node["info"];

                // TODO: Fetch info from infoNode

                if (node.ContainsChild("foothold"))
                {
                    LoadFoothold(map, node["foothold"]);
                }

                if (node.ContainsChild("life"))
                {
                    LoadLife(map, node["life"]);
                }

                if (node.ContainsChild("portal"))
                {
                    LoadPortals(map, node["portal"]);
                }

                if (node.ContainsChild("reactor"))
                {
                    LoadReactors(map, node["reactor"]);
                }

                if (node.ContainsChild("seat"))
                {
                    LoadSeats(map, node["seat"]);
                }

                m_maps.Add(identifier, map);
            }
        }

        private void LoadFoothold(Map map, NXNode footholdNode)
        {
            foreach (var layer in footholdNode)
            {
                foreach (var category in layer)
                {
                    foreach (var node in category)
                    {
                        FootholdData foothold = new FootholdData();

                        foothold.Identifier = node.GetIdentifier<short>();
                        foothold.Next = node.GetShort("next");
                        foothold.Previous = node.GetShort("prev");
                        foothold.Point1 = new Point(node.GetShort("x1"), node.GetShort("y1"));
                        foothold.Point2 = new Point(node.GetShort("x2"), node.GetShort("y2"));

                        map.Footholds.Add(foothold);
                    }
                }
            }
        }

        private void LoadLife(Map map, NXNode lifeNode)
        {
            // NOTE: Some maps have life in categories.
            // We're going to skip them for now until we figure this one out.
            if (lifeNode.ContainsChild("isCategory"))
            {
                return;
            }

            foreach (var node in lifeNode)
            {
                int identifier = int.Parse(node.GetString("id")); // NOTE: Life identifiers are string
                Point position = new Point(node.GetShort("x"), node.GetShort("cy"));
                ushort foothold = node.GetUShort("fh");
                bool flip = node.GetBoolean("f");
                bool hide = node.GetBoolean("hide");

                switch (node.GetString("type"))
                {
                    case "n":
                        {
                            short cy = node.GetShort("cy");
                            short rx0 = node.GetShort("rx0");
                            short rx1 = node.GetShort("rx1");

                            Npc npc = new Npc(identifier, rx0, rx1, position, foothold, flip, hide);

                            map.Npcs.Add(npc);
                        }
                        break;

                    case "m":
                        {
                            int respawnTime = node.GetInt("mobTime");

                            SpawnPoint spawnPoint = new SpawnPoint(identifier, position, foothold, flip, hide);

                            map.SpawnPoints.Add(spawnPoint);
                        }
                        break;

                    case "r":
                        {
                            // NOTE: Reactors are no longer inside the life node.
                            // Instead, they have their own separate node called "reactor".
                        }
                        break;
                }
            }
        }

        // TODO: Use the "pt" property which indicates the type of the portal (spawn point, door, etcetera)
        private void LoadPortals(Map map, NXNode portalNode)
        {
            foreach (var node in portalNode)
            {
                try
                {
                    PortalData portal = new PortalData();

                    portal.Identifier = node.GetIdentifier<sbyte>();
                    portal.Label = node.GetString("pn");
                    portal.DestinationMap = node.GetInt("tm");
                    portal.DestinationLabel = node.GetString("tn");
                    portal.Position = new Point(node.GetShort("x"), node.GetShort("y"));
                    portal.Script = node.GetString("script");

                    if (portal.Label == "sp")
                    {
                        map.Portals.SpawnPoints.Add(portal);
                    }
                    else
                    {
                        map.Portals.Regular.Add(portal);
                    }
                }
                catch
                {
                    // NOTE: Some maps include way too many portals. We'll deal with them later.
                    // For now, we're skipping them.
                }
            }
        }

        private void LoadReactors(Map map, NXNode reactorNode)
        {
            foreach (var node in reactorNode)
            {
                int identifier = int.Parse(node.GetString("id")); // NOTE: Reactor identifiers are string
                string label = node.GetString("name");
                int time = node.GetInt("reactorTime");
                Point position = new Point(node.GetShort("x"), node.GetShort("cy"));
                bool flip = node.GetBoolean("f");

                Reactor reactor = new Reactor(identifier, label, time, position, 0, flip, false);

                map.Reactors.Add(reactor);
            }
        }

        private void LoadSeats(Map map, NXNode seatNode)
        {
            foreach (var node in seatNode)
            {
                SeatData seat = new SeatData();

                seat.Identifier = node.GetIdentifier<short>();
                seat.Position = new Point(0, 0); // TODO: NXValuedNode<Point>, somehow.

                map.Seats.Add(seat);
            }
        }
    }

    internal sealed class FootholdData
    {
        public short Identifier { get; set; }
        public short Previous { get; set; }
        public short Next { get; set; }
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }

        public bool IsWall
        {
            get
            {
                return Point1.X == Point2.X;
            }
        }
    }

    internal sealed class PortalData
    {
        public sbyte Identifier { get; set; }
        public string Label { get; set; }
        public int DestinationMap { get; set; }
        public string DestinationLabel { get; set; }
        public Point Position { get; set; }
        public string Script { get; set; }
    }

    internal sealed class SeatData
    {
        public short Identifier { get; set; }
        public Point Position { get; set; }
    }
}