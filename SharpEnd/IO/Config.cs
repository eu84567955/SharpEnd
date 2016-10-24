using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharpEnd.Utility;
using System.IO;

namespace SharpEnd.IO
{
    public sealed class Config
    {
        private static Config m_instance;

        [JsonProperty("database")]
        private CDatabase m_databaseConfig;
        [JsonProperty("login")]
        private CLogin m_loginConfig;
        [JsonProperty("worlds")]
        private CWorld[] m_worldConfig;

        public static void Load()
        {
            Config instance = null;

            try
            {
                string contents = File.ReadAllText("config.json");

                instance = JsonConvert.DeserializeObject<Config>(contents);

                Log.Inform("Configuration file '{0}' loaded.", "config.json");
            }
            catch (FileNotFoundException)
            {
                instance = new Config
                {
                    m_databaseConfig = new CDatabase
                    {
                        Host = "localhost",
                        Schema = "sharpend",
                        Username = "root",
                        Password = string.Empty
                    },
                    m_loginConfig = new CLogin
                    {
                        RequestPin = false,
                        RequestPic = true,
                        RequireStaffIP = true,
                        DefaultCharacterSlots = 3,
                        Port = 8484,
                        InvalidLoginThreshold = 5,
                        RankingUpdateFrequency = 360000
                    },
                    m_worldConfig = new CWorld[1]
                    {
                        new CWorld
                        {
                            Channels = 2,
                            Port = 8585,
                            Name = "Scania",
                            EventMessage = string.Empty,
                            TickerMessage = string.Empty,
                            BackgroundEvents = new string[0],
                            Ribbon = EWorldRibbon.None,
                            ExperienceRate = 1,
                            MesoRate = 1,
                            DropRate = 1
                        }
                    }
                };

                string contents = JsonConvert.SerializeObject(instance, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                File.WriteAllText("config.json", contents);

                Log.Warn("Configuration file '{0}' was not found; created new one with default values.", "config.json");
            }

            m_instance = instance;

            Database.Host = m_instance.DatabaseConfig.Host;
            Database.Schema = m_instance.DatabaseConfig.Schema;
            Database.Username = m_instance.DatabaseConfig.Username;
            Database.Password = m_instance.DatabaseConfig.Password;
        }

        public static Config Instance { get { return m_instance; } }

        public CDatabase DatabaseConfig { get { return m_databaseConfig; } set { m_databaseConfig = value; } }
        public CLogin LoginConfig { get { return m_loginConfig; } set { m_loginConfig = value; } }
        public CWorld[] WorldConfig { get { return m_worldConfig; } set { m_worldConfig = value; } }
    }

    public struct CDatabase
    {
        public string Host { get; set; }
        public string Schema { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public struct CLogin
    {
        public bool RequestPin { get; set; }
        public bool RequestPic { get; set; }
        public bool RequireStaffIP { get; set; }
        public ushort Port { get; set; }
        public int DefaultCharacterSlots { get; set; }
        public int InvalidLoginThreshold { get; set; }
        public int RankingUpdateFrequency { get; set; }
    }

    public struct CWorld
    {
        public byte Channels { get; set; }
        public ushort Port { get; set; }
        public string Name { get; set; }
        public EWorldRibbon Ribbon { get; set; }
        public string EventMessage { get; set; }
        public string TickerMessage { get; set; }
        public string[] BackgroundEvents { get; set; }
        public int ExperienceRate { get; set; }
        public int MesoRate { get; set; }
        public int DropRate { get; set; }
    }
}
