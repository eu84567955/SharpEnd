using SharpEnd.Network;
using SharpEnd.Packets;
using SharpEnd.Utility;
using System;
using System.Collections.Generic;

namespace SharpEnd.Players
{
    internal class BonusSet
    {
        public int Strength = 0;
        public int Dexterity = 0;
        public int Intelligence = 0;
        public int Luck = 0;
        public int Health = 0;
        public int Mana = 0;
    }

    internal class EquipBonus : BonusSet
    {
        public int Identifier = 0;
    }

    internal sealed class PlayerStats
    {
        private Player m_player;

        public byte Level { get; private set; }
        public ushort Job { get; private set; }
        public ushort SubJob { get; private set; }
        public ushort Strength { get; private set; }
        public ushort Dexterity { get; private set; }
        public ushort Intelligence { get; private set; }
        public ushort Luck { get; private set; }
        public uint Health { get; private set; }
        public uint MaxHealth { get; private set; }
        public uint Mana { get; private set; }
        public uint MaxMana { get; private set; }
        public ushort AbilityPoints { get; private set; }
        public byte[] SkillPoints { get; private set; }
        public ulong Experience { get; private set; }
        public int Fame { get; private set; }

        public bool IsAlive => Health > 0;

        public ushort StrengthWithBonus => (ushort)(Strength + mEquipBonuses.Strength + mBuffBonuses.Strength);
        public ushort DexterityWithBonus => (ushort)(Dexterity + mEquipBonuses.Dexterity + mBuffBonuses.Dexterity);
        public ushort IntelligenceWithBonus => (ushort)(Intelligence + mEquipBonuses.Intelligence + mBuffBonuses.Intelligence);
        public ushort LuckWithBonus => (ushort)(Luck + mEquipBonuses.Luck + mBuffBonuses.Luck);
        public uint RealMaxHealth => (uint)(Math.Min(MaxHealth + mEquipBonuses.Health + mBuffBonuses.Health, 500000));
        public uint RealMaxMana => (uint)(Math.Min(MaxMana + mEquipBonuses.Mana + mBuffBonuses.Mana, 500000));

        private BonusSet mEquipBonuses;
        private BonusSet mBuffBonuses;
        private Dictionary<short, EquipBonus> mEquipStats;

        public PlayerStats(Player player, DatabaseQuery query)
        {
            m_player = player;

            Level = query.Get<byte>("level");
            Job = query.Get<ushort>("job");
            SubJob = query.Get<ushort>("sub_job");
            Strength = query.Get<ushort>("strength");
            Dexterity = query.Get<ushort>("dexterity");
            Intelligence = query.Get<ushort>("intelligence");
            Luck = query.Get<ushort>("luck");
            Health = query.Get<uint>("health");
            MaxHealth = query.Get<uint>("max_health");
            Mana = query.Get<uint>("mana");
            MaxMana = query.Get<uint>("max_mana");
            AbilityPoints = query.Get<ushort>("ability_points");
            SkillPoints = query.Get<byte[]>("skill_points");
            Experience = query.Get<ulong>("experience");
            Fame = query.Get<int>("fame");

            if (!IsAlive)
            {
                Health = 50;
            }

            mEquipBonuses = new BonusSet();
            mBuffBonuses = new BonusSet();
            mEquipStats = new Dictionary<short, EquipBonus>();
        }

        public void WriteInitial(OutPacket outPacket)
        {
            outPacket
                .WriteByte(Level)
                .WriteUShort(Job)
                .WriteUShort(Strength)
                .WriteUShort(Dexterity)
                .WriteUShort(Intelligence)
                .WriteUShort(Luck)
                .WriteUInt(Health)
                .WriteUInt(MaxHealth)
                .WriteUInt(Mana)
                .WriteUInt(MaxMana)
                .WriteUShort(AbilityPoints);

            if (GameLogicUtilities.HasSeparatedSkillPoints(Job))
            {
                List<int> points = new List<int>();

                for (int i = 0; i < SkillPoints.Length; i++)
                {
                    if (SkillPoints[i] != 0)
                    {
                        points.Add(SkillPoints[i]);
                    }
                }

                outPacket.WriteByte((byte)points.Count);

                foreach (var p in points)
                {
                    outPacket
                        .WriteByte()
                        .WriteInt(p);
                }
            }
            else
            {
                outPacket.WriteUShort(SkillPoints[0]);
            }

            outPacket
                .WriteULong(Experience)
                .WriteInt(Fame);
        }

        private void UpdateBonuses(bool updateEquips = false, bool isLoading = false)
        {
            // TODO: Maple Warrior

            if (updateEquips)
            {
                // TODO: Update equips
            }

            // TODO: Hyper Body

            if (!isLoading)
            {
                // TODO: Health/Mana adjustment
            }
        }

