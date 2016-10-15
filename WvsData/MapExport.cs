using reNX;
using reNX.NXProperties;
using SharpEnd.Data;
using SharpEnd.Drawing;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static SharpEnd.Game.Data.MapData;

namespace WvsData
{
    internal static class MapExport
    {
        public static void Export(string path)
        {
            Dictionary<int, MapData> maps = new Dictionary<int, MapData>();

            using (FileStream stream = File.Create("data/Maps.bin"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                {
                    using (NXFile file = new NXFile(path + "/Map.nx"))
                    {
                        foreach (NXNode category in file.BaseNode["Map"])
                        {
                            if (category.Name.EndsWith(".img"))
                            {
                                continue;
                            }

                            foreach (NXNode node in category)
                            {
                                if (!node.ContainsChild("info"))
                                {
                                    continue;
                                }

                                int identifier = node.GetIdentifier<int>();

                                if (maps.ContainsKey(identifier))
                                {
                                    Console.WriteLine("Duplicate map {0}", identifier);

                                    continue;
                                }

                                NXNode infoNode = node["info"];

                                MapData map = new MapData();

                                map.Identifier = identifier;
                                map.ShuffleName = infoNode.GetString("shuffleName");
                                map.Music = infoNode.GetString("bgm");
                                map.ReturnMapIdentifier = infoNode.GetInt("returnMap");
                                map.ForcedReturnMapIdentifier = infoNode.GetInt("forcedReturn");
                                map.EntryScript = infoNode.GetString("onUserEnter");
                                map.InitialEntryScript = infoNode.GetString("onFirstUserEnter");

                                map.Footholds = new List<MapFootholdData>();
                                if (node.ContainsChild("foothold"))
                                {
                                    ExportFootholds(map, node["foothold"]);
                                }

                                map.Mobs = new List<MapMobData>();
                                map.Npcs = new List<MapNpcData>();
                                if (node.ContainsChild("life"))
                                {
                                    ExportSpawns(map, node["life"]);
                                }

                                map.Portals = new List<MapPortalData>();
                                if (node.ContainsChild("portal"))
                                {
                                    ExportPortals(map, node["portal"]);
                                }

                                if (node.ContainsChild("reactor"))
                                {
                                    ExportReactors(map, node["reactor"]);
                                }

                                map.Seats = new List<MapSeatData>();
                                if (node.ContainsChild("seat"))
                                {
                                    ExportSeats(map, node["seat"]);
                                }

                                maps.Add(identifier, map);
                            }
                        }
                    }

                    writer.Write(maps.Count);

                    foreach (MapData map in maps.Values)
                    {
                        map.Write(writer);
                    }

                    Console.WriteLine("Maps: {0}", maps.Count);
                }
            }
        }

        private static void ExportFootholds(MapData data, NXNode footholdNode)
        {
            foreach (var layer in footholdNode)
            {
                foreach (var category in layer)
                {
                    foreach (var node in category)
                    {
                        MapFootholdData foothold = new MapFootholdData();

                        foothold.Identifier = node.GetIdentifier<ushort>();
                        foothold.NextIdentifier = node.GetUShort("next");
                        foothold.PreviousIdentifier = node.GetUShort("prev");
                        foothold.DragForce = 0; // TODO.
                        foothold.Point1 = new Point(node.GetShort("x1"), node.GetShort("y1"));
                        foothold.Point2 = new Point(node.GetShort("x2"), node.GetShort("y2"));

                        data.Footholds.Add(foothold);
                    }
                }
            }
        }

        private static void ExportSpawns(MapData map, NXNode lifeNode)
        {
            // NOTE: Some maps have life in categories.
            // We're going to skip them for now until we figure this one out.
            if (lifeNode.ContainsChild("isCategory"))
            {
                return;
            }

            foreach (var node in lifeNode)
            {
                switch (node.GetString("type"))
                {
                    case "m":
                        {
                            MapMobData mob = new MapMobData();

                            mob.Identifier = int.Parse(node.GetString("id"));
                            mob.Position = new Point(node.GetShort("x"), node.GetShort("cy"));
                            mob.Foothold = node.GetUShort("fh");
                            mob.RespawnTime = node.GetInt("mobTime");
                            mob.Flip = node.GetBoolean("f");
                            mob.Hide = node.GetBoolean("hide");

                            map.Mobs.Add(mob);

                        }
                        break;

                    case "n":
                        {
                            MapNpcData npc = new MapNpcData();

                            npc.Identifier = int.Parse(node.GetString("id"));
                            npc.Position = new Point(node.GetShort("x"), node.GetShort("cy"));
                            npc.Foothold = node.GetUShort("fh");
                            npc.MinimumClickX = node.GetShort("rx0");
                            npc.MaximumClickX = node.GetShort("rx1");
                            npc.Flip = node.GetBoolean("f");
                            npc.Hide = node.GetBoolean("hide");

                            map.Npcs.Add(npc);
                        }
                        break;
                }
            }
        }

        private static void ExportPortals(MapData map, NXNode portalNode)
        {
            foreach (var node in portalNode)
            {
                try
                {
                    MapPortalData portal = new MapPortalData();

                    portal.Identifier = node.GetIdentifier<sbyte>();
                    portal.Position = new Point(node.GetShort("x"), node.GetShort("y"));
                    portal.Label = node.GetString("pn");
                    portal.ToMap = node.GetInt("tm");
                    portal.ToName = node.GetString("tn");
                    portal.Script = node.GetString("script");

                    map.Portals.Add(portal);
                }
                catch
                {
                    // NOTE: Some maps include way too many portals. We'll deal with them later.
                    // For now, we're skipping them.
                }
            }
        }

        private static void ExportReactors(MapData map, NXNode reactorNode)
        {
            // TODO: Reactors.
        }

        private static void ExportSeats(MapData map, NXNode seatNode)
        {
            // TODO: Seats.
        }
    }
}
