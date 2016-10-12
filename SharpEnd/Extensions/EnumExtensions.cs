using System;

namespace SharpEnd
{
    internal static class EnumExtensions
    {
        public static EJob GetJob(this EJobType jobType)
        {
            switch (jobType)
            {
                case EJobType.Advenutrer: return EJob.Beginner;
                case EJobType.Resistance: return EJob.Citizen;
                case EJobType.Cygnus: return EJob.Noblesse;
                case EJobType.Aran: return EJob.Legend;
                case EJobType.Evan: return EJob.Evan;
                case EJobType.Mercedes: return EJob.Mercedes;
                case EJobType.Demon: return EJob.Demon;
                case EJobType.Phantom: return EJob.Phantom;
                case EJobType.DualBlade: return EJob.Beginner;
                case EJobType.Mihile: return EJob.Mihile;
                case EJobType.Luminous: return EJob.Luminous;
                case EJobType.Kaiser: return EJob.Kaiser;
                case EJobType.AngelicBuster: return EJob.AngelicBuster;
                case EJobType.Cannoneer: return EJob.Beginner;
                case EJobType.Xenon: return EJob.Xenon;
                case EJobType.Zero: return EJob.Zero;
                case EJobType.Jett: return EJob.Beginner;
                case EJobType.Hayato: return EJob.Hayato;
                case EJobType.Kanna: return EJob.Kanna;
                case EJobType.BeastTamer: return EJob.BeastTamer;
                default: throw new Exception();
            }
        }
    }
}