        public void SetEquip(short slot, PlayerItem equip, bool isLoading)
        {
            slot = Math.Abs(slot);

            if (equip != null)
            {
                if (mEquipStats.ContainsKey(slot))
                {
                    throw new InvalidOperationException("The equipped slot is already taken.");
                }

                EquipBonus bonus = new EquipBonus();

                bonus.Identifier = equip.Identifier;
                bonus.Strength = equip.Strength;
                bonus.Dexterity = equip.Dexterity;
                bonus.Intelligence = equip.Intelligence;
                bonus.Luck = equip.Luck;
                bonus.Health = equip.Health;
                bonus.Mana = equip.Mana;

                mEquipStats.Add(slot, bonus);
            }
            else
            {
                mEquipStats.Remove(slot);
            }

            UpdateBonuses(true, isLoading);
        }

        public void CheckHealthMana()
        {
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            if (Mana > MaxMana)
            {
                Mana = MaxMana;
            }
        }

        public void SetLevel(byte value)
        {
            Level = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Level, Level));
            //m_player.SendMap(LevelsPackets.LevelUp(m_player.Identifier));
        }

        public void SetHealth(ushort value, bool sendPacket = true)
        {
            Health = value; // TODO: Check constrain range

            if (sendPacket)
            {
                m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Health, Health));
            }
        }

        public void ModifyHealth(uint mod, bool sendPacket = true)
        {
            uint tempHealth = Health + mod; // TODO: Check constrain range

            Health = (ushort)tempHealth;

            if (sendPacket)
            {
                m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Health, Health));
            }

            ModifiedHealth();
        }

        public void DamageHealth(int damage)
        {
            Health = (uint)(Math.Max(0, (Health - damage)));

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Health, Health));

            ModifiedHealth();
        }

        private void ModifiedHealth()
        {
            // TODO: Update party health bar

            // TODO: Check Berserk

            if (Health == 0)
            {
                /*if (m_player.Instance != null)
                {
                    m_player.Instance.PlayerDeath(m_player.Identifier);
                }*/

                LoseExperience();

                // TODO: Remove summons
            }
        }

        public void SetMana(ushort value, bool sendPacket = false)
        {
            // TODO: Check for infinity

            Mana = value; // TODO: Check constrain range

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Mana, Mana));
        }

        public void ModifyMana(uint mod, bool sendPacket = false)
        {
            // TODO: Check for infinity

            uint tempMana = Mana + mod; // TODO: Check constrain range

            Mana = (ushort)tempMana;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Mana, Mana, sendPacket));
        }

        public void DamageMana(int damage)
        {
            // TODO: Check for infinity

            Mana = (ushort)(Math.Max(50, (Mana - damage)));

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Mana, Mana));
        }

        public void SetAbilityPoints(ushort value)
        {
            AbilityPoints = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.AbilityPoints, AbilityPoints));
        }

        /*public void SetSkillPoints(ushort value)
        {
            SkillPoints = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Sp, SkillPoints));
        }*/

        public void SetJob(ushort value)
        {
            Job = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Job, Job));
            //m_player.SendMap(LevelsPackets.JobChange(m_player.Identifier));
        }

        public void SetStrength(ushort value)
        {
            Strength = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Strength, Strength));
        }

        public void SetDexterity(ushort value)
        {
            Dexterity = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Dexterity, Dexterity));
        }

        public void SetIntelligence(ushort value)
        {
            Intelligence = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Intelligence, Intelligence));
        }

        public void SetLuck(ushort value)
        {
            Luck = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Luck, Luck));
        }

        public void SetMaxHealth(ushort value)
        {
            MaxHealth = value; // TODO: Check constrain range

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.MaxHealth, MaxHealth));

            ModifiedHealth();
        }

        public void SetMaxMana(ushort value)
        {
            MaxMana = value; // TODO: Check constrain range

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.MaxMana, MaxMana));
        }

        public void ModifyMaxHealth(ushort mod)
        {
            MaxHealth = (ushort)(Math.Min((MaxHealth + mod), 500000));

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.MaxHealth, MaxHealth));
        }

        public void ModifyMaxMana(ushort mod)
        {
            MaxMana = (ushort)(Math.Min((MaxMana + mod), 500000));

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.MaxMana, MaxMana));
        }

        public void SetExperience(uint value)
        {
            Experience = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Experience, (int)Experience));
        }

        public void SetFame(short value)
        {
            Fame = value;

            m_player.Send(PlayerPackets.PlayerStatUpdate(EStatisticType.Fame, Fame));
        }

        public void LoseExperience()
        {
            /*if (!GameLogicUtilities.IsBeginnerJob(Job) && Level < Stats.PlayerLevels && m_player.Map != 270020211)
            {
                // TODO: Experience charms

                sbyte experienceLoss = 10;

                if (m_player.Channel.GetMap(m_player.Map).LoseOnePercent)
                {
                    experienceLoss = 1;
                }
                else
                {
                    switch (GameLogicUtilities.GetJobLine(Job))
                    {
                        case Jobs.JobLines.Magician:
                            experienceLoss = 7;
                            break;

                        case Jobs.JobLines.Thief:
                            experienceLoss = 5;
                            break;
                    }
                }

                uint experience = Experience;

                experience -= (uint)(GetExperience(Level) * experienceLoss / 100);

                SetExperience(experience);
            }*/
        }

        public void AddAbility(EStatisticType type, ushort mod = 1, bool reset = false)
        {
            switch (type)
            {
                case EStatisticType.Strength:
                    {
                        if (Strength >= 999)
                        {
                            return;
                        }

                        SetStrength((ushort)(Strength + mod));
                    }
                    break;

                case EStatisticType.Dexterity:
                    {
                        if (Dexterity >= 999)
                        {
                            return;
                        }

                        SetDexterity((ushort)(Dexterity + mod));
                    }
                    break;

                case EStatisticType.Intelligence:
                    {
                        if (Intelligence >= 999)
                        {
                            return;
                        }

                        SetIntelligence((ushort)(Intelligence + mod));
                    }
                    break;

                case EStatisticType.Luck:
                    {
                        if (Luck >= 999)
                        {
                            return;
                        }

                        SetLuck((ushort)(Luck + mod));
                    }
                    break;
            }

            if (!reset)
            {
                SetAbilityPoints((ushort)(AbilityPoints - mod));
            }

            UpdateBonuses();
        }

        public void GiveExperience(ulong experience, bool inChat, bool white)
        {
            /*byte level = Level;

            byte maxLevel = Stats.PlayerLevels;

            if (level >= maxLevel)
            {
                // NOTE: Do not give experience to characters of max level or over

                return;
            }

            ulong currentExperience = Experience + experience;

            if (experience > 0)
            {
                ulong expCounter = experience;
                ulong batchSize = uint.MaxValue;

                while (expCounter > 0)
                {
                    uint allocate = (uint)(Math.Min(expCounter, batchSize));

                    m_player.Send(LevelsPackets.ShowExperience(allocate, white, inChat));

                    expCounter -= allocate;
                }
            }

            if (currentExperience >= GetExperience(level))
            {
                byte levelsGained = 0;
                byte levelsMax = 1; // TODO: maxMultiLevel configuration
                ushort abilityPointsGain = 0;
                ushort skillPointsGain = 0;
                ushort healthGain = 0;
                ushort manaGain = 0;
                Jobs.JobLines jobLine = GameLogicUtilities.GetJobLine(Job);
                short intelligence = (short)(Intelligence / 10);
                short x = 0; // NOTE: X Value for improving increase skills, cached, only needs to be set once

                while (currentExperience >= GetExperience(level) && levelsGained < levelsMax)
                {
                    currentExperience -= GetExperience(level);
                    level++;
                    levelsGained++;

                    abilityPointsGain += Stats.AbilityPointsPerLevel;

                    switch (jobLine)
                    {
                        // TODO: HP addition bonuses
                    }

                    if (Job != 0)
                    {
                        skillPointsGain += Stats.SkillPointsPerLevel;
                    }

                    // NOTE: Do not let people level past the level cap
                    if (level >= maxLevel)
                    {
                        currentExperience = 0;

                        break;
                    }
                }

                // NOTE: Check if the m_player has leveled up at all, it is possible that the user hasn't leveled up if multi-level limit is 0
                if (levelsGained != 0)
                {
                    ModifyMaxHealth(healthGain);
                    ModifyMaxMana(manaGain);
                    SetLevel(level);
                    SetAbilityPoints((ushort)(AbilityPoints + abilityPointsGain));
                    SetSkillPoints((ushort)(SkillPoints + skillPointsGain));

                    // NOTE: Let Hyper Body remain on during a level up, as it should
                    // TODO: Hyper body stuff

                    SetHealth(MaxHealth);
                    SetMana(MaxMana);

                    if (Level == maxLevel && true) // TODO: isGm check
                    {
                        string message = string.Format("[Congrats] {0} has reached Level {1}! Congratulate {0} on such an amazing achievement!", m_player.Name, maxLevel);

                        // TODO: Notify the world about the stupid idiot who actually spent so much time on my server to reach Lv. 200.
                    }
                }
            }

            // NOTE: By this point, the experience should be a valid experience in the range of 0 to max
            SetExperience((uint)currentExperience);
            */
        }

        /*private ushort RandHealth()
        {

        }

        private ushort RandMana()
        {

        }

        private ushort LevelHealth(ushort value, ushort bonus = 0)
        {

        }

        private ushort LevelMana(ushort value, ushort bonus = 0)
        {

        }*/

        /*public uint GetExperience(byte level)
        {
            return Stats.PlayerExperience[level - 1];
        }*/
    }
}
