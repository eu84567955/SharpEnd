using SharpEnd.Game.Players;

namespace SharpEnd.Game.Commands.Gm
{
    public static class StatCommands
    {
        [GmCommand("level", "Sets your level to the desired level.")]
        public static void LevelCommand(Player player, byte value)
        {
            //player.Stats.SetLevel(value);
        }

        [GmCommand("job", "Sets your job to the desired job.")]
        public static void JobCommand(Player player, ushort value)
        {
           // player.Stats.SetJob(value);
        }

        [GmCommand("str", "Sets your strength to the desired value.")]
        public static void StrengthCommand(Player player, ushort value)
        {
            //player.Stats.SetStrength(value);
        }

        [GmCommand("dex", "Sets your dexterity to the desired value.")]
        public static void DexterityCommand(Player player, ushort value)
        {
            //player.Stats.SetDexterity(value);
        }

        [GmCommand("int", "Sets your intelligence to the desired value.")]
        public static void IntelligenceCommand(Player player, ushort value)
        {
           // player.Stats.SetIntelligence(value);
        }

        [GmCommand("luk", "Sets your luck to the desired value.")]
        public static void LuckCommand(Player player, ushort value)
        {
            //player.Stats.SetLuck(value);
        }

        [GmCommand("hp", "Sets your max health to the desired value.")]
        public static void HpCommand(Player player, ushort value)
        {
            //player.Stats.SetMaxHealth(value);
        }

        [GmCommand("mp", "Sets your max mana to the desired value.")]
        public static void MpCommand(Player player, ushort value)
        {
            //player.Stats.SetMaxMana(value);
        }

        [GmCommand("heal", "Seats your health and mana to their maximum values.")]
        public static void HealCommand(Player player)
        {
            //player.Stats.SetHealth(player.Stats.MaxHealth);
            //player.Stats.SetMana(player.Stats.MaxMana);
        }

        [GmCommand("ap", "Sets your ability points to the desired value.")]
        public static void ApCommand(Player player, ushort value)
        {
            //player.Stats.SetAbilityPoints(value);
        }

        [GmCommand("sp", "Sets your skill points to the desired value.")]
        public static void SpCommand(Player player, ushort value)
        {
            //player.Stats.SetSkillPoints(value);
        }
    }
}
