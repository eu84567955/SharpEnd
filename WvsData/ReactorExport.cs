using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WvsData
{
    internal static class ReactorExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            Console.Write(" > Exporting reactors... ");

            Dictionary<int, ReactorData> reactors = new Dictionary<int, ReactorData>();

            using (NXFile file = new NXFile(inputPath))
            {
                foreach (NXNode node in file.BaseNode)
                {
                    if (!node.ContainsChild("info"))
                    {
                        continue;
                    }

                    int identifier = node.GetIdentifier<int>();

                    if (reactors.ContainsKey(identifier))
                    {
                        continue;
                    }

                    ReactorData reactor = new ReactorData();

                    reactor.Identifier = identifier;

                    reactors.Add(identifier, reactor);
                }
            }

            foreach (ReactorData reactor in reactors.Values)
            {
                using (FileStream stream = File.Create(Path.Combine(outputPath, reactor.Identifier.ToString() + ".shd")))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                    {
                        reactor.Save(writer);
                    }
                }
            }

            Console.WriteLine("\tDone ({0}).", reactors.Count);
        }
    }
}