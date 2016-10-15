﻿using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SharpEnd.Players;
using SharpEnd.Servers;
using System;

namespace SharpEnd.Script
{
    internal abstract class ScriptBase
    {
        private enum VariableType : int
        {
            Boolean,
            String,
            Number,
            Integer
        }

        protected readonly Player m_player;

        private readonly string m_path;
        private readonly ScriptEngine m_engine;
        private readonly ScriptScope m_scope;

        protected ScriptBase(Player player, string path)
        {
            m_player = player;

            m_path = path;
            m_engine = Python.CreateEngine();
            m_scope = m_engine.CreateScope();

            SetEnvironmentVariables();
            SetBaseVariables();
        }

        private void SetEnvironmentVariables()
        {
            // NOTE: Return types.
            Set("type_bool", VariableType.Boolean);
            Set("type_int", VariableType.Integer);
            Set("type_num", VariableType.Number);
            Set("type_str", VariableType.String);

            // NOTE: Notification types.
            Set("notice_notice", ENoticeType.Notice);
            Set("notice_popup", ENoticeType.Popup);
            Set("notice_megaphone", ENoticeType.Megaphone);
            Set("notice_super_megaphone", ENoticeType.SuperMegaphone);
            Set("notice_Header", ENoticeType.Header);
            Set("notice_pink", ENoticeType.Pink);
            Set("notice_blue", ENoticeType.Blue);
        }

        private void SetBaseVariables()
        {
            // NOTE: Map exports.
            Set("getMap", new Func<int>(() => m_player.MapIdentifier));
            Set("setMap", new Action<int, string>((mapIdentifier, portalIdentifier) =>
            {
                m_player.SetMap(mapIdentifier, MasterServer.Instance.GetMap(mapIdentifier).Portals[portalIdentifier]);
            }));
            Set("getMapPlayerCount", new Func<int>(() => m_player.Map.Players.Count));

            // NOTE: Inventory exports.
            Set("giveMeso", new Func<int, bool>((amount) =>
            {
                return true;
            }));
            Set("giveItem", new Func<int, ushort, bool>((itemIdentifier, quantity) =>
            {
                if (quantity < 0)
                {
                    return true;
                }
                else
                {
                    m_player.Items.Add(new PlayerItem(itemIdentifier, quantity));

                    return true;
                }
            }));

            // NOTE: Player exports.
            Set("notify", new Action<string, ENoticeType>((text, type) => m_player.Notify(text, type)));
            Set("getPlayerVariable", new Func<string, string>((key) =>
            {
                string value = null;

                m_player.Variables.TryGetValue(key, out value);

                return value;
            }));
            Set("removePlayerVariable", new Action<string>((key) => m_player.Variables.Remove(key)));
            Set("setPlayerVariable", new Action<string, object>((key, value) =>
            {
                if (m_player.Variables.ContainsKey(key))
                {
                    m_player.Variables[key] = value.ToString();
                }
                else
                {
                    m_player.Variables.Add(key, value.ToString());
                }
            }));
        }

        public dynamic Get(string name)
        {
            return m_scope.GetVariable(name);
        }

        protected void Set(string name, object value)
        {
            m_scope.SetVariable(name, value);
        }

        public virtual void Execute()
        {
            m_engine.ExecuteFile(m_path, m_scope);
        }
    }
}