using reNX;
using SharpEnd.Drawing;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class MobSkillData
    {
        public int mpCon;
        public int summonEffect;
        public int hp;
        public int x;
        public int y;
        public int time;
        public int interval;
        public int prop;
        public short limit;
        public Point lt;
        public Point rb;
        public bool summonOnce;
    }

    internal sealed class SkillDataProvider
    {
        private Dictionary<int, SkillData> m_playerSkills;

        public SkillDataProvider()
        {
            m_playerSkills = new Dictionary<int, SkillData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine("nx", "Skill.nx")))
            {
                foreach (var job in file.BaseNode)
                {
                    int jobId;

                    if (int.TryParse(job.Name.Replace(".img", ""), out jobId))
                    {
                        foreach (var node in job["skill"])
                        {
                            int identifier = node.GetIdentifier<int>();

                            SkillData item = new SkillData();

                            item.Identifier = identifier;

                            m_playerSkills.Add(identifier, item);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public SkillData this[int identifier]
        {
            get
            {
                return m_playerSkills.GetOrDefault(identifier, null);
            }
        }
    }

    internal sealed class SkillData
    {
        public int Identifier { get; set; }
    }
}