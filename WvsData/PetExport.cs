using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WvsData
{
    internal static class PetExport
    {
        public static void Export(NXNode petNode, string outputPath)
        {
            Console.Write(" > Exporting pets... ");

            Dictionary<int, PetData> pets = new Dictionary<int, PetData>();

            foreach (NXNode node in petNode)
            {
                if (!node.ContainsChild("info"))
                {
                    continue;
                }

                int identifier = node.GetIdentifier<int>();

                if (pets.ContainsKey(identifier))
                {
                    continue;
                }

                var infoNode = node["info"];

                PetData pet = new PetData();

                pet.Identifier = identifier;
                pet.IsMultiPet = infoNode.GetBoolean("multiPet");
                pet.IsPermanent = infoNode.GetBoolean("permanent");
                pet.HasPickupItem = infoNode.GetBoolean("pickupItem");
                pet.HasAutoBuff = infoNode.GetBoolean("autoBuff");
                pet.Life = infoNode.GetInt("life");
                pet.Hunger = infoNode.GetInt("hungry");

                pets.Add(identifier, pet);
            }

            foreach (PetData pet in pets.Values)
            {
                using (FileStream stream = File.Create(Path.Combine(outputPath, pet.Identifier.ToString() + ".shd")))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
                    {
                        pet.Save(writer);
                    }
                }
            }

            Console.WriteLine("\t\tDone ({0}).", pets.Count);
        }
    }
}