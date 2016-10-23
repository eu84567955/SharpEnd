using SharpEnd.Network.Servers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpEnd.Game.Scripting
{
    public sealed class EventManager : Dictionary<string, Tuple<ScriptEvent, EventManipulator>>
    {
        private ChannelServer m_channel;
        private string[] m_backgroundEvents;

        public EventManager(ChannelServer channel, string[] backgroundEvents)
        {
            m_channel = channel;
            m_backgroundEvents = backgroundEvents;
        }

        public void Initialize()
        {
            foreach (string script in m_backgroundEvents)
            {
                RunScript(script, true, null);
            }
        }

        public ScriptEvent GetRunningScript(string name)
        {
            return this.GetOrDefault(name, null).Item1;
        }

        public ScriptEvent RunScript(string name, bool assosicateWithName, object attachment)
        {
            ScriptEvent scriptEvent = null;
            EventManipulator delegator;

            try
            {
                string contents = File.ReadAllText("scripts/events/" + name + ".py");
                Script script = new Script(contents);
                delegator = new EventManipulator(script);
                scriptEvent = new ScriptEvent(script, name, m_channel, delegator);

                if (assosicateWithName && ContainsKey(name))
                {
                    return null;
                }

                Add(name, new Tuple<ScriptEvent, EventManipulator>(scriptEvent, delegator));

                script.SetVariable("event", scriptEvent);

                script.Execute();

                if (script.ContainsVariable("init"))
                {
                    script.GetVariable("init")(attachment);
                }
            }
            catch (FileNotFoundException)
            {
                Log.Warn("Missing event script '{0}'", name);
            }
            catch (Exception e)
            {
                Log.Error("Error executing event script '{0}': \n{1}", name, e.Message);
            }

            return scriptEvent;
        }
    }
}
