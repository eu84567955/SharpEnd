using SharpEnd.Game.Players;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Scripting
{
    public sealed class PortalScriptManager
    {
        private static PortalScriptManager instance;

        public static PortalScriptManager Instance
        {
            get
            {
                return instance ?? (instance = new PortalScriptManager());
            }
        }

        private string m_portalPath;
        private Dictionary<int, bool> m_playersBeingFulfilled;

        private PortalScriptManager()
        {
            m_portalPath = "scripts/portals/";
            m_playersBeingFulfilled = new Dictionary<int, bool>();
        }

        public bool RunScript(Player player, string scriptName, string portalID)
        {
            if (m_playersBeingFulfilled.ContainsKey(player.Id))
            {
                return false;
            }

            m_playersBeingFulfilled.Add(player.Id, true);

            try
            {
                string contents = File.ReadAllText(m_portalPath + scriptName + ".py");
                Script script = new Script(contents);
                ScriptPortal scriptPortal = new ScriptPortal(script, player.Client, portalID);

                script.SetVariable("portal", scriptPortal);
                script.SetVariable("player", new ScriptPlayer(script, player));
                script.SetVariable("map", new ScriptMap(script, player.Map));

                script.Execute();

                return scriptPortal.Warped;
            }
            catch (FileNotFoundException)
            {
                Log.Warn("Missing portal script '{0}'.", scriptName);

                return false;
            }
            catch (Exception e)
            {
                Log.Error("Error executing portal script '{0}': \n{1}", scriptName, e.Message);

                return false;
            }
            finally
            {
                m_playersBeingFulfilled.Remove(player.Id);
            }
        }
    }
}
