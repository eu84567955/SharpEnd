using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using WvsDump.Utility;

namespace WvsDump
{
    internal static class ReactorExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "Reactor.nx");
            outputPath = Path.Combine(outputPath, "reactors.bin");

            List<ReactorData> reactors = new List<ReactorData>();

            using (NXFile file = new NXFile(inputPath))
            {
                Application.ResetCounter(file.BaseNode.ChildCount);

                foreach (NXNode node in file.BaseNode)
                {
                    int reactorID = node.GetID<int>();

                    ReactorData reactor = new ReactorData();

                    reactor.ID = reactorID;

                    reactor.Events = new List<ReactorEventData>();
                    foreach (var a in node)
                    {
                        if (a.ContainsChild("event"))
                        {
                            foreach (NXNode eventNode in a["event"])
                            {
                                ReactorEventData evt = new ReactorEventData();

                                evt.State = eventNode.GetByte("state");
                                evt.Type = eventNode.GetShort("type");

                                reactor.Events.Add(evt);
                            }
                        }
                    }

                    reactors.Add(reactor);
                    ++dataCount;
                    ++Application.AllDataCounter;
                    Application.IncrementCounter();
                }
            }

            using (FileStream stream = File.Create(outputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(reactors.Count);
                    reactors.ForEach(r => r.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Reactors", dataCount, timer.Duration);
        }
    }
}
