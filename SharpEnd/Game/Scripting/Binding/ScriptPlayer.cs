using SharpEnd.Game.Players;
using SharpEnd.Network.Servers;
using System;

namespace SharpEnd.Game.Scripting
{
    public sealed class ScriptPlayer
    {
        private Script m_script;
        private WeakReference<Player> m_player;

        public Player Player
        {
            get
            {
                Player player = null;

                m_player.TryGetTarget(out player);

                return player;
            }
        }

        public ScriptPlayer(Script script, Player player)
        {
            m_script = script;
            m_player = new WeakReference<Player>(player);
        }

        public int GetId()
        {
            return Player.ID;
        }

        public string GetName()
        {
            return Player.Name;
        }

        public byte GetGender()
        {
            return Player.Gender;
        }

        public byte GetSkin()
        {
            return Player.Skin;
        }

        public int GetFace()
        {
            return Player.Face;
        }

        public int GetHair()
        {
            return Player.Hair;
        }

        public byte GetLevel()
        {
            return Player.Level;
        }

        public short GetJob()
        {
            return Player.Job;
        }

        /*public ushort GetSubJob()
        {
            return Player.Stats.SubJob;
        }*/

        public short GetStrength()
        {
            return Player.Strength;
        }

        public short GetDexterity()
        {
            return Player.Dexterity;
        }

        public short GetIntelligence()
        {
            return Player.Intelligence;
        }

        public short GetLuck()
        {
            return Player.Luck;
        }

        public int GetHealth()
        {
            return Player.HP;
        }

        public int GetMaxHealth()
        {
            return Player.MaxHP;
        }

        public int GetMana()
        {
            return Player.MP;
        }

        public int GetMaxMana()
        {
            return Player.MaxMP;
        }

        /*public ushort GetAbilityPoints()
        {
            return Player.Stats.AbilityPoints;
        }*/

        public long GetExperience()
        {
            return Player.Experience;
        }

        public int GetFame()
        {
            return Player.Fame;
        }

        /*public string GetVariable(string key)
        {
            string value = null;

            Player.Variables.TryGetValue(key, out value);

            return value;
        }

        public void SetVariable(string key, object value)
        {
            if (Player.Variables.ContainsKey(key))
            {
                Player.Variables[key] = value.ToString();
            }
            else
            {
                Player.Variables.Add(key, value.ToString());
            }
        }

        public void RemoveVariable(string key)
        {
            if (Player.Variables.ContainsKey(key))
            {
                Player.Variables.Remove(key);
            }
        }

        public void SetMap(int mapID)
        {
            Player.SetMap(mapID);
        }

        public void SetMap(int mapID, string portalID)
        {
            Player.SetMap(mapID, MasterServer.Instance.Worlds[Player.Client.WorldID][Player.Client.ChannelID].MapFactory.GetMap(mapID).Portals[portalID]);
        }*/
    }
}
