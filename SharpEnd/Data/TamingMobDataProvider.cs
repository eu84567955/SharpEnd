using reNX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class TamingMobDataProvider
    {
        private Dictionary<int, TamingMobData> m_tamingMobs;

        public TamingMobDataProvider()
        {
            m_tamingMobs = new Dictionary<int, TamingMobData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "TamingMob.nx")))
            {
                foreach (var node in file.BaseNode)
                {
                    int identifier = node.GetIdentifier<int>();

                    TamingMobData item = new TamingMobData();

                    item.Identifier = identifier;

                    var infoNode = node["info"];

                    item.Fatigue = infoNode.GetInt("fatigue");
                    item.FS = infoNode.GetDouble("fs");
                    item.Jump = infoNode.GetInt("jump");
                    item.Speed = infoNode.GetInt("speed");
                    item.Swim = infoNode.GetDouble("swim");

                    m_tamingMobs.Add(identifier, item);
                }
            }

            Log.Inform($"Loaded {m_tamingMobs.Count} taming mobs.");
        }

        public TamingMobData this[int identifier]
        {
            get
            {
                return m_tamingMobs.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class TamingMobData
    {
        public int Identifier { get; set; }
        public int Fatigue { get; set; }
        public double FS { get; set; }
        public int Jump { get; set; }
        public int Speed { get; set; }
        public double Swim { get; set; }
    }
}