using reNX;
using reNX.NXProperties;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Data
{
    internal sealed class ValidCharDataProvider
    {
        private Dictionary<ushort, ValidCharData> m_validChars;

        public ValidCharDataProvider()
        {
            m_validChars = new Dictionary<ushort, ValidCharData>();
        }

        public void Load()
        {
            using (NXFile file = new NXFile(Path.Combine(Application.NXPath, "Etc.nx")))
            {
                foreach (var node in file.BaseNode["MakeCharInfo.img"])
                {
                    ushort job;

                    if (ushort.TryParse(node.Name, out job))
                    {
                        ValidCharData validChar = new ValidCharData();

                        if (node.ContainsChild("male"))
                        {
                            foreach (var category in node["male"])
                            {
                                foreach (var objectt in category)
                                {
                                    if (objectt.Name == "color")
                                    {
                                        continue;
                                    }

                                    validChar.Male.Add((int)objectt.ValueOrDie<long>());
                                }
                            }
                        }

                        if (node.ContainsChild("female"))
                        {
                            foreach (var category in node["female"])
                            {
                                foreach (var objectt in category)
                                {
                                    if (objectt.Name == "color")
                                    {
                                        continue;
                                    }

                                    validChar.Female.Add((int)objectt.ValueOrDie<long>());
                                }
                            }
                        }

                        m_validChars.Add(job, validChar);
                    }
                    else
                    {
                        // NOTE: Some nodes contain either "_Dummy" suffix or another weird suffix.
                        // Until we figure out their purpose, we're skipping them.

                        continue;
                    }
                }
            }
        }

        public bool Validate(ushort job, List<int> objects)
        {
            return true;
        }
    }

    internal sealed class ValidCharData
    {
        public List<int> Male { get; private set; }
        public List<int> Female { get; private set; }

        public ValidCharData()
        {
            Male = new List<int>();
            Female = new List<int>();
        }
    }
}

