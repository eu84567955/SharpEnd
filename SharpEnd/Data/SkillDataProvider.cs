using reNX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class SkillDataProvider
    {
        private Dictionary<int, SkillData> m_playerSkills;

        public SkillDataProvider()
        {
            m_playerSkills = new Dictionary<int, SkillData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "Skill.nx")))
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