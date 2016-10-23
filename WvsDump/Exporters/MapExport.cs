using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using WvsDump.Utility;
using System;

namespace WvsDump
{
    internal static class MapExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "Map.nx");
            outputPath = Path.Combine(outputPath, "maps.bin");

            List<MapData> maps = new List<MapData>();

            using (NXFile file = new NXFile(inputPath))
            {
                int count = file.BaseNode["Map"].GetCount();

                Application.ResetCounter(count);

                foreach (NXNode category in file.BaseNode["Map"])
                {
                    if (category.Name.Contains(".img"))
                    {
                        continue;
                    }

                    foreach (NXNode node in category)
                    {
                        int mapID = node.GetID<int>();

                        MapData map = new MapData();

                        NXNode infoNode = node["info"];

                        map.ID = mapID;
                        map.Town = infoNode.GetBoolean("town");
                        map.Swim = infoNode.GetBoolean("swim");
                        map.Clock = infoNode.GetBoolean("clock");
                        map.Everlasting = infoNode.GetBoolean("everlast"); // TODO: Needs validation.
                        map.PersonalShop = infoNode.GetBoolean("shop"); // TODO: Needs validation.
                        map.AllMoveCheck = infoNode.GetBoolean("allMoveCheck"); // TODO: Needs validation.
                        map.Recovery = infoNode.GetDouble("recovery"); // TODO: Needs validation.
                        map.ReturnMap = infoNode.GetInt("returnMap");
                        map.ForcedMap = infoNode.GetInt("forcedReturn");
                        map.Limit = infoNode.GetInt("fieldLimit"); // TODO: Needs validation.
                        map.AutoDecHp = infoNode.GetInt("autoDecHp"); // TODO: Needs validation.
                        map.AutoDecMp = infoNode.GetInt("autoDecMp"); // TODO: Needs validation.
                        map.ProtectItem = infoNode.GetInt("protectItem"); // TODO: Needs validation.

                        map.Seats = new List<MapSeatData>();
                        if (node.ContainsChild("seat"))
                        {
                            foreach (NXNode seatNode in node["seat"])
                            {
                                if (seatNode.Name == "offset" ||
                                    seatNode.Name == "sitDir")
                                {
                                    continue;
                                }

                                MapSeatData seat = new MapSeatData();

                                seat.ID = seatNode.GetID<short>();

                                Point point = seatNode.ValueOrDie<Point>();

                                seat.X = (short)point.X;
                                seat.Y = (short)point.Y;

                                map.Seats.Add(seat);
                            }
                        }

                        if (node.ContainsChild("portal"))
                        {
                            foreach (NXNode portalNode in node["portal"])
                            {
                                byte portalID = portalNode.GetID<byte>();

                                MapPortalData portal = new MapPortalData();

                                portal.ID = portalID;
                                portal.X = portalNode.GetShort("x");
                                portal.Y = portalNode.GetShort("y");
                                portal.Type = portalNode.GetInt("pt");
                                portal.DestinationMap = portalNode.GetInt("tm");
                                portal.DestinationName = portalNode.GetString("tn");
                                portal.Name = portalNode.GetString("pn");
                                portal.Script = portalNode.GetString("script");

                                map.Portals.Add(portal);
                            }
                        }

                        if (node.ContainsChild("life"))
                        {
                            foreach (NXNode lifeNode in node["life"])
                            {
                                if (!lifeNode.ContainsChild("type"))
                                {
                                    continue;
                                }

                                MapSpawnData spawn = new MapSpawnData();

                                spawn.Type = lifeNode.GetString("type")[0];
                                spawn.ID = int.Parse(lifeNode.GetString("id"));
                                spawn.Flip = lifeNode.GetBoolean("f");
                                spawn.Hide = lifeNode.GetBoolean("hide");
                                spawn.X = lifeNode.GetShort("x");
                                spawn.Y = lifeNode.GetShort("y");
                                spawn.CY = lifeNode.GetShort("cy");
                                spawn.Foothold = lifeNode.GetShort("fh");
                                spawn.RX0 = lifeNode.GetShort("rx0");
                                spawn.RX1 = lifeNode.GetShort("rx1");
                                spawn.MobTime = lifeNode.GetInt("mobTime");

                                map.Spawns.Add(spawn);
                            }
                        }

                        if (node.ContainsChild("foothold"))
                        {
                            foreach (NXNode layout in node["foothold"])
                            {
                                foreach (NXNode layer in layout)
                                {
                                    foreach (NXNode footholdNode in layer)
                                    {
                                        MapFootholdData foothold = new MapFootholdData();

                                        foothold.ID = footholdNode.GetID<short>();
                                        foothold.X1 = footholdNode.GetShort("x1");
                                        foothold.Y1 = footholdNode.GetShort("y1");
                                        foothold.X2 = footholdNode.GetShort("x2");
                                        foothold.Y2 = footholdNode.GetShort("y2");

                                        map.Footholds.Add(foothold);
                                    }
                                }
                            }
                        }

                        if (node.ContainsChild("reactor"))
                        {
                            foreach (NXNode reactorNode in node["reactor"])
                            {
                                MapReactorData reactor = new MapReactorData();

                                reactor.ID = reactorNode.GetInt("id");
                                reactor.Flip = reactorNode.GetBoolean("f");
                                reactor.X = reactorNode.GetShort("x");
                                reactor.Y = reactorNode.GetShort("y");
                                reactor.ReactorTime = reactorNode.GetInt("reactorTime");
                                reactor.Name = reactorNode.GetString("name");

                                map.Reactors.Add(reactor);
                            }
                        }

                        maps.Add(map);
                        ++dataCount;
                        ++Application.AllDataCounter;
                        Application.IncrementCounter();
                    }
                }
            }

            using (FileStream stream = File.Create(outputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(maps.Count);
                    maps.ForEach(m => m.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Maps", dataCount, timer.Duration);
        }
    }
}
