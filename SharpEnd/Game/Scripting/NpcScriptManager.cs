using SharpEnd.Game.Life;
using SharpEnd.Network;
using System;
using System.IO;

namespace SharpEnd.Game.Scripting
{
    public sealed class NpcScriptManager
    {
        private static NpcScriptManager instance;

        public static NpcScriptManager Instance
        {
            get
            {
                return instance ?? (instance = new NpcScriptManager());
            }
        }

        private string m_npcPath;
        private string m_questPath;

        private NpcScriptManager()
        {
            m_npcPath = "scripts/npcs/";
            m_questPath = "scripts/quests/";
        }

        private bool OpenDefault(GameClient client, int npcID)
        {
            // TODO: Check for shop.
            // TODO: Check for storage.

            if (npcID == 9010009)
            {
                // TODO: Maple Package Service.

                return true;
            }

            if (npcID == 9201066 || npcID >= 9250023 && npcID <= 9250026 ||
                npcID >= 9250042 && npcID <= 9250046 || npcID >= 9270000 &&
                npcID <= 9270016 || npcID == 9270040)
            {
                // TODO: Maple TV.

                return true;
            }
            return false;
        }

        public void RunScript(GameClient client, Npc npc)
        {
            int npcId = npc.ID;
            string scriptName = "";

            if (scriptName == null)
            {
                if (!OpenDefault(client, npcId))
                {
                    // TODO: Missing shop packet.
                }

                return;
            }

            try
            {
                string contents = File.ReadAllText(m_npcPath + scriptName + ".py");
                Script script = new Script(contents);
                ScriptNpc scriptNpc = new ScriptNpc(script, client, npcId);

                script.SetVariable("npc", scriptNpc);
                script.SetVariable("player", new ScriptPlayer(script, client.Player));
                script.SetVariable("map", new ScriptMap(script, client.Player.Map));

                script.Execute();
            }
            catch (FileNotFoundException)
            {
                // TODO: Missing npc packet.
            }
            catch (Exception e)
            {
                Log.Error("Error executing npc script '{0}': \n{1}", scriptName, e.Message);
            }
        }

        public void RunQuestScript(GameClient client, int npcId, ushort questId, string scriptName)
        {
            try
            {
                string contents = File.ReadAllText(m_questPath + scriptName + ".py");
                Script script = new Script(contents);
                ScriptQuest scriptQuest = new ScriptQuest(script, client, npcId, questId);

                script.SetVariable("npc", scriptQuest);
                script.SetVariable("player", new ScriptPlayer(script, client.Player));
                script.SetVariable("map", new ScriptMap(script, client.Player.Map));

                script.Execute();
            }
            catch (FileNotFoundException)
            {
                // TODO: Missing quest packet.
            }
            catch (Exception e)
            {
                Log.Error("Error executing quest script '{0}': \n{1}", scriptName, e.Message);
            }
        }
    }
}
