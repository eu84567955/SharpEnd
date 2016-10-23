using reNX;
using reNX.NXProperties;
using SharpEnd.Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using WvsDump.Utility;

namespace WvsDump
{
    internal static class QuestExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            PerformanceTimer timer = new PerformanceTimer();
            long dataCount = 0;
            timer.Unpause();

            inputPath = Path.Combine(inputPath, "Quest.nx");
            outputPath = Path.Combine(outputPath, "quests.bin");

            List<QuestData> quests = new List<QuestData>();

            using (NXFile file = new NXFile(inputPath))
            {
                Application.ResetCounter(file.BaseNode["QuestInfo.img"].ChildCount);

                foreach (var node in file.BaseNode["QuestInfo.img"])
                {
                    int questID = node.GetID<int>();

                    QuestData quest = new QuestData();

                    quest.ID = questID;
                    quest.Name = node.GetString("name");
                    quest.AutoStart = node.GetBoolean("autoStart");
                    quest.AutoPreComplete = node.GetBoolean("autoPreComplete");

                    foreach (var stateNode in file.BaseNode["Act.img"][questID.ToString()])
                    {
                        if (stateNode.ChildCount == 0)
                        {
                            continue;
                        }

                        QuestRewardData reward = new QuestRewardData();

                        reward.NextQuest = stateNode.GetInt("nextQuest");
                        reward.Experience = stateNode.GetInt("exp");
                        reward.Meso = stateNode.GetInt("money");
                        reward.Fame = stateNode.GetInt("pop");
                        reward.PetTameness = stateNode.GetInt("pettameness");
                        reward.PetSpeed = stateNode.GetInt("petspeed");
                        reward.Buff = stateNode.GetInt("buffItemID");
                        reward.Info = stateNode.GetString("info");
                        reward.Message = stateNode.GetString("message");
                        reward.NpcAction = stateNode.GetString("npcAct");

                        reward.Maps = new List<long>();
                        if (stateNode.ContainsChild("map"))
                        {
                            foreach (var mapNode in stateNode["map"])
                            {
                                long mapId = mapNode.ValueOrDie<long>();

                                reward.Maps.Add(mapId);
                            }
                        }

                        Func<string, int> convertPotentialGrade = new Func<string, int>((grade) =>
                        {
                            switch (grade)
                            {
                                case "normal":
                                case "Normal": return 0;
                                case "rare":
                                case "Rare": return 1;
                                case "Epic": return 2;
                                case "Unique": return 3;
                                case "노말": // amq?
                                case "Grade C": return -1;//????
                            }

                            throw new Exception("Unknown potential " + grade + "!");
                        });

                        reward.Items = new List<QuestRewardItemData>();
                        if (stateNode.ContainsChild("item"))
                        {
                            foreach (var itemNode in stateNode["item"])
                            {
                                QuestRewardItemData item = new QuestRewardItemData();

                                item.Named = itemNode.GetBoolean("name");
                                item.UntilMidnight = itemNode.GetBoolean("untilMidNight");
                                item.ResignRemove = itemNode.GetBoolean("resignRemove");
                                item.Id = itemNode.GetInt("id");
                                item.Quantity = itemNode.GetInt("count");
                                item.Meso = 0; // TODO
                                item.Period = itemNode.GetInt("period");
                                item.Gender = itemNode.GetInt("gender");
                                item.ProbRate = itemNode.GetInt("prop");
                                item.PotentialGrade = convertPotentialGrade(itemNode.GetString("potentialGrade"));
                                item.PotentialCount = itemNode.GetInt("potentialCount");
                                item.MadeBySkill = 0;
                                item.AdditionalOptCount = 0;
                                item.AdditionalGrade = 0;
                                item.DateExpire = itemNode.GetULong("dateExpire");

                                reward.Items.Add(item);
                            }
                        }

                        reward.Skills = new List<QuestRewardSkillData>();
                        if (stateNode.ContainsChild("skill"))
                        {

                        }

                        if (stateNode.Name == "0") quest.StartRewards = reward;
                        else if (stateNode.Name == "1") quest.CompleteRewards = reward;
                    }

                    quests.Add(quest);
                    ++dataCount;
                    ++Application.AllDataCounter;
                    Application.IncrementCounter();
                }
            }

            using (FileStream stream = File.Create(outputPath))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(quests.Count);
                    quests.ForEach(q => q.Save(writer));
                }
            }

            timer.Pause();
            Console.WriteLine("| {0,-24} | {1,-16} | {2,-24} |", "Quests", dataCount, timer.Duration);
        }
    }
}
