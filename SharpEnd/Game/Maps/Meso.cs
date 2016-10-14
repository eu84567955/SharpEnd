namespace SharpEnd.Game.Maps
{
    internal sealed class Meso : Drop
    {
        public int Amount { get; private set; }

        public Meso(int amount)
            : base()
        {
            Amount = amount;
        }
    }
}
